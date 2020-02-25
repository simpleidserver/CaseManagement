using CaseManagement.CMMN.CasePlan.Exceptions;
using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Extensions;
using CaseManagement.CMMN.Persistence;
using CaseManagement.CMMN.Persistence.Parameters;
using CaseManagement.CMMN.Persistence.Responses;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CasePlan
{
    public class CasePlanService : ICasePlanService
    {
        private readonly ICasePlanQueryRepository _queryRepository;

        public CasePlanService(ICasePlanQueryRepository queryRepository)
        {
            _queryRepository = queryRepository;
        }

        public async Task<JObject> Count()
        {
            var result = await _queryRepository.Count();
            return new JObject
            {
                { "count", result }
            };
        }

        public async Task<JObject> GetMe(string id, string nameIdentifier)
        {
            var result = await _queryRepository.FindById(id);
            if (result == null)
            {
                throw new UnknownCasePlanException(id);
            }

            if (result.CaseOwner != nameIdentifier)
            {
                throw new UnauthorizedCasePlanException(nameIdentifier, id);
            }

            return ToDto(result);
        }

        public async Task<JObject> Get(string id)
        {
            var result = await _queryRepository.FindById(id);
            if (result == null)
            {
                throw new UnknownCasePlanException(id);
            }

            return ToDto(result);
        }

        public async Task<JObject> Search(IEnumerable<KeyValuePair<string, string>> query)
        {
            var parameter = ExtractFindParameter(query);
            var result = await _queryRepository.Find(parameter);
            return ToDto(result);
        }

        private static JObject ToDto(FindResponse<CasePlanAggregate> resp)
        {
            return new JObject
            {
                { "start_index", resp.StartIndex },
                { "total_length", resp.TotalLength },
                { "count", resp.Count },
                { "content", new JArray(resp.Content.Select(r => ToDto(r))) }
            };
        }

        private static JObject ToDto(CasePlanAggregate def)
        {
            return new JObject
            {
                { "id", def.Id },
                { "name", def.Name },
                { "description", def.Description },
                { "case_file", def.CaseFileId },
                { "create_datetime", def.CreateDateTime },
                { "version", def.Version },
                { "owner", def.CaseOwner }
            };
        }

        private static FindCasePlansParameter ExtractFindParameter(IEnumerable<KeyValuePair<string, string>> query)
        {
            string caseFile;
            string text;
            string caseOwner;
            string casePlanId;
            bool takeLatest = false;
            var parameter = new FindCasePlansParameter();
            parameter.ExtractFindParameter(query);
            if (query.TryGet("case_file", out caseFile))
            {
                parameter.CaseFileId = caseFile;
            }

            if (query.TryGet("text", out text))
            {
                parameter.Text = text;
            }

            if (query.TryGet("owner", out caseOwner))
            {
                parameter.CaseOwner = caseOwner;
            }

            if (query.TryGet("case_plan_id", out casePlanId))
            {
                parameter.CasePlanId = casePlanId;
            }

            if (query.TryGet("take_latest", out takeLatest))
            {
                parameter.TakeLatest = takeLatest;
            }

            return parameter;
        }
    }
}
