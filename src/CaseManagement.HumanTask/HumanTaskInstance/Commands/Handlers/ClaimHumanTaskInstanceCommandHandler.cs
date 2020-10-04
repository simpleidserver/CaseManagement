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
    public class ClaimHumanTaskInstanceCommandHandler : IRequestHandler<ClaimHumanTaskInstanceCommand, bool>
    {
        private readonly IHumanTaskInstanceQueryRepository _humanTaskInstanceQueryRepository;
        private readonly IHumanTaskInstanceCommandRepository _humanTaskInstanceCommandRepository;
        private readonly IAuthorizationHelper _authorizationHelper;
        private readonly ILogger<ClaimHumanTaskInstanceCommandHandler> _logger;

        public ClaimHumanTaskInstanceCommandHandler(
            IHumanTaskInstanceQueryRepository humanTaskInstanceQueryRepository,
            IHumanTaskInstanceCommandRepository humanTaskInstanceCommandRepository,
            IAuthorizationHelper authorizationHelper,
            ILogger<ClaimHumanTaskInstanceCommandHandler> logger)
        {
            _humanTaskInstanceQueryRepository = humanTaskInstanceQueryRepository;
            _humanTaskInstanceCommandRepository = humanTaskInstanceCommandRepository;
            _authorizationHelper = authorizationHelper;
            _logger = logger;
        }

        public async Task<bool> Handle(ClaimHumanTaskInstanceCommand request, CancellationToken cancellationToken)
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

            if (humanTaskInstance.Status != HumanTaskInstanceStatus.READY)
            {
                _logger.LogError("Claim operation can be performed only on READY human task instance");
                throw new BadOperationExceptions(string.Format(Global.OperationCanBePerformed, "Claim", "Ready"));
            }

            var userPrincipal = request.Claims.GetUserNameIdentifier();
            humanTaskInstance.Claim(userPrincipal);
            await _humanTaskInstanceCommandRepository.Update(humanTaskInstance, cancellationToken);
            await _humanTaskInstanceCommandRepository.SaveChanges(cancellationToken);
            return true;
        }
    }
}
