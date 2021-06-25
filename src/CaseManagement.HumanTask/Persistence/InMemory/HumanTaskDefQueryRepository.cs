using CaseManagement.Common.Responses;
using CaseManagement.HumanTask.Domains;
using CaseManagement.HumanTask.Persistence.Parameters;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.HumanTask.Persistence.InMemory
{
    public class HumanTaskDefQueryRepository : IHumanTaskDefQueryRepository
    {
        private readonly ConcurrentBag<HumanTaskDefinitionAggregate> _humanTaskDefs;
        private static Dictionary<string, string> MAPPING_HUMANTASKDEF_TO_PROPERTYNAME = new Dictionary<string, string>
        {
            { "create_datetime", "CreateDateTime" },
            { "update_datetime", "UpdateDateTime" },
        };

        public HumanTaskDefQueryRepository(ConcurrentBag<HumanTaskDefinitionAggregate> humanTaskDefs)
        {
            _humanTaskDefs = humanTaskDefs;
        }

        public Task<HumanTaskDefinitionAggregate> Get(string id, CancellationToken token)
        {
            return Task.FromResult((HumanTaskDefinitionAggregate)_humanTaskDefs.FirstOrDefault(_ => _.AggregateId == id)?.Clone());
        }

        public Task<HumanTaskDefinitionAggregate> GetLatest(string name, CancellationToken token)
        {
            return Task.FromResult((HumanTaskDefinitionAggregate)_humanTaskDefs.OrderByDescending(_ => _.Version).FirstOrDefault(_ => _.Name == name)?.Clone());
        }

        public Task<SearchResult<HumanTaskDefinitionAggregate>> Search(SearchHumanTaskDefParameter parameter, CancellationToken token)
        {
            IQueryable<HumanTaskDefinitionAggregate> result = _humanTaskDefs.AsQueryable();
            if (!string.IsNullOrWhiteSpace(parameter.Name))
            {
                result = result.Where(_ => _.Name == parameter.Name);
            }

            if (MAPPING_HUMANTASKDEF_TO_PROPERTYNAME.ContainsKey(parameter.OrderBy))
            {
                result = result.InvokeOrderBy(MAPPING_HUMANTASKDEF_TO_PROPERTYNAME[parameter.OrderBy], parameter.Order);
            }

            int totalLength = result.Count();
            result = result.Skip(parameter.StartIndex).Take(parameter.Count);
            return Task.FromResult(new SearchResult<HumanTaskDefinitionAggregate>
            {
                StartIndex = parameter.StartIndex,
                Count = parameter.Count,
                TotalLength = totalLength,
                Content = result.ToList()
            });
        }

        public Task<ICollection<HumanTaskDefinitionAggregate>> GetAll(CancellationToken token)
        {
            return Task.FromResult((ICollection<HumanTaskDefinitionAggregate>)_humanTaskDefs.ToList());
        }
    }
}
