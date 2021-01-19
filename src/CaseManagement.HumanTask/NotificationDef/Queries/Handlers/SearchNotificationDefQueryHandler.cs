using CaseManagement.Common.Results;
using CaseManagement.HumanTask.NotificationDef.Results;
using CaseManagement.HumanTask.Persistence;
using CaseManagement.HumanTask.Persistence.Parameters;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.HumanTask.NotificationDef.Queries.Handlers
{
    public class SearchNotificationDefQueryHandler : IRequestHandler<SearchNotificationDefQuery, SearchResult<NotificationDefResult>>
    {
        private readonly INotificationDefQueryRepository _notificationDefQueryRepository;

        public SearchNotificationDefQueryHandler(INotificationDefQueryRepository notificationDefQueryRepository)
        {
            _notificationDefQueryRepository = notificationDefQueryRepository;
        }

        public async Task<SearchResult<NotificationDefResult>> Handle(SearchNotificationDefQuery request, CancellationToken cancellationToken)
        {
            var result = await _notificationDefQueryRepository.Search(new SearchNotificationDefParameter
            {
                Count = request.Count,
                Order = request.Order,
                OrderBy = request.OrderBy,
                StartIndex = request.StartIndex
            }, cancellationToken);
            return new SearchResult<NotificationDefResult>
            {
                Count = result.Count,
                StartIndex = result.StartIndex,
                TotalLength = result.TotalLength,
                Content = result.Content.Select(_ => NotificationDefResult.ToDto(_))
            };
        }
    }
}
