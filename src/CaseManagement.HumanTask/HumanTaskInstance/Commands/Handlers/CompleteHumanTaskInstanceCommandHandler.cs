using CaseManagement.Common.Exceptions;
using CaseManagement.Common.Expression;
using CaseManagement.Common.Factories;
using CaseManagement.HumanTask.Domains;
using CaseManagement.HumanTask.Exceptions;
using CaseManagement.HumanTask.Parser;
using CaseManagement.HumanTask.Persistence;
using CaseManagement.HumanTask.Resources;
using IdentityModel.Client;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.HumanTask.HumanTaskInstance.Commands.Handlers
{
    public class CompleteHumanTaskInstanceCommandHandler : IRequestHandler<CompleteHumanTaskInstanceCommand, bool>
    {
        private readonly IHumanTaskInstanceQueryRepository _humanTaskInstanceQueryRepository;
        private readonly IHumanTaskInstanceCommandRepository _humanTaskInstanceCommandRepository;
        private readonly IParameterParser _parameterParser;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HumanTaskServerOptions _options;
        private readonly ILogger<CompleteHumanTaskInstanceCommandHandler> _logger;

        public CompleteHumanTaskInstanceCommandHandler(
            IHumanTaskInstanceQueryRepository humanTaskInstanceQueryRepository,
            IHumanTaskInstanceCommandRepository humanTaskInstanceCommandRepository,
            IParameterParser parameteParser,
            IHttpClientFactory httpClientFactory,
            IOptions<HumanTaskServerOptions> options,
            ILogger<CompleteHumanTaskInstanceCommandHandler> logger)
        {
            _humanTaskInstanceQueryRepository = humanTaskInstanceQueryRepository;
            _humanTaskInstanceCommandRepository = humanTaskInstanceCommandRepository;
            _parameterParser = parameteParser;
            _httpClientFactory = httpClientFactory;
            _options = options.Value;
            _logger = logger;
        }

        public async Task<bool> Handle(CompleteHumanTaskInstanceCommand request, CancellationToken cancellationToken)
        {
            if (request.Claims == null || !request.Claims.Any())
            {
                _logger.LogError("User is not authenticated");
                throw new NotAuthenticatedException(Global.UserNotAuthenticated);
            }

            if (request.OperationParameters == null)
            {
                _logger.LogError("Output data must be specified");
                throw new BadRequestException(string.Format(Global.MissingParameter, "operationParameters"));
            }

            var humanTaskInstance = await _humanTaskInstanceQueryRepository.Get(request.HumanTaskInstanceId, cancellationToken);
            if (humanTaskInstance == null)
            {
                _logger.LogError($"HumanTask '{request.HumanTaskInstanceId}' doesn't exist");
                throw new UnknownHumanTaskInstanceException(string.Format(Global.UnknownHumanTaskInstance, request.HumanTaskInstanceId));
            }

            if (humanTaskInstance.Status != HumanTaskInstanceStatus.INPROGRESS)
            {
                _logger.LogError("Complete operation can be performed only on INPROGRESS human task instance");
                throw new BadOperationExceptions(string.Format(Global.OperationCanBePerformed, "Complete", "INPROGRESS"));
            }

            var operationParameters = request.OperationParameters;
            var parameters = _parameterParser.ParseOperationParameters(humanTaskInstance.OutputOperationParameters, operationParameters);
            var nameIdentifier = request.Claims.GetUserNameIdentifier();
            if (nameIdentifier != humanTaskInstance.ActualOwner)
            {
                _logger.LogError("Authenticated user is not the actual owner");
                throw new NotAuthorizedException(Global.NotActualOwner);
            }

            var completionResult = await CheckCompletionBehavior(humanTaskInstance, parameters, cancellationToken);
            if (completionResult.IsSatisfied)
            {
                parameters = completionResult.Content;
            }

            await Complete(humanTaskInstance, parameters, nameIdentifier, cancellationToken);
            if(!string.IsNullOrWhiteSpace(humanTaskInstance.ParentHumanTaskId))
            {
                var parentTask = await _humanTaskInstanceQueryRepository.Get(humanTaskInstance.ParentHumanTaskId, cancellationToken);
                if (parentTask.CompletionBehavior == CompletionBehaviors.AUTOMATIC)
                {
                    completionResult = await CheckCompletionBehavior(parentTask, new Dictionary<string, string>(), cancellationToken, false);
                    if (completionResult.IsSatisfied)
                    {
                        await Complete(parentTask, new Dictionary<string, string>(), nameIdentifier, cancellationToken);
                    }
                }
            }

            foreach(var cb in humanTaskInstance.CallbackOperations)
            {
                var jObj = new JObject
                {
                    { "parameters", parameters.ToJObj() }
                };
                using (var httpClient = _httpClientFactory.Build())
                {
                    var tokenResponse = await httpClient.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
                    {
                        Address = _options.OAuthTokenEndpoint,
                        ClientId = _options.ClientId,
                        ClientSecret = _options.ClientSecret,
                        Scope = _options.Scope
                    }, cancellationToken);
                    var req = new HttpRequestMessage
                    {
                        Method = HttpMethod.Post,
                        RequestUri = new Uri(cb.Url),
                        Content = new StringContent(jObj.ToString(), Encoding.UTF8, "application/json")
                    };
                    req.Headers.Add("Authorization", $"Bearer {tokenResponse.AccessToken}");
                    await httpClient.SendAsync(req, cancellationToken);
                }
            }

            return true;
        }

        private class CompletionBehaviorResult
        {
            public bool IsSatisfied { get; set; }
            public Dictionary<string, string> Content { get; set; }
        }

        private async Task<CompletionBehaviorResult> CheckCompletionBehavior(HumanTaskInstanceAggregate humanTaskInstance, Dictionary<string, string> parameters, CancellationToken token, bool raiseException = true)
        {
            if (humanTaskInstance.Completions == null || !humanTaskInstance.Completions.Any())
            {
                return new CompletionBehaviorResult
                {
                    IsSatisfied = false
                };
            }

            var subTasks = await _humanTaskInstanceQueryRepository.GetSubTasks(humanTaskInstance.AggregateId, token);
            var executionContext = new HumanTaskInstanceExpressionContext(humanTaskInstance, parameters, subTasks);
            var result = new Dictionary<string, string>();
            foreach (var completion in humanTaskInstance.Completions)
            {
                if (string.IsNullOrWhiteSpace(completion.Condition) || ExpressionParser.IsValid(completion.Condition, executionContext))
                {
                    foreach (var copy in completion.CopyLst)
                    {
                        result.Add(copy.To, ExpressionParser.GetString(copy.From, executionContext));
                    }
                    break;
                }
            }

            if (raiseException && !result.Any())
            {
                _logger.LogError("At least one completion behavior must be satisfied");
                throw new BadOperationExceptions(Global.CompletionBehaviorMustBeSatisfied);
            }

            return new CompletionBehaviorResult
            {
                IsSatisfied = true,
                Content = result
            };
        }

        private async Task Complete(HumanTaskInstanceAggregate humanTaskInstance, Dictionary<string, string> parameters, string nameIdentifier, CancellationToken token)
        {
            humanTaskInstance.Complete(parameters, nameIdentifier);
            await _humanTaskInstanceCommandRepository.Update(humanTaskInstance, token);
            var subTasks = await _humanTaskInstanceQueryRepository.GetSubTasks(humanTaskInstance.AggregateId, token);
            foreach(var subTask in subTasks)
            {
                if (subTask.Status == HumanTaskInstanceStatus.CREATED ||
                    subTask.Status == HumanTaskInstanceStatus.READY ||
                    subTask.Status == HumanTaskInstanceStatus.RESERVED ||
                    subTask.Status == HumanTaskInstanceStatus.INPROGRESS)
                {
                    subTask.Skip(nameIdentifier);
                    await _humanTaskInstanceCommandRepository.Update(subTask, token);
                }
            }

            await _humanTaskInstanceCommandRepository.SaveChanges(token);
        }
    }
}
