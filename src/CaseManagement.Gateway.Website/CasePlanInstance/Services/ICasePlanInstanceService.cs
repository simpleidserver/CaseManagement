using CaseManagement.Gateway.Website.CasePlanInstance.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CaseManagement.Gateway.Website.CasePlanInstance.Services
{
    public interface ICasePlanInstanceService
    {
        Task<FindResponse<CasePlanInstanceResponse>> Search(IEnumerable<KeyValuePair<string, string>> queries);
        Task<FindResponse<CasePlanInstanceResponse>> SearchMe(IEnumerable<KeyValuePair<string, string>> queries, string identityToken);
        Task<CasePlanInstanceResponse> Get(string id);
        Task<CasePlanInstanceResponse> GetMe(string id, string identityToken);
        Task<CasePlanInstanceResponse> AddMe(AddCasePlanInstanceParameter addCasePlanInstanceParameter, string identityToken);
        Task LaunchMe(string casePlanInstanceId, string identityToken);
        Task SuspendMe(string id, string identityToken);
        Task ReactivateMe(string id, string identityToken);
        Task ResumeMe(string id, string identityToken);
        Task TerminateMe(string id, string identityToken);
        Task CloseMe(string id, string identityToken);
    }
}
