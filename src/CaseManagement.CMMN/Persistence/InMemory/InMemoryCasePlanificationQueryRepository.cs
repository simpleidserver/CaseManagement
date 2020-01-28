using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Extensions;
using CaseManagement.CMMN.Persistence.Parameters;
using CaseManagement.CMMN.Persistence.Responses;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Persistence.InMemory
{
    public class InMemoryCasePlanificationQueryRepository : ICasePlanificationQueryRepository
    {
        private static Dictionary<string, string> MAPPING_PLANIFICATIONNAME_TO_PROPERTYNAME = new Dictionary<string, string>
        {
            { "case_instance_id", "CaseInstanceId" },
            { "case_name", "CaseName" },
            { "case_description", "CaseDescription" },
            { "case_element_id", "CaseElementId" },
            { "case_element_name", "CaseElementName" },
            { "role", "UserRole" },
            { "create_datetime", "CreateDateTime" }
        };
        private readonly ConcurrentBag<CasePlanificationAggregate> _casePlanifications;

        public InMemoryCasePlanificationQueryRepository(ConcurrentBag<CasePlanificationAggregate> casePlanifications)
        {
            _casePlanifications = casePlanifications;
        }

        public Task<CasePlanificationAggregate> FindById(string caseInstanceId, string caseElementId)
        {
            return Task.FromResult(_casePlanifications.FirstOrDefault(c => c.CaseInstanceId == caseInstanceId && c.CaseElementId == caseElementId));
        }

        public Task<FindResponse<CasePlanificationAggregate>> Find(FindCasePlanificationParameter parameter)
        {
            IQueryable<CasePlanificationAggregate> result = _casePlanifications.AsQueryable();
            if (MAPPING_PLANIFICATIONNAME_TO_PROPERTYNAME.ContainsKey(parameter.OrderBy))
            {
                result = result.InvokeOrderBy(MAPPING_PLANIFICATIONNAME_TO_PROPERTYNAME[parameter.OrderBy], parameter.Order);
            }

            if (parameter.Roles != null && parameter.Roles.Any())
            {
                result = result.Where(r => parameter.Roles.Contains(r.UserRole));
            }

            if (!string.IsNullOrWhiteSpace(parameter.CaseInstanceId))
            {
                result = result.Where(r => parameter.CaseInstanceId == r.CaseInstanceId);
            }

            List<CasePlanificationAggregate> content;
            int totalLength = 0;
            if (parameter.GroupBy == "case_instance_id")
            {
                var groupedResult = result.GroupBy(p => p.CaseInstanceId);
                totalLength = groupedResult.Select(v => v.Count()).Sum();
                content = groupedResult.SelectMany(v => v.Skip(parameter.StartIndex).Take(parameter.Count).ToList()).ToList();
            }
            else
            {
                totalLength = result.Count();
                result = result.Skip(parameter.StartIndex).Take(parameter.Count);
                content = result.ToList();
            }

            return Task.FromResult(new FindResponse<CasePlanificationAggregate>
            {
                StartIndex = parameter.StartIndex,
                Count = parameter.Count,
                TotalLength = totalLength,
                Content = content.ToList()
            });
        }
    }
}
