using CaseManagement.BPMN.DelegateConfiguration.Queries;
using CaseManagement.BPMN.DelegateConfiguration.Results;
using CaseManagement.BPMN.Domains;
using CaseManagement.Common.Results;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.BPMN.Persistence
{
    public interface IDelegateConfigurationRepository
    {
        Task<DelegateConfigurationAggregate> Get(string delegateId, CancellationToken cancellationToken);
        Task<DelegateConfigurationResult> GetResult(string delegateId, CancellationToken cancellationToken);
        Task<SearchResult<DelegateConfigurationResult>> Search(SearchDelegateConfigurationQuery searchDelegateConfigurationQuery,  CancellationToken cancellationToken);
        Task<IEnumerable<string>> GetAll(CancellationToken cancellationToken);
        Task<bool> Update(DelegateConfigurationAggregate configuration, CancellationToken cancellationToken);
        Task<int> SaveChanges(CancellationToken cancellationToken);
    }
}
