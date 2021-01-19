using CaseManagement.Common.Responses;
using CaseManagement.HumanTask.Domains;
using CaseManagement.HumanTask.Persistence.Parameters;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.HumanTask.Persistence.EF.Persistence
{
    public class HumanTaskDefQueryRepository : IHumanTaskDefQueryRepository
    {
        private readonly HumanTaskDBContext _humanTaskDBContext;
        private static Dictionary<string, string> MAPPING_HUMANTASKDEF_TO_PROPERTYNAME = new Dictionary<string, string>
        {
            { "create_datetime", "CreateDateTime" },
            { "update_datetime", "UpdateDateTime" },
        };

        public HumanTaskDefQueryRepository(HumanTaskDBContext humanTaskDBContext)
        {
            _humanTaskDBContext = humanTaskDBContext;
        }

        public Task<HumanTaskDefinitionAggregate> Get(string id, CancellationToken token)
        {
            return _humanTaskDBContext.HumanTaskDefinitions
                .Include(_ => _.OperationParameters)
                .Include(_ => _.PeopleAssignments)
                .Include(_ => _.PresentationElements)
                .Include(_ => _.PresentationParameters)
                .Include(_ => _.RenderingElements).ThenInclude(_ => _.Labels)
                .Include(_ => _.RenderingElements).ThenInclude(_ => _.Values).ThenInclude(_ => _.DisplayNames)
                .Include(_ => _.DeadLines).ThenInclude(_ => _.Escalations).ThenInclude(_ => _.ToParts)
                .FirstOrDefaultAsync(_ => _.AggregateId == id, token);
        }

        public Task<HumanTaskDefinitionAggregate> GetLatest(string name, CancellationToken token)
        {
            return _humanTaskDBContext.HumanTaskDefinitions
                .Include(_ => _.OperationParameters)
                .Include(_ => _.PeopleAssignments)
                .Include(_ => _.PresentationElements)
                .Include(_ => _.PresentationParameters)
                .Include(_ => _.RenderingElements).ThenInclude(_ => _.Labels)
                .Include(_ => _.RenderingElements).ThenInclude(_ => _.Values).ThenInclude(_ => _.DisplayNames)
                .Include(_ => _.DeadLines).ThenInclude(_ => _.Escalations).ThenInclude(_ => _.ToParts)
                .OrderByDescending(_ => _.Version).FirstOrDefaultAsync(_ => _.Name == name, token);
        }

        public async Task<FindResponse<HumanTaskDefinitionAggregate>> Search(SearchHumanTaskDefParameter parameter, CancellationToken token)
        {
            IQueryable<HumanTaskDefinitionAggregate> result = _humanTaskDBContext.HumanTaskDefinitions
                .Include(_ => _.OperationParameters)
                .Include(_ => _.PresentationElements)
                .Include(_ => _.RenderingElements).ThenInclude(_ => _.Labels)
                .Include(_ => _.RenderingElements).ThenInclude(_ => _.Values).ThenInclude(_ => _.DisplayNames)
                .Include(_ => _.DeadLines).ThenInclude(_ => _.Escalations).ThenInclude(_ => _.ToParts)
                .Include(_ => _.PresentationParameters)
                .Include(_ => _.PeopleAssignments);
            if (!string.IsNullOrWhiteSpace(parameter.Name))
            {
                result = result.Where(_ => _.Name == parameter.Name);
            }

            if (MAPPING_HUMANTASKDEF_TO_PROPERTYNAME.ContainsKey(parameter.OrderBy))
            {
                result = result.InvokeOrderBy(MAPPING_HUMANTASKDEF_TO_PROPERTYNAME[parameter.OrderBy], parameter.Order);
            }

            int totalLength = await result.CountAsync(token);
            result = result.Skip(parameter.StartIndex).Take(parameter.Count);
            var content = await result.ToListAsync(token);
            return new FindResponse<HumanTaskDefinitionAggregate>
            {
                StartIndex = parameter.StartIndex,
                Count = parameter.Count,
                TotalLength = totalLength,
                Content = content
            };
        }

        public async Task<ICollection<HumanTaskDefinitionAggregate>> GetAll(CancellationToken token)
        {
            var result = await _humanTaskDBContext.HumanTaskDefinitions.ToListAsync(token);
            return result;
        }
    }
}
