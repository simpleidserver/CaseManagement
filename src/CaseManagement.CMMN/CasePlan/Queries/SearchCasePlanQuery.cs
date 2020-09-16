using MediatR;
using CaseManagement.CMMN.CasePlan.Results;
using CaseManagement.CMMN.Common;
using CaseManagement.CMMN.Common.Parameters;

namespace CaseManagement.CMMN.CasePlan.Queries
{
    public class SearchCasePlanQuery : BaseSearchParameter, IRequest<SearchResult<CasePlanResult>>
    {
        public string Owner { get; set; }
        public string CaseFileId { get; set; }
        public string CasePlanId { get; set; }
        public bool TakeLatest { get; set; }
    }
}
