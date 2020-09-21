using CaseManagement.CMMN.CaseWorkerTask.Results;
using CaseManagement.Common.Parameters;
using CaseManagement.Common.Results;
using MediatR;
using System.Collections.Generic;

namespace CaseManagement.CMMN.CaseWorkerTask.Queries
{
    public class SearchCaseWorkerTaskQuery : BaseSearchParameter, IRequest<SearchResult<CaseWorkerTaskResult>>
    {
        public IEnumerable<KeyValuePair<string, string>> Claims { get; set; }
    }
}
