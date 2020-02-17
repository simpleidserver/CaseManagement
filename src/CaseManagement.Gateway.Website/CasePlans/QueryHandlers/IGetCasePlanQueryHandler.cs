using CaseManagement.Gateway.Website.CasePlans.DTOs;
using System.Threading.Tasks;

namespace CaseManagement.Gateway.Website.CasePlans.QueryHandlers
{
    public interface IGetCasePlanQueryHandler
    {
        Task<CasePlanResponse> Handle(string id, string identityToken);
    }
}
