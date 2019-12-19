using CaseManagement.CMMN.Extensions;
using CaseManagement.CMMN.Parser;
using CaseManagement.CMMN.Persistence;
using CaseManagement.CMMN.Persistence.Parameters;
using CaseManagement.CMMN.Persistence.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Apis
{
    [Route(CMMNConstants.RouteNames.CaseDefinitions)]
    public class CaseDefinitionsController : Controller
    {
        private readonly ICMMNDefinitionsQueryRepository _queryRepository;

        public CaseDefinitionsController(ICMMNDefinitionsQueryRepository queryRepository)
        {
            _queryRepository = queryRepository;
        }

        [HttpGet(".search")]
        public async Task<IActionResult> Get()
        {
            var query = HttpContext.Request.Query;
            // var result = await _queryRepository.Find(ExtractFindParameter(query));
            // return new OkObjectResult(ToDto(result));
            return null;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var result = await _queryRepository.FindDefinitionById(id);
            if (result == null)
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(ToDto(result));
        }

        [HttpGet("{id}/cases")]
        public async Task<IActionResult> GetCases(string id)
        {
            var result = await _queryRepository.FindDefinitionById(id);
            if (result == null)
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(result.@case);
        }

        private static BaseFindParameter ExtractFindParameter(IQueryCollection query)
        {
            int startIndex;
            int count;
            string orderBy;
            FindOrders findOrder;
            var parameter = new BaseFindParameter();
            if (query.TryGet("start_index", out startIndex))
            {
                parameter.StartIndex = startIndex;
            }

            if (query.TryGet("count", out count))
            {
                parameter.Count = count;
            }

            if (query.TryGet("order_by", out orderBy))
            {
                parameter.OrderBy = orderBy;
            }

            if (query.TryGet("order", out findOrder))
            {
                parameter.Order = findOrder;
            }

            return parameter;
        }

        private static JObject ToDto(FindResponse<tDefinitions> resp)
        {
            return new JObject
            {
                { "start_index", resp.StartIndex },
                { "total_length", resp.TotalLength },
                { "count", resp.Count },
                { "content", new JArray(resp.Content.Select(r => new JObject
                {
                    { "id", r.id },
                    { "name", r.name },
                    { "create_datetime", r.creationDate }
                })) }
            };
        }

        private static JObject ToDto(tDefinitions def)
        {
            return new JObject
            {
                { "id", def.id },
                { "name", def.name },
                { "create_datetime", def.creationDate },
                { "cases", ToDto(def.@case) },
                { "xml",  new CMMNParser().Serialize(def) }
            };
        }

        private static JArray ToDto(tCase[] tCases)
        {
            var result = new JArray();
            foreach(var tCase in tCases)
            {
                result.Add(new JObject
                {
                    { "id", tCase.id },
                    { "name", tCase.casePlanModel.name }
                });
            }

            return result;
        }
    }
}
