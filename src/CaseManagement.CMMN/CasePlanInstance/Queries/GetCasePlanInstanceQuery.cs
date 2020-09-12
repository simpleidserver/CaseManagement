using MediatR;
using CaseManagement.CMMN.CasePlanInstance.Results;

namespace CaseManagement.CMMN.CasePlanInstance.Queries
{
    public class GetCasePlanInstanceQuery : IRequest<CasePlanInstanceResult>
    {
        public string CasePlanInstanceId { get; set; }
    }
}
