using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Persistence.Parameters;
using CaseManagement.Common.Responses;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Persistence
{
    public interface ICasePlanInstanceQueryRepository : IDisposable
    {
        Task<CasePlanInstanceAggregate> Get(string id, CancellationToken token);
        Task<SearchResult<CasePlanInstanceAggregate>> Find(FindCasePlanInstancesParameter parameter, CancellationToken token);
    }
}
