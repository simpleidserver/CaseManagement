using CaseManagement.CMMN.AspNetCore.Extensions;
using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Extensions;
using CaseManagement.CMMN.Persistence;
using CaseManagement.CMMN.Persistence.Parameters;
using CaseManagement.CMMN.Persistence.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.AspNetCore.Apis
{
    [Route(CMMNConstants.RouteNames.CasePlans)]
    public class CasePlansController : Controller
    {
        private readonly ICasePlanQueryRepository _queryRepository;

        public CasePlansController(ICasePlanQueryRepository queryRepository)
        {
            _queryRepository = queryRepository;
        }

        [HttpGet("count")]
        [Authorize("get_statistic")]
        public async Task<IActionResult> Count()
        {
            var result = await _queryRepository.Count();
            return new OkObjectResult(new 
            {
                count = result
            });
        }

        [HttpGet("{id}")]
        [Authorize("get_caseplan")]
        public async Task<IActionResult> Get(string id)
        {
            var result = await _queryRepository.FindById(id);
            if (result == null)
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(ToDto(result));
        }

        [HttpGet("search")]
        [Authorize("get_caseplan")]
        public async Task<IActionResult> Search()
        {
            var query = HttpContext.Request.Query.ToEnumerable();
            var result = await _queryRepository.Find(ExtractFindParameter(query));
            return new OkObjectResult(ToDto(result));
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
                { "create_datetime", def.CreateDateTime }
            };
        }

        private static FindCasePlansParameter ExtractFindParameter(IEnumerable<KeyValuePair<string, string>> query)
        {
            string caseFile;
            string text;
            string caseOwner;
            string casePlanId;
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

            return parameter;
        }
    }
}
