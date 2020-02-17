using CaseManagement.Gateway.Website.CasePlanInstance.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CaseManagement.Gateway.Website.CasePlanInstance.Services
{
    public interface ICasePlanInstanceService
    {
        Task<FindResponse<CasePlanInstanceResponse>> Search(IEnumerable<KeyValuePair<string, string>> queries);
        Task<CasePlanInstanceResponse> GetMe(string id, string identityToken);
        Task<CasePlanInstanceResponse> AddMe(AddCasePlanInstanceParameter addCasePlanInstanceParameter, string identityToken);
        Task Launch(string casePlanInstanceId, string identityToken);
        Task<CasePlanInstanceResponse> Suspend(string id, string identityToken);
    }
}
