using CaseManagement.Gateway.Website.CaseWorkerTask.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CaseManagement.Gateway.Website.CaseWorkerTask.Services
{
    public interface ICaseWorkerTaskService
    {
        Task<FindResponse<CaseWorkerTaskResponse>> Search(IEnumerable<KeyValuePair<string, string>> queries);
    }
}
