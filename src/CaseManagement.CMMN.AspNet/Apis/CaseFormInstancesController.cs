﻿using CaseManagement.CMMN.AspNet.Extensions;
using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Extensions;
using CaseManagement.CMMN.Persistence;
using CaseManagement.CMMN.Persistence.Parameters;
using CaseManagement.CMMN.Persistence.Responses;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace CaseManagement.CMMN.AspNet.Apis
{
    [RoutePrefix(CMMNConstants.RouteNames.CaseFormInstances)]
    public class CaseFormInstancesController : ApiController
    {
        private readonly IFormInstanceQueryRepository _formInstanceQueryRepository;
        private readonly IRoleQueryRepository _roleQueryRepository;

        public CaseFormInstancesController(IFormInstanceQueryRepository formInstanceQueryRepository, IRoleQueryRepository roleQueryRepository)
        {
            _formInstanceQueryRepository = formInstanceQueryRepository;
            _roleQueryRepository = roleQueryRepository;
        }

        [HttpGet]
        [Route("search")]
        public async Task<IHttpActionResult> Search()
        {
            var query = this.Request.GetQueryNameValuePairs();
            var result = await _formInstanceQueryRepository.Find(ExtractFindFormInstanceParameter(query, null));
            return Ok(ToDto(result));
        }

        [HttpGet]
        [Route("me/search")]
        [Authorize]
        public async Task<IHttpActionResult> SearchMyCaseFormInstances()
        {
            var nameIdentifier = this.GetNameIdentifier();
            var userRoles = await _roleQueryRepository.FindRolesByUser(nameIdentifier);
            var roleIds = userRoles.Select(r => r.Id);
            var query = this.Request.GetQueryNameValuePairs();
            var result = await _formInstanceQueryRepository.Find(ExtractFindFormInstanceParameter(query, roleIds));
            return Ok(ToDto(result));
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
                        { "performer", r.RoleId },
                        { "case_definition_id", r.CaseDefinitionId },
                        { "case_instance_id", r.CaseElementInstanceId },
                        { "case_element_definition_id", r.CaseElementDefinitionId },
                        { "case_element_instance_id", r.CaseElementInstanceId },
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

        private static FindFormInstanceParameter ExtractFindFormInstanceParameter(IEnumerable<KeyValuePair<string, string>> query, IEnumerable<string> roleIds)
        {
            string caseDefinitionId;
            var parameter = new FindFormInstanceParameter
            {
                RoleIds = roleIds
            };
            parameter.ExtractFindParameter(query);
            if (query.TryGet("case_definition_id", out caseDefinitionId))
            {
                parameter.CaseDefinitionId = caseDefinitionId;
            }

            return parameter;
        }
    }
}