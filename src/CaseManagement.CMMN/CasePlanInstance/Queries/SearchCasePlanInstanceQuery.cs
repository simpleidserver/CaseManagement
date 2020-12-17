using CaseManagement.CMMN.CasePlanInstance.Results;
using CaseManagement.Common.Parameters;
using CaseManagement.Common.Results;
using MediatR;

namespace CaseManagement.CMMN.CasePlanInstance.Queries
{
    public class SearchCasePlanInstanceQuery : BaseSearchParameter, IRequest<SearchResult<CasePlanInstanceResult>>
    {
        public string CasePlanId { get; set; }
        public string CaseFileId { get; set; }
    }
}
