using CaseManagement.CMMN.CasePlanInstance.Results;
using CaseManagement.CMMN.Persistence.Parameters;
using CaseManagement.Common.Results;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Persistence
{
    public interface ICasePlanInstanceQueryRepository : IDisposable
    {
        Task<CasePlanInstanceResult> Get(string id, CancellationToken token);
        Task<SearchResult<CasePlanInstanceResult>> Find(FindCasePlanInstancesParameter parameter, CancellationToken token);
    }
}
