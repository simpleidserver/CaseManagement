using CaseManagement.Gateway.Website.CaseFile.Commands;
using CaseManagement.Gateway.Website.CaseFile.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CaseManagement.Gateway.Website.CaseFile.Services
{
    public interface ICaseFileService
    {
        Task<string> Add(AddCaseFileCommand command);
        Task<FindResponse<CaseFileResponse>> Search(IEnumerable<KeyValuePair<string, string>> queries);
        Task<CaseFileResponse> Get(string caseFileId);
        Task Update(UpdateCaseFileCommand command);
        Task<string> Publish(PublishCaseFileCommand command);
    }
}
