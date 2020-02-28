using CaseManagement.Gateway.Website.CasePlanInstance.Queries;
using CaseManagement.Gateway.Website.CasePlanInstance.Results;
using System.Threading.Tasks;

namespace CaseManagement.Gateway.Website.CasePlanInstance.QueryHandlers
{
    public interface IGetAssignedCasePlanInstanceQueryHandler
    {
        Task<GetCasePlanInstanceResult> Handle(GetAssignedCasePlanInstanceQuery query);
    }
}
