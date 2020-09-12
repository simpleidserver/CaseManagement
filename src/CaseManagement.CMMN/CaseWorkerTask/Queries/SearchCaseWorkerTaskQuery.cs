using MediatR;
using CaseManagement.CMMN.CaseWorkerTask.Results;
using CaseManagement.CMMN.Common;
using CaseManagement.CMMN.Common.Parameters;
using System.Collections.Generic;

namespace CaseManagement.CMMN.CaseWorkerTask.Queries
{
    public class SearchCaseWorkerTaskQuery : BaseSearchParameter, IRequest<SearchResult<CaseWorkerTaskResult>>
    {
        public IEnumerable<KeyValuePair<string, string>> Claims { get; set; }
    }
}
