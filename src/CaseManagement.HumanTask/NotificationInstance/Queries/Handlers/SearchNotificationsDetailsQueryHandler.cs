using CaseManagement.Common.Results;
using CaseManagement.HumanTask.Authorization;
using CaseManagement.HumanTask.Exceptions;
using CaseManagement.HumanTask.Localization;
using CaseManagement.HumanTask.NotificationInstance.Queries.Results;
using CaseManagement.HumanTask.Persistence;
using CaseManagement.HumanTask.Persistence.Parameters;
using CaseManagement.HumanTask.Resources;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
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
        private readonly ITranslationHelper _translationHelper;
        private readonly INotificationInstanceQueryRepository _notificationInstanceQueryRepository;

        public SearchNotificationsDetailsQueryHandler(
            ILogger<SearchNotificationsDetailsQueryHandler> logger,
            IAuthorizationHelper authorizationHelper,
            ILogicalPeopleGroupStore logicalPeopleGroupStore,
            ITranslationHelper translationHelper,
            INotificationInstanceQueryRepository notificationInstanceQueryRepository)
        {
            _logger = logger;
            _authorizationHelper = authorizationHelper;
            _logicalPeopleGroupStore = logicalPeopleGroupStore;
            _translationHelper = translationHelper;
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
            var content = new List<NotificationDetailsResult>();
            foreach (var record in result.Content)
            {
                var callbackTxt = new Func<ICollection<Domains.Text>, Translation>((t) =>
                {
                    if (t == null || !t.Any())
                    {
                        return null;
                    }

                    try
                    {
                        return _translationHelper.Translate(t);
                    }
                    catch (BadOperationExceptions) { return null; }
                });
                var name = callbackTxt(record.PresentationElement.Names);
                var subject = callbackTxt(record.PresentationElement.Subjects);
                content.Add(NotificationDetailsResult.ToDto(record, name, subject));
            }

            return new SearchResult<NotificationDetailsResult>
            {
                StartIndex = result.StartIndex,
                TotalLength = result.TotalLength,
                Count = result.Count,
                Content = content
            };
        }
    }
}
