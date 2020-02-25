using CaseManagement.Gateway.Website.Form.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CaseManagement.Gateway.Website.Form.Services
{
    public interface IFormService
    {
        Task<FormResponse> Get(string id);
        Task<FindResponse<FormResponse>> Search(IEnumerable<KeyValuePair<string, string>> queries);
    }
}
