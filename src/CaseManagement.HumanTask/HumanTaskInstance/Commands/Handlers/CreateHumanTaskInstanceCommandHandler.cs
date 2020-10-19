using CaseManagement.HumanTask.Authorization;
using CaseManagement.HumanTask.Domains;
using CaseManagement.HumanTask.Exceptions;
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
    public class CreateHumanTaskInstanceCommandHandler : IRequestHandler<CreateHumanTaskInstanceCommand, string>
    {
        private readonly IHumanTaskDefQueryRepository _humanTaskDefQueryRepository;
        private readonly IHumanTaskInstanceCommandRepository _humanTaskInstanceCommandRepository;
        private readonly IAuthorizationHelper _authorizationHelper;
        private readonly IParameterParser _parameterParser;
        private readonly IDeadlineParser _deadlineParser;
        private readonly ILogger<CreateHumanTaskInstanceCommandHandler> _logger;

        public CreateHumanTaskInstanceCommandHandler(
            IHumanTaskDefQueryRepository humanTaskDefQueryRepository,
            IHumanTaskInstanceCommandRepository humanTaskInstanceCommandRepository,
            IAuthorizationHelper authorizationHelper,
            IParameterParser parameterParser,
            IDeadlineParser deadLineParser,
            ILogger<CreateHumanTaskInstanceCommandHandler> logger)
        {
            _humanTaskDefQueryRepository = humanTaskDefQueryRepository;
            _humanTaskInstanceCommandRepository = humanTaskInstanceCommandRepository;
            _authorizationHelper = authorizationHelper;
            _parameterParser = parameterParser;
            _deadlineParser = deadLineParser;
            _logger = logger;
        }

        public async Task<string> Handle(CreateHumanTaskInstanceCommand request, CancellationToken cancellationToken)
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
            var parameters = _parameterParser.ParseOperationParameters(humanTaskDef.Operation.InputParameters, operationParameters);
            var assignment = EnrichAssignment(humanTaskDef, request);
            var priority = EnrichPriority(humanTaskDef, request);
            var assignmentInstance = _parameterParser.ParseHumanTaskInstancePeopleAssignment(assignment, parameters);
            if (!request.IsCreatedByTaskParent)
            {
                var roles = await _authorizationHelper.GetRoles(assignmentInstance, request.Claims, cancellationToken);
                if (!roles.Any(r => r == UserRoles.TASKINITIATOR))
                {
                    _logger.LogError("User is not a task initiator");
                    throw new NotAuthorizedException(Global.UserNotAuthorized);
                }
            }

            _logger.LogInformation($"Create human task '{request.HumanTaskName}'");
            var userPrincipal = request.Claims.GetUserNameIdentifier();
            var id = Guid.NewGuid().ToString();
            var deadLines = new List<HumanTaskInstanceDeadLine>();
            if (humanTaskDef.DeadLines.StartDeadLines != null && humanTaskDef.DeadLines.StartDeadLines.Any())
            {
                deadLines.AddRange(_deadlineParser.Evaluate(humanTaskDef.DeadLines.StartDeadLines, HumanTaskInstanceDeadLineTypes.START, parameters));
            }

            if (humanTaskDef.DeadLines.CompletionDeadLines != null && humanTaskDef.DeadLines.CompletionDeadLines.Any())
            {
                deadLines.AddRange(_deadlineParser.Evaluate(humanTaskDef.DeadLines.CompletionDeadLines, HumanTaskInstanceDeadLineTypes.COMPLETION, parameters));
            }

            var presentationElementInstance = _parameterParser.ParsePresentationElement(humanTaskDef.PresentationElement, operationParameters);
            HumanTaskInstanceComposition composition = null;
            if (humanTaskDef.Composition != null)
            {
                composition = new HumanTaskInstanceComposition
                {
                    InstantiationPattern = humanTaskDef.Composition.InstantiationPattern,
                    Type = humanTaskDef.Composition.Type,
                    SubTasks = humanTaskDef.Composition.SubTasks.Select(_ => new HumanTaskInstanceSubTask
                    {
                        HumanTaskName = _.TaskName,
                        ToParts = _.ToParts
                    }).ToList()
                };
            }

            var humanTaskInstance = HumanTaskInstanceAggregate.New(
                id, 
                userPrincipal, 
                request.HumanTaskName, 
                parameters,
                assignmentInstance, 
                priority, 
                request.ActivationDeferralTime, 
                request.ExpirationTime, 
                deadLines, 
                presentationElementInstance,
                composition,
                humanTaskDef.Operation,
                humanTaskDef.CompletionBehavior);
            await _humanTaskInstanceCommandRepository.Add(humanTaskInstance, cancellationToken);
            await _humanTaskInstanceCommandRepository.SaveChanges(cancellationToken);
            return humanTaskInstance.AggregateId;
        }

        private static HumanTaskDefinitionAssignment EnrichAssignment(HumanTaskDefinitionAggregate humanTaskDef, CreateHumanTaskInstanceCommand request)
        {
            HumanTaskDefinitionAssignment taskPeopleAssignment = humanTaskDef.PeopleAssignment;
            if (request.PeopleAssignment != null)
            {
                var domain = request.PeopleAssignment.ToDomain();
                if (domain.BusinessAdministrator != null)
                {
                    taskPeopleAssignment.BusinessAdministrator = domain.BusinessAdministrator;
                }

                if (domain.ExcludedOwner != null)
                {
                    taskPeopleAssignment.ExcludedOwner = domain.ExcludedOwner;
                }

                if (domain.PotentialOwner != null)
                {
                    taskPeopleAssignment.PotentialOwner = domain.PotentialOwner;
                }

                if (domain.Recipient != null)
                {
                    taskPeopleAssignment.Recipient = domain.Recipient;
                }

                if (domain.TaskStakeHolder != null)
                {
                    taskPeopleAssignment.TaskStakeHolder = domain.TaskStakeHolder;
                }
            }

            return taskPeopleAssignment;
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
