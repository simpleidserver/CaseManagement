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
    [Route(CMMNConstants.RouteNames.CaseFormInstances)]
    public class CaseFormInstancesController : Controller
    {
        private readonly IFormInstanceQueryRepository _formInstanceQueryRepository;
        private readonly IRoleQueryRepository _roleQueryRepository;

        public CaseFormInstancesController(IFormInstanceQueryRepository formInstanceQueryRepository, IRoleQueryRepository roleQueryRepository)
        {
            _formInstanceQueryRepository = formInstanceQueryRepository;
            _roleQueryRepository = roleQueryRepository;
        }

        [HttpGet("search")]
        [Authorize("get_forminstances")]
        public async Task<IActionResult> Search()
        {
            var query = HttpContext.Request.Query.ToEnumerable();
            var result = await _formInstanceQueryRepository.Find(ExtractFindFormInstanceParameter(query));
            return new OkObjectResult(ToDto(result));
        }

        private static JObject ToDto(FindResponse<FormInstanceAggregate> resp)
        {
            return new JObject
            {
                { "start_index", resp.StartIndex },
                { "total_length", resp.TotalLength },
                { "count", resp.Count },
                { "content", new JArray(resp.Content.Select(r => {
                    var result = new JObject
                    {
                        { "id", r.Id },
                        { "create_datetime", r.CreateDateTime },
                        { "update_datetime", r.UpdateDateTime },
                        { "performer", r.PerformerRole },
                        { "case_plan_instance_id", r.CasePlanInstanceId },
                        { "case_plan_element_instance_id", r.CaseElementInstanceId },
                        { "form_id", r.FormId },
                        { "status", Enum.GetName(typeof(FormInstanceStatus), r.Status).ToLower() }
                    };
                    var content = new JArray();
                    foreach(var formElt in r.Content)
                    {
                        var record = new JObject
                        {
                            { "form_element_id", formElt.FormElementId },
                            { "value", formElt.Value }
                        };
                        content.Add(record);
                    }

                    result.Add("content", content);
                    return result;
                })) }
            };
        }

        private static FindFormInstanceParameter ExtractFindFormInstanceParameter(IEnumerable<KeyValuePair<string, string>> query)
        {
            var parameter = new FindFormInstanceParameter();
            parameter.ExtractFindParameter(query);
            return parameter;
        }
    }
}