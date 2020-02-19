using CaseManagement.Gateway.Website.CasePlanInstance.DTOs;
using System.Threading.Tasks;

namespace CaseManagement.Gateway.Website.CasePlans.CommandHandlers
{
    public interface ILaunchCasePlanInstanceCommandHandler
    {
        Task<CasePlanInstanceResponse> Handle(string casePlanId, string identityToken);
    }
}
