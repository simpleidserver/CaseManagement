using CaseManagement.Common.Parameters;
using CaseManagement.Common.Results;
using CaseManagement.HumanTask.Domains;
using CaseManagement.HumanTask.HumanTaskInstance.Queries.Results;
using MediatR;
using System.Collections.Generic;

namespace CaseManagement.HumanTask.HumanTaskInstance.Queries
{
    public class SearchHumanTaskInstanceDetailsQuery : BaseSearchParameter, IRequest<SearchResult<TaskInstanceDetailsResult>>
    {
        public ICollection<HumanTaskInstanceStatus> StatusLst { get; set; }
        public IEnumerable<KeyValuePair<string, string>> Claims { get; set; }
    }
}
