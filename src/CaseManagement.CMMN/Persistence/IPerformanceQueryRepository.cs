using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Persistence.Parameters;
using CaseManagement.CMMN.Persistence.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Persistence
{
    public interface IPerformanceQueryRepository
    {
        Task<IEnumerable<string>> GetMachineNames();
        Task<FindResponse<PerformanceAggregate>> FindPerformance(FindPerformanceParameter parameter);
    }
}