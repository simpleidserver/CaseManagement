using MediatR;
using CaseManagement.CMMN.CasePlanInstance.Results;
using CaseManagement.CMMN.Common;
using CaseManagement.CMMN.Common.Parameters;

namespace CaseManagement.CMMN.CasePlanInstance.Queries
{
    public class SearchCasePlanInstanceQuery : BaseSearchParameter, IRequest<SearchResult<CasePlanInstanceResult>>
    {
        public string CasePlanId { get; set; }
    }
}
