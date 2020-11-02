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
    public class SearchNotificationsDetailsQueryHandler : IRequestHandler<SearchNotificationsQuery, SearchResult<NotificationDetailsResult>>
    {
        private readonly ILogger<SearchNotificationsDetailsQueryHandler> _logger;
        private readonly IAuthorizationHelper _authorizationHelper;
        private readonly ILogicalPeopleGroupStore _logicalPeopleGroupStore;
        private readonly INotificationInstanceQueryRepository _notificationInstanceQueryRepository;

        public SearchNotificationsDetailsQueryHandler(
            ILogger<SearchNotificationsDetailsQueryHandler> logger,
            IAuthorizationHelper authorizationHelper,
            ILogicalPeopleGroupStore logicalPeopleGroupStore,
            INotificationInstanceQueryRepository notificationInstanceQueryRepository)
        {
            _logger = logger;
            _authorizationHelper = authorizationHelper;
            _logicalPeopleGroupStore = logicalPeopleGroupStore;
            _notificationInstanceQueryRepository = notificationInstanceQueryRepository;
        }

        public async Task<SearchResult<NotificationDetailsResult>> Handle(SearchNotificationsQuery request, CancellationToken cancellationToken)
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
