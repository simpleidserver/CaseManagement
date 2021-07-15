using CaseManagement.HumanTask.Authorization;
using CaseManagement.HumanTask.Domains;
using CaseManagement.HumanTask.Exceptions;
using CaseManagement.HumanTask.HumanTaskInstance.Queries.Results;
using CaseManagement.HumanTask.Parser;
using CaseManagement.HumanTask.Persistence;
using CaseManagement.HumanTask.Resources;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.HumanTask.HumanTaskInstance.Commands.Handlers
{
    public class CreateHumanTaskInstanceCommandHandler : IRequestHandler<CreateHumanTaskInstanceCommand, TaskInstanceCreatedResult>
    {
        private readonly IHumanTaskDefQueryRepository _humanTaskDefQueryRepository;
        private readonly IHumanTaskDefCommandRepository _humanTaskDefCommandRepository;
        private readonly IHumanTaskInstanceCommandRepository _humanTaskInstanceCommandRepository;
        private readonly IAuthorizationHelper _authorizationHelper;
        private readonly IParameterParser _parameterParser;
        private readonly IDeadlineParser _deadlineParser;
        private readonly ILogger<CreateHumanTaskInstanceCommandHandler> _logger;

        public CreateHumanTaskInstanceCommandHandler(
            IHumanTaskDefQueryRepository humanTaskDefQueryRepository,
            IHumanTaskInstanceCommandRepository humanTaskInstanceCommandRepository,
            IHumanTaskDefCommandRepository humanTaskDefCommandRepository,
            IAuthorizationHelper authorizationHelper,
            IParameterParser parameterParser,
            IDeadlineParser deadLineParser,
            ILogger<CreateHumanTaskInstanceCommandHandler> logger)
        {
            _humanTaskDefQueryRepository = humanTaskDefQueryRepository;
            _humanTaskDefCommandRepository = humanTaskDefCommandRepository;
            _humanTaskInstanceCommandRepository = humanTaskInstanceCommandRepository;
            _authorizationHelper = authorizationHelper;
            _parameterParser = parameterParser;
            _deadlineParser = deadLineParser;
            _logger = logger;
        }

        public async Task<TaskInstanceCreatedResult> Handle(CreateHumanTaskInstanceCommand request, CancellationToken cancellationToken)
        {
            if (request.Claims == null || !request.Claims.Any())
            {
                _logger.LogError("User is not authenticated");
                throw new NotAuthenticatedException(Global.UserNotAuthenticated);
            }

            var humanTaskDef = await _humanTaskDefQueryRepository.GetLatest(request.HumanTaskName, cancellationToken);
            if (humanTaskDef == null)
            {
                _logger.LogError($"Human task definition '{request.HumanTaskName}' doesn't exist");
                throw new UnknownHumanTaskDefException(string.Format(Global.UnknownHumanTaskDef, request.HumanTaskName));
            }

            var operationParameters = request.OperationParameters == null ? new Dictionary<string, string>() : request.OperationParameters;
            var parameters = _parameterParser.ParseOperationParameters(humanTaskDef.InputParameters, operationParameters);
            var assignment = EnrichAssignment(humanTaskDef, request);
            var priority = EnrichPriority(humanTaskDef, request);
            var assignmentInstance = _parameterParser.ParsePeopleAssignments(assignment, parameters);
            if (!request.IgnorePermissions)
            {
                var roles = await _authorizationHelper.GetRoles(assignmentInstance, request.Claims, cancellationToken);
                if (!roles.Any(r => r == UserRoles.TASKINITIATOR))
                {
                    _logger.LogError("User is not a task initiator");
                    throw new NotAuthorizedException(Global.UserNotAuthorized);
                }
            }

            _logger.LogInformation($"Create human task '{request.HumanTaskName}'");
            var userPrincipal = request.NameIdentifier;
            var id = Guid.NewGuid().ToString();
            var deadLines = _deadlineParser.Evaluate(humanTaskDef.DeadLines, parameters);
            var presentationElements = _parameterParser.ParsePresentationElements(humanTaskDef.PresentationElements, humanTaskDef.PresentationParameters, operationParameters);
            var humanTaskInstance = HumanTaskInstanceAggregate.New(
                id, 
                userPrincipal, 
                request.HumanTaskName, 
                humanTaskDef.ValidatorFullQualifiedName,
                parameters,
                assignmentInstance, 
                priority, 
                request.ActivationDeferralTime, 
                request.ExpirationTime,
                deadLines,
                presentationElements,
                humanTaskDef.Type,
                humanTaskDef.InstantiationPattern,
                humanTaskDef.SubTasks.Select(_ => new HumanTaskInstanceSubTask
                {
                    HumanTaskName = _.TaskName,
                    ToParts = _.ToParts
                }).ToList(),
                humanTaskDef.OperationParameters,
                humanTaskDef.CompletionAction,
                humanTaskDef.Completions,
                humanTaskDef.Rendering,
                request.CallbackUrls == null ? new List<CallbackOperation>() : request.CallbackUrls.Select(_ => new CallbackOperation
                {
                    Id = Guid.NewGuid().ToString(),
                    Url = _
                }).ToList());
            await _humanTaskInstanceCommandRepository.Add(humanTaskInstance, cancellationToken);
            await _humanTaskInstanceCommandRepository.SaveChanges(cancellationToken);
            humanTaskDef.IncrementInstance();
            await _humanTaskDefCommandRepository.Update(humanTaskDef, cancellationToken);
            return new TaskInstanceCreatedResult
            {
                Id = humanTaskInstance.AggregateId,
                DefId = humanTaskDef.AggregateId
            };
        }

        private static ICollection<PeopleAssignmentDefinition> EnrichAssignment(HumanTaskDefinitionAggregate humanTaskDef, CreateHumanTaskInstanceCommand request)
        {
            var result = humanTaskDef.PeopleAssignments.ToList();
            if (request.PeopleAssignments != null)
            {
                foreach(var peopleAssignment in request.PeopleAssignments)
                {
                    result.Add(new PeopleAssignmentDefinition
                    {
                        Type = peopleAssignment.Type,
                        Usage = peopleAssignment.Usage,
                        Value = peopleAssignment.Value
                    });
                }
            }

            return result;
        }

        private static int EnrichPriority(HumanTaskDefinitionAggregate humanTaskDef, CreateHumanTaskInstanceCommand request)
        {
            int priority = humanTaskDef.Priority;
            if (request.Priority != null)
            {
                priority = request.Priority.Value;
            }

            return priority;
        }
    }
}