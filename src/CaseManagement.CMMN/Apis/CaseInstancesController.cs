using CaseManagement.CMMN.CaseInstance.CommandHandlers;
using CaseManagement.CMMN.CaseInstance.Commands;
using CaseManagement.CMMN.CaseInstance.Exceptions;
using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Extensions;
using CaseManagement.Workflow.Domains;
using CaseManagement.Workflow.Domains.Process.Exceptions;
using CaseManagement.Workflow.Persistence;
using CaseManagement.Workflow.Persistence.Parameters;
using CaseManagement.Workflow.Persistence.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Apis
{
    [Route(CMMNConstants.RouteNames.CaseInstances)]
    public class CaseInstancesController : Controller
    {
        private readonly ICreateCaseInstanceCommandHandler _createCaseInstanceCommandHandler;
        private readonly ILaunchCaseInstanceCommandHandler _launchCaseInstanceCommandHandler;
        private readonly IConfirmFormCommandHandler _confirmFormCommandHandler;
        private readonly IProcessFlowInstanceQueryRepository _processFlowInstanceQueryRepository;

        public CaseInstancesController(ICreateCaseInstanceCommandHandler createCaseInstanceCommandHandler, ILaunchCaseInstanceCommandHandler launchCaseInstanceCommandHandler, IConfirmFormCommandHandler confirmFormCommandHandler, IProcessFlowInstanceQueryRepository processFlowInstanceQueryRepository)
        {
            _createCaseInstanceCommandHandler = createCaseInstanceCommandHandler;
            _launchCaseInstanceCommandHandler = launchCaseInstanceCommandHandler;
            _confirmFormCommandHandler = confirmFormCommandHandler;
            _processFlowInstanceQueryRepository = processFlowInstanceQueryRepository;
        }

        [HttpGet(".search")]
        public async Task<IActionResult> Search()
        {
            var query = HttpContext.Request.Query;
            var result = await _processFlowInstanceQueryRepository.Find(ExtractFindWorkflowInstanceParameter(query));
            return new OkObjectResult(ToDto(result));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCaseInstanceCommand createCaseInstance)
        {
            var result = await _createCaseInstanceCommandHandler.Handle(createCaseInstance);
            return new ContentResult
            {
                StatusCode = (int)HttpStatusCode.Created,
                Content = ToDto(result).ToString()
            };
        }

        [HttpGet("{id}/launch")]
        public async Task<IActionResult> Launch(string id)
        {
            await _launchCaseInstanceCommandHandler.Handle(new LaunchCaseInstanceCommand { CaseInstanceId = id });
            return new OkResult();
        }

        [HttpPost("{id}/confirm/{elt}")]
        public async Task<IActionResult> ConfirmForm(string id, string elt, [FromBody] JObject jObj)
        {
            try
            {
                await _confirmFormCommandHandler.Handle(new ConfirmFormCommand { CaseInstanceId = id, CaseElementInstanceId = elt, Content = jObj });
                return new OkResult();
            }
            catch(UnknownCaseInstanceException)
            {
                return ToError(new Dictionary<string, string>
                {
                    { "bad_request", "case instance doesn't exist" }
                }, HttpStatusCode.NotFound, Request);
            }
            catch (UnknownCaseInstanceElementException)
            {
                return ToError(new Dictionary<string, string>
                {
                    { "bad_request", "case instance element doesn't exist" }
                }, HttpStatusCode.NotFound, Request);
            }
            catch (ProcessFlowInstanceDomainException ex)
            {
                return ToError(ex.Errors, HttpStatusCode.BadRequest, Request);
            }
            catch(Exception ex)
            {
                return ToError(new Dictionary<string, string>
                {
                    { "invalid_request", ex.Message }
                }, HttpStatusCode.BadRequest, Request);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var flowInstance =  await _processFlowInstanceQueryRepository.FindFlowInstanceById(id);
            if (flowInstance == null)
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(ToDto(flowInstance));
        }

        [HttpGet("{id}/steps/.search")]
        public async Task<IActionResult> SearchExecutionSteps(string id)
        {
            var query = HttpContext.Request.Query;
            var parameter = ExtractFindExecutionStepsParameter(query);
            parameter.ProcessFlowInstanceId = id;
            var executionSteps = await _processFlowInstanceQueryRepository.FindExecutionSteps(parameter);
            return new OkObjectResult(ToDto(executionSteps));
        }

        private static FindWorkflowInstanceParameter ExtractFindWorkflowInstanceParameter(IQueryCollection query)
        {
            string templateId;
            var parameter = new FindWorkflowInstanceParameter();
            if (query.TryGet("template_id", out templateId))
            {
                parameter.ProcessFlowTemplateId = templateId;
            }
            ExtractFindParameter(query, parameter);
            return parameter;
        }

        private static FindExecutionStepsParameter ExtractFindExecutionStepsParameter(IQueryCollection query)
        {
            var parameter = new FindExecutionStepsParameter();
            ExtractFindParameter(query, parameter);
            return parameter;
        }

        private static void ExtractFindParameter(IQueryCollection query, BaseFindParameter parameter)
        {
            int startIndex, count;
            string orderBy;
            FindOrders findOrder;
            if (query.TryGet("start_index", out startIndex))
            {
                parameter.StartIndex = startIndex;
            }

            if (query.TryGet("count", out count))
            {
                parameter.Count = count;
            }

            if (query.TryGet("order_by", out orderBy))
            {
                parameter.OrderBy = orderBy;
            }

            if (query.TryGet("order", out findOrder))
            {
                parameter.Order = findOrder;
            }
        }

        private static JObject ToDto(FindResponse<ProcessFlowInstanceExecutionStep> resp)
        {
            return new JObject
            {
                { "start_index", resp.StartIndex },
                { "total_length", resp.TotalLength },
                { "count", resp.Count },
                { "content", new JArray(resp.Content.Select(r => new JObject
                {
                    { "start_datetime", r.StartDateTime },
                    { "end_datetime", r.EndDateTime },
                    { "name", r.ElementName },
                    { "id", r.ElementId }
                })) }
            };
        }

        private static JObject ToDto(FindResponse<ProcessFlowInstance> resp)
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
                        { "name", r.ProcessFlowName },
                        { "template_id", r.ProcessFlowTemplateId },
                        { "create_datetime", r.CreateDateTime }
                    };
                    if (r.Status != null)
                    {
                        result.Add("status", Enum.GetName(typeof(ProcessFlowInstanceStatus), r.Status).ToLowerInvariant());
                    }

                    return result;
                })) }
            };
        }

        private static JObject ToDto(ProcessFlowInstance flowInstance)
        {
            var result = new JObject
            {
                { "id", flowInstance.Id },
                { "create_datetime", flowInstance.CreateDateTime },
                { "template_id", flowInstance.ProcessFlowTemplateId },
                { "name", flowInstance.ProcessFlowName },
                { "context", ToDto(flowInstance.ExecutionContext) }
            };
            if (flowInstance.Status != null)
            {
                result.Add("status", Enum.GetName(typeof(ProcessFlowInstanceStatus), flowInstance.Status).ToLowerInvariant());
            }

            var planItems = new JArray();
            foreach(var planItem in flowInstance.Elements.Where(e => e is CMMNPlanItem).Cast<CMMNPlanItem>())
            {
                planItems.Add(ToDto(planItem));
            }

            result.Add("items", planItems);
            return result;
        }

        private static JObject ToDto(ProcessFlowInstanceExecutionContext context)
        {
            var jObj = new JObject();
            foreach (var kvp in context.Variables)
            {
                jObj.Add(kvp.Key, kvp.Value);
            }

            return jObj;
        }

        private static JObject ToDto(CMMNPlanItem planItem)
        {
            var result = new JObject
            {
                { "id", planItem.Id },
                { "name", planItem.Name }
            };
            if (planItem.Status != null)
            {
                result.Add("status", Enum.GetName(typeof(ProcessFlowInstanceElementStatus), planItem.Status).ToLowerInvariant());
            }

            return result;
        }

        private static ActionResult ToError(ICollection<KeyValuePair<string, string>> errors, HttpStatusCode statusCode, HttpRequest request)
        {
            var problemDetails = new ValidationProblemDetails
            {
                Instance = request.Path,
                Status = (int)statusCode,
                Detail = "Please refer to the errors property for additional details."
            };
            foreach (var kvp in errors.GroupBy(e => e.Key))
            {
                problemDetails.Errors.Add(kvp.Key, kvp.Select(s => s.Value).ToArray());
            }

            return new BadRequestObjectResult(problemDetails)
            {
                StatusCode = (int)statusCode
            };
        }
    }
}