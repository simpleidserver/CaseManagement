using CaseManagement.Gateway.Website.CaseFile.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CaseManagement.Gateway.Website.CaseFile.Services
{
    public interface ICaseFileService
    {
        Task<string> AddMe(AddCaseFileParameter command, string identityToken);
        Task<FindResponse<CaseFileResponse>> Search(IEnumerable<KeyValuePair<string, string>> queries);
        Task<CaseFileResponse> GetMe(string id, string identityToken);
        Task UpdateMe(string id, UpdateCaseFileParameter command, string identityToken);
        Task<string> PublishMe(string id, string identityToken);
        Task<int> Count();
    }
}
