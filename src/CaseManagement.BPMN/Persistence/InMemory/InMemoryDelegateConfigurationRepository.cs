using CaseManagement.BPMN.DelegateConfiguration.Queries;
using CaseManagement.BPMN.DelegateConfiguration.Results;
using CaseManagement.BPMN.Domains;
using CaseManagement.Common.Results;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.BPMN.Persistence.InMemory
{
    public class InMemoryDelegateConfigurationRepository : IDelegateConfigurationRepository
    {
        private static Dictionary<string, string> MAPPING_DELEGATECONFIGURATION_TO_PROPERTYNAME = new Dictionary<string, string>
        {
            { "create_datetime", "CreateDateTime" },
            { "update_datetime", "UpdateDateTime" }
        };
        private readonly ConcurrentBag<DelegateConfigurationAggregate> _delegateConfigurationAggregates;

        public InMemoryDelegateConfigurationRepository(ConcurrentBag<DelegateConfigurationAggregate> delegateConfigurationAggregates)
        {
            _delegateConfigurationAggregates = delegateConfigurationAggregates;
        }

        public Task<DelegateConfigurationAggregate> Get(string delegateId, CancellationToken cancellationToken)
        {
            return Task.FromResult(_delegateConfigurationAggregates.FirstOrDefault(_ => _.AggregateId == delegateId));
        }

        public Task<DelegateConfigurationResult> GetResult(string delegateId, CancellationToken cancellationToken)
        {
            var result = _delegateConfigurationAggregates.FirstOrDefault(_ => _.AggregateId == delegateId);
            if (result == null)
            {
                return Task.FromResult((DelegateConfigurationResult)null);
            }

            return Task.FromResult(DelegateConfigurationResult.ToDto(result));
        }

        public Task<SearchResult<DelegateConfigurationResult>> Search(SearchDelegateConfigurationQuery parameter, CancellationToken cancellationToken)
        {
            IQueryable<DelegateConfigurationAggregate> result = _delegateConfigurationAggregates.AsQueryable();
            if (MAPPING_DELEGATECONFIGURATION_TO_PROPERTYNAME.ContainsKey(parameter.OrderBy))
            {
                result = result.InvokeOrderBy(MAPPING_DELEGATECONFIGURATION_TO_PROPERTYNAME[parameter.OrderBy], parameter.Order);
            }

            int totalLength = result.Count();
            result = result.Skip(parameter.StartIndex).Take(parameter.Count);
            var content = result.ToList();
            content = content.OrderByDescending(r => r.Version).GroupBy(r => r.AggregateId).Select(r => r.First()).ToList();
            return Task.FromResult(new SearchResult<DelegateConfigurationResult>
            {
                StartIndex = parameter.StartIndex,
                Count = parameter.Count,
                TotalLength = totalLength,
                Content = content.Select(_ => DelegateConfigurationResult.ToDto(_)).ToList()
            });
        }

        public Task<bool> Update(DelegateConfigurationAggregate configuration, CancellationToken cancellationToken)
        {
            var record = _delegateConfigurationAggregates.First(_ => _.AggregateId == configuration.AggregateId);
            _delegateConfigurationAggregates.Remove(record);
            _delegateConfigurationAggregates.Add((DelegateConfigurationAggregate)record.Clone());
            return Task.FromResult(true);
        }

        public Task<IEnumerable<string>> GetAll(CancellationToken cancellationToken)
        {
            IEnumerable<string> result =_delegateConfigurationAggregates.Select(_ => _.AggregateId);
            return Task.FromResult(result);
        }

        public Task<int> SaveChanges(CancellationToken cancellationToken)
        {
            return Task.FromResult(1);
        }
    }
}
