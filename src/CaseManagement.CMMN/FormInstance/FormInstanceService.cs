using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Extensions;
using CaseManagement.CMMN.FormInstance.CommandHandlers;
using CaseManagement.CMMN.FormInstance.Commands;
using CaseManagement.CMMN.Persistence;
using CaseManagement.CMMN.Persistence.Parameters;
using CaseManagement.CMMN.Persistence.Responses;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.FormInstance
{
    public class FormInstanceService : IFormInstanceService
    {
        private readonly IFormInstanceQueryRepository _formInstanceQueryRepository;
        private readonly IConfirmFormInstanceCommandHandler _confirmFormInstanceCommandHandler;

        public FormInstanceService(IFormInstanceQueryRepository formInstanceQueryRepository, IConfirmFormInstanceCommandHandler confirmFormInstanceCommandHandler)
        {
            _formInstanceQueryRepository = formInstanceQueryRepository;
            _confirmFormInstanceCommandHandler = confirmFormInstanceCommandHandler;
        }

        public Task Confirm(ConfirmFormInstanceCommand cmd)
        {
            return _confirmFormInstanceCommandHandler.Handle(cmd);
        }

        public async Task<JObject> Search(IEnumerable<KeyValuePair<string, string>> query)
        {
            var result = await _formInstanceQueryRepository.Find(ExtractFindFormInstanceParameter(query));
            return ToDto(result);
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
                        { "case_plan_id", r.CasePlanId },
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
            string casePlanId;
            string casePlanInstanceId;
            if (query.TryGet("case_plan_id", out casePlanId))
            {
                parameter.CasePlanId = casePlanId;
            }

            if (query.TryGet("case_plan_instance_id", out casePlanInstanceId))
            {
                parameter.CasePlanInstanceId = casePlanInstanceId;
            }

            return parameter;
        }
    }
}
