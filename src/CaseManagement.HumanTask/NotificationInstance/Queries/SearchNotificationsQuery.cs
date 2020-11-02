using CaseManagement.Common.Parameters;
using CaseManagement.Common.Results;
using CaseManagement.HumanTask.NotificationInstance.Queries.Results;
using MediatR;
using System.Collections.Generic;

namespace CaseManagement.HumanTask.NotificationInstance.Queries
{
    public class SearchNotificationsQuery : BaseSearchParameter, IRequest<SearchResult<NotificationDetailsResult>>
    {
        public IEnumerable<KeyValuePair<string, string>> Claims { get; set; }
    }
}
