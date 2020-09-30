using CaseManagement.Common.Parameters;
using CaseManagement.Common.Results;
using CaseManagement.HumanTask.HumanTaskInstance.Queries.Results;
using MediatR;

namespace CaseManagement.HumanTask.HumanTaskInstance.Queries
{
    public class GetHumanTaskInstanceHistoryQuery : BaseSearchParameter, IRequest<SearchResult<TaskInstanceHistoryResult>>
    {
        public string HumanTaskInstanceId { get; set; }
        public bool IncludeData { get; set; }
    }
}
