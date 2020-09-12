using MediatR;
using CaseManagement.CMMN.CasePlan.Results;

namespace CaseManagement.CMMN.CasePlan.Queries
{
    public class GetCasePlanQuery : IRequest<CasePlanResult>
    {
        public string Id { get; set; }
    }
}
