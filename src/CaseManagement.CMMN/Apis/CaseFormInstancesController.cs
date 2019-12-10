using CaseManagement.CMMN.Extensions;
using CaseManagement.Workflow.Domains;
using CaseManagement.Workflow.Persistence;
using CaseManagement.Workflow.Persistence.Parameters;
using CaseManagement.Workflow.Persistence.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Apis
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

        [HttpGet(".me/search")]
        [Authorize("IsConnected")]
        public async Task<IActionResult> Search()
        {
            var nameIdentifier = this.GetNameIdentifier();
            var userRoles = await _roleQueryRepository.FindRolesByUser(nameIdentifier);
            var roleIds = userRoles.Select(r => r.Id);
            var query = HttpContext.Request.Query;
            var result = await _formInstanceQueryRepository.Find(ExtractFindFormInstanceParameter(query, roleIds));
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
                        { "create_datetime", r.CreateDateTime },
                        { "update_datetime", r.UpdateDateTime },
                        { "status", Enum.GetName(typeof(FormInstanceStatus), r.Status).ToLowerInvariant() },
                        { "form_id", r.FormId }
                    };
                    foreach(var title in r.Titles)
                    {
                        result.Add($"title#{title.Language}", title.Value);
                    }

                    var content = new JArray();
                    foreach(var formElt in r.Content)
                    {
                        var record = new JObject
                        {
                            { "form_element_id", formElt.FormElementId },
                            { "is_required", formElt.IsRequired },
                            { "value", formElt.Value },
                            { "type", Enum.GetName(typeof(FormElementTypes), formElt.Type).ToLowerInvariant() }
                        };
                        foreach(var name in formElt.Names)
                        {
                            record.Add($"name#{name.Language}", name.Value);
                        }

                        foreach(var description in formElt.Descriptions)
                        {
                            record.Add($"description#{description.Language}", description.Value);
                        }

                        content.Add(record);
                    }

                    result.Add("content", content);
                    return result;
                })) }
            };
        }

        private static FindFormInstanceParameter ExtractFindFormInstanceParameter(IQueryCollection query, IEnumerable<string> roleIds)
        {
            var parameter = new FindFormInstanceParameter
            {
                RoleIds = roleIds
            };
            parameter.ExtractFindParameter(query);
            return parameter;
        }
    }
}