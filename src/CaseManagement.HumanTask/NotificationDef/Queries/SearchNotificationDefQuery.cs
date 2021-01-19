using CaseManagement.Common.Parameters;
using CaseManagement.Common.Results;
using CaseManagement.HumanTask.NotificationDef.Results;
using MediatR;

namespace CaseManagement.HumanTask.NotificationDef.Queries
{
    public class SearchNotificationDefQuery : BaseSearchParameter, IRequest<SearchResult<NotificationDefResult>>
    {
    }
}