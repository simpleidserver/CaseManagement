using CaseManagement.HumanTask.Authorization;
using CaseManagement.HumanTask.Domains;
using CaseManagement.HumanTask.Exceptions;
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
    public class StartHumanTaskInstanceCommandHandler : IRequestHandler<StartHumanTaskInstanceCommand, bool>
    {
        private readonly IHumanTaskInstanceQueryRepository _humanTaskInstanceQueryRepository;
        private readonly IHumanTaskInstanceCommandRepository _humanTaskInstanceCommandRepository;
        private readonly IAuthorizationHelper _authorizationHelper;
        private readonly IMediator _mediator;
        private readonly ILogger<StartHumanTaskInstanceCommandHandler> _logger;

        public StartHumanTaskInstanceCommandHandler(
            IHumanTaskInstanceQueryRepository humanTaskInstanceQueryRepository,
            IHumanTaskInstanceCommandRepository humanTaskInstanceCommandRepository,
            IAuthorizationHelper authorizationHelper,
            IMediator mediator,
            ILogger<StartHumanTaskInstanceCommandHandler> logger)
        {
            _humanTaskInstanceQueryRepository = humanTaskInstanceQueryRepository;
            _humanTaskInstanceCommandRepository = humanTaskInstanceCommandRepository;
            _authorizationHelper = authorizationHelper;
            _mediator = mediator;
            _logger = logger;
        }

        public async Task<bool> Handle(StartHumanTaskInstanceCommand request, CancellationToken cancellationToken)
        {
            if (request.Claims == null || !request.Claims.Any())
            {
                _logger.LogError("User is not authenticated");
                throw new NotAuthenticatedException(Global.UserNotAuthenticated);
            }

            var humanTaskInstance = await _humanTaskInstanceQueryRepository.Get(request.HumanTaskInstanceId, cancellationToken);
            if (humanTaskInstance == null)
            {
                _logger.LogError($"HumanTask '{request.HumanTaskInstanceId}' doesn't exist");
                throw new UnknownHumanTaskInstanceException(string.Format(Global.UnknownHumanTaskInstance, request.HumanTaskInstanceId));
            }


            var roles = await _authorizationHelper.GetRoles(humanTaskInstance, request.Claims, cancellationToken);
            if (!roles.Contains(UserRoles.POTENTIALOWNER))
            {
                _logger.LogError("User is not a potentiel owner");
                throw new NotAuthorizedException(Global.UserNotAuthorized);
            }

            if (humanTaskInstance.Status != HumanTaskInstanceStatus.READY && humanTaskInstance.Status != HumanTaskInstanceStatus.RESERVED)
            {
                _logger.LogError("Claim operation can be performed only on READY / RESERVED human task instance");
                throw new BadOperationExceptions(string.Format(Global.OperationCanBePerformed, "Claim", "Ready/Reserved"));
            }

            var userPrincipal = request.Claims.GetUserNameIdentifier();
            humanTaskInstance.Start(userPrincipal);
            await _humanTaskInstanceCommandRepository.Update(humanTaskInstance, cancellationToken);
            await _humanTaskInstanceCommandRepository.SaveChanges(cancellationToken);
            if (humanTaskInstance.InstantiationPattern == InstantiationPatterns.AUTOMATIC && humanTaskInstance.SubTasks.Any())
            {
                if (humanTaskInstance.Type == CompositionTypes.PARALLEL)
                {
                    foreach (var subTask in humanTaskInstance.SubTasks)
                    {
                        await _mediator.Send(new InstantiateSubTaskCommand
                        {
                            Claims = request.Claims,
                            HumanTaskInstanceId = humanTaskInstance.AggregateId,
                            SubTaskName = subTask.HumanTaskName
                        }, cancellationToken);
                    }
                }
            }

            return true;
        }
    }
}   
