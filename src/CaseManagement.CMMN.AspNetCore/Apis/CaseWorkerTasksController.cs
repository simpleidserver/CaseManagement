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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.AspNetCore.Apis
{
    [Route(CMMNConstants.RouteNames.CaseWorkerTasks)]
    public class CaseWorkerTasksController : Controller
    {
        private readonly ICaseWorkerTaskQueryRepository _activationQueryRepository;

        public CaseWorkerTasksController(ICaseWorkerTaskQueryRepository activationQueryRepository)
        {
            _activationQueryRepository = activationQueryRepository;
        }

        [HttpGet("search")]
        [Authorize("get_caseworkertasks")]
        public async Task<IActionResult> Search()
        {
            var query = HttpContext.Request.Query.ToEnumerable();
            var result = await _activationQueryRepository.Find(ExtractFindParameter(query, null));
            return new OkObjectResult(ToDto(result));
        }

        private static JObject ToDto(FindResponse<CaseWorkerTaskAggregate> resp)
        {
            return new JObject
            {
                { "start_index", resp.StartIndex },
                { "total_length", resp.TotalLength },
                { "count", resp.Count },
                { "content", new JArray(resp.Content.Select(r => {
                    var result = new JObject
                    {
                        { "case_plan_instance_id", r.CasePlanInstanceId },
                        { "case_plan_element_instance_id", r.CasePlanElementInstanceId},
                        { "create_datetime", r.CreateDateTime },
                        { "performer", r.PerformerRole },
                        { "status", Enum.GetName(typeof(CaseWorkerTaskStatus), r.Status).ToLowerInvariant() }
                    };
                    return result;
                })) }
            };
        }

        private static FindCaseWorkerTasksParameter ExtractFindParameter(IEnumerable<KeyValuePair<string, string>> query, IEnumerable<string> roleIds)
        {
            var parameter = new FindCaseWorkerTasksParameter();
            parameter.ExtractFindParameter(query);
            return parameter;
        }
    }
}