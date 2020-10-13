using CaseManagement.Common.Exceptions;
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
    public class NominateHumanTaskInstanceCommandHandler : IRequestHandler<NominateHumanTaskInstanceCommand, bool>
    {
        private readonly ILogger<NominateHumanTaskInstanceCommandHandler> _logger;
        private readonly IAuthorizationHelper _authorizationHelper;
        private readonly IHumanTaskInstanceQueryRepository _humanTaskInstanceQueryRepository;
        private readonly IHumanTaskInstanceCommandRepository _humanTaskInstanceCommandRepository;

        public NominateHumanTaskInstanceCommandHandler(
            ILogger<NominateHumanTaskInstanceCommandHandler> logger,
            IAuthorizationHelper authorizationHelper,
            IHumanTaskInstanceQueryRepository humanTaskInstanceQueryRepository,
            IHumanTaskInstanceCommandRepository humanTaskInstanceCommandRepository)
        {
            _logger = logger;
            _authorizationHelper = authorizationHelper;
            _humanTaskInstanceQueryRepository = humanTaskInstanceQueryRepository;
            _humanTaskInstanceCommandRepository = humanTaskInstanceCommandRepository;
        }

        public async Task<bool> Handle(NominateHumanTaskInstanceCommand request, CancellationToken cancellationToken)
        {
            if (request.Claims == null || !request.Claims.Any())
            {
                _logger.LogError("User is not authenticated");
                throw new NotAuthenticatedException(Global.UserNotAuthenticated);
            }

            if (request.GroupNames != null && request.GroupNames.Any() && request.UserIdentifiers != null && request.UserIdentifiers.Any())
            {
                _logger.LogError("GroupNames and UserIdentifiers parameters cannot be specified at the same time");
                throw new BadRequestException(Global.GroupNamesAndUserIdentifiersSpecified);
            }

            if ((request.GroupNames == null || !request.GroupNames.Any()) && (request.UserIdentifiers == null || !request.UserIdentifiers.Any()))
            {
                _logger.LogError("GroupNames or UserIdentifiers parameters must be specified");
                throw new BadRequestException(string.Format(Global.MissingParameters, "GroupNames,UserIdentifiers"));
            }

            var humanTaskInstance = await _humanTaskInstanceQueryRepository.Get(request.HumanTaskInstanceId, cancellationToken);
            if (humanTaskInstance == null)
            {
                _logger.LogError($"HumanTask '{request.HumanTaskInstanceId}' doesn't exist");
                throw new UnknownHumanTaskInstanceException(string.Format(Global.UnknownHumanTaskInstance, request.HumanTaskInstanceId));
            }

            var roles = await _authorizationHelper.GetRoles(humanTaskInstance, request.Claims, cancellationToken);
            if (!roles.Contains(UserRoles.BUSINESSADMINISTRATOR))
            {
                _logger.LogError("User is not a business administrator");
                throw new NotAuthorizedException(Global.UserNotAuthorized);
            }

            if (humanTaskInstance.Status != HumanTaskInstanceStatus.CREATED)
            {
                _logger.LogError("Nomination can be performed only on created human task instance");
                throw new BadOperationExceptions(string.Format(Global.OperationCanBePerformed, "Nominate", "Created"));
            }

            var userPrincipal = request.Claims.GetUserNameIdentifier();
            humanTaskInstance.Nominate(userPrincipal, request.GroupNames, request.UserIdentifiers);
            await _humanTaskInstanceCommandRepository.Update(humanTaskInstance, cancellationToken);
            await _humanTaskInstanceCommandRepository.SaveChanges(cancellationToken);
            return true;
        }
    }
}
