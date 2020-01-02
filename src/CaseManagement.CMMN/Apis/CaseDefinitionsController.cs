using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Extensions;
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
        private readonly ICMMNWorkflowDefinitionQueryRepository _queryRepository;

        public CaseDefinitionsController(ICMMNWorkflowDefinitionQueryRepository queryRepository)
        {
            _queryRepository = queryRepository;
        }

        [HttpGet("cmmndefinitions")]
        public async Task<IActionResult> GetCMMNDefinitions()
        {
            var result = await _queryRepository.GetCMMDefinitions();
            return new OkObjectResult(result);
        }
                
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var result = await _queryRepository.FindById(id);
            if (result == null)
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(ToDto(result));
        }

        [HttpGet(".search")]
        public async Task<IActionResult> Search()
        {
            var query = HttpContext.Request.Query;
            var result = await _queryRepository.Find(ExtractFindParameter(query));
            return new OkObjectResult(ToDto(result));
        }

        private static JObject ToDto(FindResponse<CMMNWorkflowDefinition> resp)
        {
            return new JObject
            {
                { "start_index", resp.StartIndex },
                { "total_length", resp.TotalLength },
                { "count", resp.Count },
                { "content", new JArray(resp.Content.Select(r => new JObject
                {
                    { "id", r.Id },
                    { "name", r.Name },
                    { "create_datetime", r.CreateDateTime }
                })) }
            };
        }

        private static JObject ToDto(CMMNWorkflowDefinition def)
        {
            return new JObject
            {
                { "id", def.Id },
                { "name", def.Name },
                { "create_datetime", def.CreateDateTime }
            };
        }

        private static FindWorkflowDefinitionsParameter ExtractFindParameter(IQueryCollection query)
        {
            int startIndex;
            int count;
            string orderBy;
            string cmmnDefinition;
            FindOrders findOrder;
            var parameter = new FindWorkflowDefinitionsParameter();
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

            if (query.TryGet("cmmn_definition", out cmmnDefinition))
            {
                parameter.CMMNDefinition = cmmnDefinition;
            }

            return parameter;
        }
    }
}
