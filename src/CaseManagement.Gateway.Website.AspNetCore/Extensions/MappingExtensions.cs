using CaseManagement.Gateway.Website.CaseFile.DTOs;
using CaseManagement.Gateway.Website.CasePlanInstance.DTOs;
using CaseManagement.Gateway.Website.CasePlanInstance.Results;
using CaseManagement.Gateway.Website.CasePlans.DTOs;
using CaseManagement.Gateway.Website.CaseWorkerTask.DTOs;
using CaseManagement.Gateway.Website.Form.DTOs;
using CaseManagement.Gateway.Website.FormInstance.DTOs;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace CaseManagement.Gateway.Website.AspNetCore.Extensions
{
    public static class MappingExtensions
    {
        public static JObject ToDto(this GetCasePlanInstanceResult resp)
        {
            var jObj = resp.CasePlanInstance.ToDto();
            var jArr = jObj["elements"] as JArray;
            foreach (JObject jRecord in jArr)
            {
                var formInstance = resp.FormInstances.Content.FirstOrDefault(c => c.CaseElementInstanceId == jRecord["id"].ToString());
                if (formInstance == null)
                {
                    continue;
                }

                jRecord.Add("form_instance", ToDto(formInstance));
                var form = resp.Forms.Content.FirstOrDefault(c => c.Id == formInstance.FormId);
                if (form == null)
                {
                    continue;
                }

                jRecord.Add("form", ToDto(form));
            }

            return jObj;
        }

        public static JObject ToDto(this FindResponse<CasePlanInstanceResponse> resp)
        {
            return new JObject
            {
                { "start_index", resp.StartIndex },
                { "total_length", resp.TotalLength },
                { "count", resp.Count },
                { "content", new JArray(resp.Content.Select(r => ToDto(r))) }
            };
        }
        
        public static JObject ToDto(this FindResponse<CasePlanResponse> resp)
        {
            return new JObject
            {
                { "start_index", resp.StartIndex },
                { "total_length", resp.TotalLength },
                { "count", resp.Count },
                { "content", new JArray(resp.Content.Select(r => ToDto(r))) }
            };
        }

        public static JObject ToDto(this FindResponse<FormInstanceResponse> resp)
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
                        { "status", r.Status }
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

        public static JObject ToDto(this FindResponse<CaseWorkerTaskResponse> resp)
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
                        { "type", r.TaskType },
                        { "status", r.Status }
                    };
                    return result;
                })) }
            };
        }

        public static JObject ToDto(this FindResponse<CaseFileResponse> resp)
        {
            return new JObject
            {
                { "start_index", resp.StartIndex },
                { "total_length", resp.TotalLength },
                { "count", resp.Count },
                { "content", new JArray(resp.Content.Select(r => ToDto(r))) }
            };
        }

        public static JObject ToDto(this CasePlanInstanceResponse casePlanInstance)
        {
            var result = new JObject
            {
                { "id", casePlanInstance.Id },
                { "name", casePlanInstance.Name },
                { "create_datetime", casePlanInstance.CreateDateTime},
                { "case_plan_id", casePlanInstance.CasePlanId },
                { "context", casePlanInstance.Context },
                { "state", casePlanInstance.State }
            };
            var stateHistories = new JArray();
            var transitionHistories = new JArray();
            var executionHistories = new JArray();
            var elts = new JArray();
            foreach (var stateHistory in casePlanInstance.StateHistories)
            {
                stateHistories.Add(new JObject
                {
                    { "state", stateHistory.State },
                    { "datetime", stateHistory.UpdateDateTime }
                });
            }

            foreach (var transitionHistory in casePlanInstance.TransitionHistories)
            {
                transitionHistories.Add(new JObject
                {
                    { "transition", transitionHistory.Transition },
                    { "datetime", transitionHistory.CreateDateTime }
                });
            }

            foreach (var elt in casePlanInstance.Elements)
            {
                elts.Add(ToDto(elt));
            }

            result.Add("state_histories", stateHistories);
            result.Add("transition_histories", transitionHistories);
            result.Add("execution_histories", executionHistories);
            result.Add("elements", elts);
            return result;
        }
        
        public static JObject ToDto(this CasePlanElementInstanceResponse elt)
        {
            var result = new JObject
            {
                { "id", elt.Id },
                { "name", elt.Name },
                { "type", elt.Type },
                { "version", elt.Version },
                { "create_datetime", elt.CreateDateTime},
                { "case_plan_element_id", elt.CasePlanElementId },
                { "state", elt.State }
            };
            var stateHistories = new JArray();
            var transitionHistories = new JArray();
            foreach (var stateHistory in elt.StateHistories)
            {
                stateHistories.Add(new JObject
                {
                    { "state", stateHistory.State },
                    { "datetime", stateHistory.UpdateDateTime }
                });
            }

            foreach (var transitionHistory in elt.TransitionHistories)
            {
                transitionHistories.Add(new JObject
                {
                    { "transition", transitionHistory.Transition },
                    { "datetime", transitionHistory.CreateDateTime }
                });
            }

            result.Add("state_histories", stateHistories);
            result.Add("transition_histories", transitionHistories);
            return result;
        }

        public static JObject ToDto(this CasePlanResponse def)
        {
            return new JObject
            {
                { "id", def.Id },
                { "name", def.Name },
                { "description", def.Description },
                { "case_file", def.CaseFileId },
                { "create_datetime", def.CreateDateTime },
                { "version", def.Version },
                { "owner", def.Owner }
            };
        }

        public static JObject ToDto(this CaseFileResponse resp)
        {
            return new JObject
            {
                { "id", resp.Id },
                { "name", resp.Name },
                { "description", resp.Description },
                { "payload", resp.Payload },
                { "create_datetime", resp.CreateDateTime },
                { "update_datetime", resp.UpdateDateTime },
                { "version", resp.Version },
                { "file_id", resp.FileId },
                { "owner", resp.Owner },
                { "status", resp.Status }
            };
        }

        public static JObject ToDto(this FormInstanceResponse formInstanceResponse)
        {
            var result = new JObject
            {
                { "case_element_instance_id", formInstanceResponse.CaseElementInstanceId },
                { "case_plan_id", formInstanceResponse.CasePlanId },
                { "case_plan_instance_id", formInstanceResponse.CasePlanInstanceId },
                { "create_datetime", formInstanceResponse.CreateDateTime },
                { "update_datetime", formInstanceResponse.UpdateDateTime },
                { "form_id", formInstanceResponse.FormId }
            };
            return result;
        }

        public static JObject ToDto(this FormResponse formResponse)
        {
            var result = new JObject
            {
                { "id", formResponse.Id },
                { "version", formResponse.Version },
                { "status", formResponse.Status },
                { "elements",  new JArray(formResponse.Elements.Select(e => ToDto(e))) },
                { "create_datetime", formResponse.CreateDateTime },
                { "update_datetime", formResponse.UpdateDateTime }
            };
            foreach (var title in formResponse.Titles)
            {
                result.Add($"title#{title.Language}", title.Value);
            }

            return result;
        }

        public static JObject ToDto(this FormElementResponse formElt)
        {
            var result = new JObject
            {
                { "id", formElt.Id },
                { "is_required", formElt.IsRequired },
                { "type", formElt.Type }
            };
            foreach (var name in formElt.Tiles)
            {
                result.Add($"title#{name.Language}", name.Value);
            }

            foreach (var description in formElt.Descriptions)
            {
                result.Add($"description#{description.Language}", description.Value);
            }

            return result;
        }
    }
}