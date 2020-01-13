using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Extensions;
using CaseManagement.CMMN.Persistence;
using CaseManagement.CMMN.Persistence.Parameters;
using CaseManagement.CMMN.Persistence.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Apis
{
    [Route(CMMNConstants.RouteNames.CaseActivations)]
    public class CaseActivationsController : Controller
    {
        private readonly IActivationQueryRepository _activationQueryRepository;

        public CaseActivationsController(IActivationQueryRepository activationQueryRepository)
        {
            _activationQueryRepository = activationQueryRepository;
        }

        [HttpGet(".search")]
        public async Task<IActionResult> Search()
        {
            var query = HttpContext.Request.Query;
            var result = await _activationQueryRepository.Find(ExtractFindParameter(query, null));
            return new OkObjectResult(ToDto(result));
        }

        private static JObject ToDto(FindResponse<CaseActivationAggregate> resp)
        {
            return new JObject
            {
                { "start_index", resp.StartIndex },
                { "total_length", resp.TotalLength },
                { "count", resp.Count },
                { "content", new JArray(resp.Content.Select(r => {
                    var result = new JObject
                    {
                        { "case_definition_id", r.WorkflowId },
                        { "case_instance_id", r.WorkflowInstanceId},
                        { "case_instance_name", r.WorkflowInstanceName },
                        { "case_element_id", r.WorkflowElementId },
                        { "case_element_instance_id", r.WorkflowElementInstanceId },
                        { "case_element_name", r.WorkflowElementName },
                        { "create_datetime", r.CreateDateTime },
                        { "performer", r.Performer }
                    };
                    return result;
                })) }
            };
        }

        private static FindCaseActivationsParameter ExtractFindParameter(IQueryCollection query, IEnumerable<string> roleIds)
        {
            string caseDefinitionId;
            var parameter = new FindCaseActivationsParameter();
            parameter.ExtractFindParameter(query);
            if (query.TryGet("case_definition_id", out caseDefinitionId))
            {
                parameter.CaseDefinitionId = caseDefinitionId;
            }

            return parameter;
        }
    }
}