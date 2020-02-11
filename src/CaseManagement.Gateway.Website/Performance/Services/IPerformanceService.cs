using CaseManagement.Gateway.Website.Performance.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CaseManagement.Gateway.Website.Performance.Services
{
    public interface IPerformanceService
    {
        Task<IEnumerable<string>> Get();
        Task<FindPerformanceResponse> Search(IEnumerable<KeyValuePair<string, string>> queries);
    }
}
