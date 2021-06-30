using CaseManagement.BPMN.DelegateConfiguration.Queries;
using CaseManagement.BPMN.DelegateConfiguration.Results;
using CaseManagement.BPMN.Domains;
using CaseManagement.Common.Results;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.BPMN.Persistence.EF.Persistence
{
    public class DelegateConfigurationRepository : IDelegateConfigurationRepository
    {
        private static Dictionary<string, string> MAPPING_DELEGATECONFIGURATION_TO_PROPERTYNAME = new Dictionary<string, string>
        {
            { "create_datetime", "CreateDateTime" },
            { "update_datetime", "UpdateDateTime" }
        };

        private readonly BPMNDbContext _dbContext;

        public DelegateConfigurationRepository(BPMNDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<DelegateConfigurationAggregate> Get(string delegateId, CancellationToken cancellationToken)
        {
            return _dbContext.DelegateConfigurations.FirstOrDefaultAsync(d => d.AggregateId == delegateId, cancellationToken);
        }

        public async Task<DelegateConfigurationResult> GetResult(string delegateId, CancellationToken cancellationToken)
        {
            var result = await _dbContext.DelegateConfigurations.FirstOrDefaultAsync(d => d.AggregateId == delegateId, cancellationToken);
            if (result == null)
            {
                return null;
            }

            return DelegateConfigurationResult.ToDto(result);
        }

        public async Task<SearchResult<DelegateConfigurationResult>> Search(SearchDelegateConfigurationQuery parameter, CancellationToken cancellationToken)
        {
            IQueryable<DelegateConfigurationAggregate> result = _dbContext.DelegateConfigurations;
            if (MAPPING_DELEGATECONFIGURATION_TO_PROPERTYNAME.ContainsKey(parameter.OrderBy))
            {
                result = result.InvokeOrderBy(MAPPING_DELEGATECONFIGURATION_TO_PROPERTYNAME[parameter.OrderBy], parameter.Order);
            }

            int totalLength = await result.CountAsync(cancellationToken);
            result = result.Skip(parameter.StartIndex).Take(parameter.Count);
            var content = await result.ToListAsync(cancellationToken);
            content = content.OrderByDescending(r => r.Version).GroupBy(r => r.AggregateId).Select(r => r.First()).ToList();
            return new SearchResult<DelegateConfigurationResult>
            {
                StartIndex = parameter.StartIndex,
                Count = parameter.Count,
                TotalLength = totalLength,
                Content = content.Select(_ => DelegateConfigurationResult.ToDto(_)).ToList()
            };
        }

        public Task<bool> Update(DelegateConfigurationAggregate configuration, CancellationToken cancellationToken)
        {
            _dbContext.DelegateConfigurations.Update(configuration);
            return Task.FromResult(true);
        }

        public async Task<IEnumerable<string>> GetAll(CancellationToken cancellationToken)
        {
            IEnumerable<string> result = await _dbContext.DelegateConfigurations.Select(_ => _.AggregateId).ToListAsync(cancellationToken);
            return result;
        }

        public Task<int> SaveChanges(CancellationToken cancellationToken)
        {
            return _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
