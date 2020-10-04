using CaseManagement.Common.Results;
using CaseManagement.HumanTask.Authorization;
using CaseManagement.HumanTask.Exceptions;
using CaseManagement.HumanTask.NotificationInstance.Queries.Results;
using CaseManagement.HumanTask.Persistence;
using CaseManagement.HumanTask.Persistence.Parameters;
using CaseManagement.HumanTask.Resources;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.HumanTask.NotificationInstance.Queries.Handlers
{
    public class GetNotificationsDetailsQueryHandler : IRequestHandler<GetNotificationsDetailsQuery, SearchResult<NotificationDetailsResult>>
    {
        private readonly INotificationInstanceQueryRepository _notificationInstanceQueryRepository;
        private readonly IAuthorizationHelper _authorizationHelper;
        private readonly ILogicalPeopleGroupStore _logicalPeopleGroupStore;
        private readonly ILogger<GetNotificationsDetailsQueryHandler> _logger;

        public GetNotificationsDetailsQueryHandler(
            INotificationInstanceQueryRepository notificationInstanceQueryRepository,
            IAuthorizationHelper authorizationHelper,
            ILogicalPeopleGroupStore logicalPeopleGroupStore,
            ILogger<GetNotificationsDetailsQueryHandler> logger)
        {
            _notificationInstanceQueryRepository = notificationInstanceQueryRepository;
            _authorizationHelper = authorizationHelper;
            _logicalPeopleGroupStore = logicalPeopleGroupStore;
            _logger = logger;
        }

        public async Task<SearchResult<NotificationDetailsResult>> Handle(GetNotificationsDetailsQuery request, CancellationToken cancellationToken)
        {
            if (request.Claims == null || !request.Claims.Any())
            {
                _logger.LogError("User is not authenticated");
                throw new NotAuthenticatedException(Global.UserNotAuthenticated);
            }

            var userClaims = new UserClaims
            {
                UserIdentifier = _authorizationHelper.GetNameIdentifier(request.Claims),
                Roles = _authorizationHelper.GetRoles(request.Claims),
                LogicalGroups = (await _logicalPeopleGroupStore.GetLogicalGroups(request.Claims, cancellationToken)).Select(_ => _.Name).ToList()
            };
            var result = await _notificationInstanceQueryRepository.Find(new FindNotificationInstanceParameter
            {
                Count = request.Count,
                Order = request.Order,
                OrderBy = request.OrderBy,
                StartIndex = request.StartIndex,
                User = userClaims
            }, cancellationToken);
            return new SearchResult<NotificationDetailsResult>
            {
                StartIndex = result.StartIndex,
                TotalLength = result.TotalLength,
                Count = result.Count,
                Content = result.Content.Select(_ => NotificationDetailsResult.ToDto(_))
            };
        }
    }
}
