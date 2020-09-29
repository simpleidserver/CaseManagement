using CaseManagement.HumanTask.Authorization;
using CaseManagement.HumanTask.Domains;
using CaseManagement.HumanTask.Exceptions;
using CaseManagement.HumanTask.Parser;
using CaseManagement.HumanTask.Persistence;
using CaseManagement.HumanTask.Resources;
using MediatR;
using Microsoft.Extensions.Logging;
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
        private readonly IOperationParametersParser _operationParametersParser;
        private readonly ILogger<CreateHumanTaskInstanceCommandHandler> _logger;

        public CreateHumanTaskInstanceCommandHandler(
            IHumanTaskDefQueryRepository humanTaskDefQueryRepository,
            IHumanTaskInstanceCommandRepository humanTaskInstanceCommandRepository,
            IAuthorizationHelper authorizationHelper,
            IOperationParametersParser operationParametersParser,
            ILogger<CreateHumanTaskInstanceCommandHandler> logger)
        {
            _humanTaskDefQueryRepository = humanTaskDefQueryRepository;
            _humanTaskInstanceCommandRepository = humanTaskInstanceCommandRepository;
            _authorizationHelper = authorizationHelper;
            _operationParametersParser = operationParametersParser;
            _logger = logger;
        }

        public async Task<string> Handle(CreateHumanTaskInstanceCommand request, CancellationToken cancellationToken)
        {
            if (request.Claims == null || !request.Claims.Any())
            {
                _logger.LogError("User is not authenticated");
                throw new NotAuthenticatedException(Global.UserNotAuthenticated);
            }

            var humanTaskDef = await _humanTaskDefQueryRepository.Get(request.HumanTaskName, cancellationToken);
            if (humanTaskDef == null)
            {
                _logger.LogError($"Human task definition '{request.HumanTaskName}' doesn't exist");
                throw new UnknownHumanTaskDefException(string.Format(Global.UnknownHumanTaskDef, request.HumanTaskName));
            }

            var assignment = BuildAssignment(humanTaskDef, request);
            var priority = BuildPriority(humanTaskDef, request);
            var roles = await _authorizationHelper.GetRoles(assignment, request.Claims, cancellationToken);
            if (!roles.Any(r => r == UserRoles.TASKINITIATOR))
            {
                _logger.LogError("User is not a task initiator");
                throw new NotAuthorizedException(Global.UserNotAuthorized);
            }

            var operationParameters = request.OperationParameters == null ? new Dictionary<string, string>() : request.OperationParameters;
            var parameters = _operationParametersParser.Parse(humanTaskDef.Operation.Parameters, operationParameters);
            _logger.LogInformation($"Create human task '{request.HumanTaskName}'");
            var humanTaskInstance = HumanTaskInstanceAggregate.New(request.HumanTaskName, parameters, assignment, priority, request.ActivationDeferralTime, request.ExpirationTime);
            await _humanTaskInstanceCommandRepository.Add(humanTaskInstance, cancellationToken);
            await _humanTaskInstanceCommandRepository.SaveChanges(cancellationToken);
            return humanTaskInstance.AggregateId;
        }

        private static TaskPeopleAssignment BuildAssignment(HumanTaskDefinitionAggregate humanTaskDef, CreateHumanTaskInstanceCommand request)
        {
            TaskPeopleAssignment taskPeopleAssignment = humanTaskDef.PeopleAssignment;
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

        private static int BuildPriority(HumanTaskDefinitionAggregate humanTaskDef, CreateHumanTaskInstanceCommand request)
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
