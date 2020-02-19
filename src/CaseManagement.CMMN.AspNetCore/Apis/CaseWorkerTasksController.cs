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
            var result = await _activationQueryRepository.Find(ExtractFindParameter(query));
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
                        { "case_plan_id", r.CasePlanId },
                        { "case_plan_instance_id", r.CasePlanInstanceId },
                        { "case_plan_element_instance_id", r.CasePlanElementInstanceId},
                        { "create_datetime", r.CreateDateTime },
                        { "update_datetime", r.UpdateDateTime },
                        { "performer", r.PerformerRole },
                        { "type", Enum.GetName(typeof(CaseWorkerTaskTypes), r.TaskType).ToLowerInvariant() },
                        { "status", Enum.GetName(typeof(CaseWorkerTaskStatus), r.Status).ToLowerInvariant() }
                    };
                    return result;
                })) }
            };
        }

        private static FindCaseWorkerTasksParameter ExtractFindParameter(IEnumerable<KeyValuePair<string, string>> query)
        {
            var parameter = new FindCaseWorkerTasksParameter();
            parameter.ExtractFindParameter(query);
            string casePlanId;
            if (query.TryGet("case_plan_id", out casePlanId))
            {
                parameter.CasePlanId = casePlanId;
            }

            return parameter;
        }
    }
}