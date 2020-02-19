using CaseManagement.Gateway.Website.FormInstance.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CaseManagement.Gateway.Website.FormInstance.Services
{
    public interface IFormInstanceService
    {
        Task<FindResponse<FormInstanceResponse>> Search(IEnumerable<KeyValuePair<string, string>> queries);
    }
}
