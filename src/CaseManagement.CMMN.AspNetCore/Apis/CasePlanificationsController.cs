using CaseManagement.CMMN.AspNetCore.Extensions;
using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Extensions;
using CaseManagement.CMMN.Persistence;
using CaseManagement.CMMN.Persistence.Parameters;
using CaseManagement.CMMN.Persistence.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.AspNetCore.Apis
{
    [Route(CMMNConstants.RouteNames.CasePlanifications)]
    public class CasePlanificationsController : Controller
    {
        private readonly ICasePlanificationQueryRepository _casePlanificationQueryRepository;
        private readonly IRoleQueryRepository _roleQueryRepository;

        public CasePlanificationsController(ICasePlanificationQueryRepository casePlanificationQueryRepository, IRoleQueryRepository roleQueryRepository)
        {
            _casePlanificationQueryRepository = casePlanificationQueryRepository;
            _roleQueryRepository = roleQueryRepository;
        }

        [HttpGet("search")]
        [Authorize("IsConnected")]
        public async Task<IActionResult> Search()
        {
            var query = HttpContext.Request.Query.ToEnumerable();
            var nameIdentifier = this.GetNameIdentifier();
            var roles = (await _roleQueryRepository.FindRolesByUser(nameIdentifier)).Select(r => r.Id);
            var result = await _casePlanificationQueryRepository.Find(ExtractFindParameter(query, null));
            return new OkObjectResult(ToDto(result));
        }

        private static FindCasePlanificationParameter ExtractFindParameter(IEnumerable<KeyValuePair<string, string>> query, IEnumerable<string> roleIds)
        {
            string groupBy;
            string caseInstanceId;
            var parameter = new FindCasePlanificationParameter();
            parameter.ExtractFindParameter(query);
            if (query.TryGet("group_by", out groupBy))
            {
                parameter.GroupBy = groupBy;
            }

            if (query.TryGet("case_instance_id", out caseInstanceId))
            {
                parameter.CaseInstanceId = caseInstanceId;
            }

            parameter.Roles = roleIds;
            return parameter;
        }

        private static JObject ToDto(FindResponse<CasePlanificationAggregate> resp)
        {
            return new JObject
            {
                { "start_index", resp.StartIndex },
                { "total_length", resp.TotalLength },
                { "count", resp.Count },
                { "content", new JArray(resp.Content.Select(r => {
                    var result = new JObject
                    {
                        { "case_instance_id", r.CaseInstanceId},
                        { "case_instance_name", r.CaseName },
                        { "case_instance_description", r.CaseDescription },
                        { "case_element_id", r.CaseElementId },
                        { "case_element_name", r.CaseElementName },
                        { "create_datetime", r.CreateDateTime }
                    };
                    return result;
                })) }
            };
        }
    }
}
