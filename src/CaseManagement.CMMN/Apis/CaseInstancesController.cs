using CaseManagement.CMMN.CaseInstance.CommandHandlers;
using CaseManagement.CMMN.CaseInstance.Commands;
using CaseManagement.CMMN.CaseInstance.Exceptions;
using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Extensions;
using CaseManagement.CMMN.Persistence;
using CaseManagement.Workflow.Infrastructure;
using Microsoft.AspNetCore.Authorization;
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
        private readonly ISuspendCommandHandler _suspendCommandHandler;
        private readonly IResumeCommandHandler _resumeCommandHandler;
        private readonly ITerminateCommandHandler _terminateCommandHandler;
        private readonly IReactivateCommandHandler _reactivateCommandHandler;
        private readonly ICloseCommandHandler _closeCommandHandler;
        private readonly IConfirmFormCommandHandler _confirmFormCommandHandler;
        private readonly ICMMNWorkflowInstanceQueryRepository _cmmnWorkflowInstanceQueryRepository;

        public CaseInstancesController(ICreateCaseInstanceCommandHandler createCaseInstanceCommandHandler, ILaunchCaseInstanceCommandHandler launchCaseInstanceCommandHandler, ISuspendCommandHandler suspendCommandHandler, IResumeCommandHandler resumeCommandHandler, ITerminateCommandHandler terminateCommandHandler, IReactivateCommandHandler reactivateCommandHandler, ICloseCommandHandler closeCommandHandler, IConfirmFormCommandHandler confirmFormCommandHandler, ICMMNWorkflowInstanceQueryRepository cmmnWorkflowInstanceQueryRepository)
        {
            _createCaseInstanceCommandHandler = createCaseInstanceCommandHandler;
            _launchCaseInstanceCommandHandler = launchCaseInstanceCommandHandler;
            _suspendCommandHandler = suspendCommandHandler;
            _resumeCommandHandler = resumeCommandHandler;
            _terminateCommandHandler = terminateCommandHandler;
            _reactivateCommandHandler = reactivateCommandHandler;
            _closeCommandHandler = closeCommandHandler;
            _confirmFormCommandHandler = confirmFormCommandHandler;
            _cmmnWorkflowInstanceQueryRepository = cmmnWorkflowInstanceQueryRepository;
        }

        /*
        [HttpGet(".search")]
        public async Task<IActionResult> Search()
        {
            var query = HttpContext.Request.Query;
            var result = await _processFlowInstanceQueryRepository.Find(ExtractFindWorkflowInstanceParameter(query));
            return new OkObjectResult(ToDto(result));
        }
        */

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCaseInstanceCommand createCaseInstance)
        {
            try
            {
                var result = await _createCaseInstanceCommandHandler.Handle(createCaseInstance);
                return new ContentResult
                {
                    StatusCode = (int)HttpStatusCode.Created,
                    Content = ToDto(result).ToString()
                };
            }
            catch (UnknownCaseDefinitionException)
            {
                return ToError(new Dictionary<string, string>
                {
                    { "bad_request", "case definition doesn't exist" }
                }, HttpStatusCode.NotFound, Request);
            }
        }

        [HttpGet("{id}/launch")]
        public async Task<IActionResult> Launch(string id)
        {
            try
            {
                await _launchCaseInstanceCommandHandler.Handle(new LaunchCaseInstanceCommand { CaseInstanceId = id });
                return new OkResult();
            }
            catch (UnknownCaseInstanceException)
            {
                return ToError(new Dictionary<string, string>
                {
                    { "bad_request", "case instance doesn't exist" }
                }, HttpStatusCode.NotFound, Request);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var flowInstance = await _cmmnWorkflowInstanceQueryRepository.FindFlowInstanceById(id);
            if (flowInstance == null)
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(ToDto(flowInstance));
        }

        [HttpGet("{id}/suspend")]
        public async Task<IActionResult> Suspend(string id)
        {
            try
            {
                await _suspendCommandHandler.Handle(new SuspendCommand(id, null));
                return new OkResult();
            }
            catch (UnknownCaseInstanceException)
            {
                return ToError(new Dictionary<string, string>
                {
                    { "bad_request", "case instance doesn't exist" }
                }, HttpStatusCode.NotFound, Request);
            }
            catch (AggregateValidationException ex)
            {
                return ToError(ex.Errors, HttpStatusCode.BadRequest, Request);
            }
            catch (Exception ex)
            {
                return ToError(new Dictionary<string, string>
                {
                    { "invalid_request", ex.Message }
                }, HttpStatusCode.BadRequest, Request);
            }
        }

        [HttpGet("{id}/suspend/{elt}")]
        public async Task<IActionResult> Suspend(string id, string elt)
        {
            try
            {
                await _suspendCommandHandler.Handle(new SuspendCommand(id, elt));
                return new OkResult();
            }
            catch (UnknownCaseInstanceException)
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
            catch (AggregateValidationException ex)
            {
                return ToError(ex.Errors, HttpStatusCode.BadRequest, Request);
            }
            catch (Exception ex)
            {
                return ToError(new Dictionary<string, string>
                {
                    { "invalid_request", ex.Message }
                }, HttpStatusCode.BadRequest, Request);
            }
        }

        [HttpGet("{id}/reactivate")]
        public async Task<IActionResult> Reactivate(string id)
        {
            try
            {
                await _reactivateCommandHandler.Handle(new ReactivateCommand(id, null));
                return new OkResult();
            }
            catch (UnknownCaseInstanceException)
            {
                return ToError(new Dictionary<string, string>
                {
                    { "bad_request", "case instance doesn't exist" }
                }, HttpStatusCode.NotFound, Request);
            }
            catch (AggregateValidationException ex)
            {
                return ToError(ex.Errors, HttpStatusCode.BadRequest, Request);
            }
            catch (Exception ex)
            {
                return ToError(new Dictionary<string, string>
                {
                    { "invalid_request", ex.Message }
                }, HttpStatusCode.BadRequest, Request);
            }
        }

        [HttpGet("{id}/reactivate/{elt}")]
        public async Task<IActionResult> Reactivate(string id, string elt)
        {
            try
            {
                // Note : possible to reactivate only stage.
                await _reactivateCommandHandler.Handle(new ReactivateCommand(id, elt));
                return new OkResult();
            }
            catch (UnknownCaseInstanceException)
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
            catch (AggregateValidationException ex)
            {
                return ToError(ex.Errors, HttpStatusCode.BadRequest, Request);
            }
            catch (Exception ex)
            {
                return ToError(new Dictionary<string, string>
                {
                    { "invalid_request", ex.Message }
                }, HttpStatusCode.BadRequest, Request);
            }
        }

        [HttpGet("{id}/resume")]
        public async Task<IActionResult> Resume(string id)
        {
            try
            {
                await _resumeCommandHandler.Handle(new ResumeCommand(id, null));
                return new OkResult();
            }
            catch (UnknownCaseInstanceException)
            {
                return ToError(new Dictionary<string, string>
                {
                    { "bad_request", "case instance doesn't exist" }
                }, HttpStatusCode.NotFound, Request);
            }
            catch (AggregateValidationException ex)
            {
                return ToError(ex.Errors, HttpStatusCode.BadRequest, Request);
            }
            catch (Exception ex)
            {
                return ToError(new Dictionary<string, string>
                {
                    { "invalid_request", ex.Message }
                }, HttpStatusCode.BadRequest, Request);
            }
        }

        [HttpGet("{id}/resume/{elt}")]
        public async Task<IActionResult> Resume(string id, string elt)
        {
            try
            {
                await _resumeCommandHandler.Handle(new ResumeCommand(id, elt));
                return new OkResult();
            }
            catch (UnknownCaseInstanceException)
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
            catch (AggregateValidationException ex)
            {
                return ToError(ex.Errors, HttpStatusCode.BadRequest, Request);
            }
            catch (Exception ex)
            {
                return ToError(new Dictionary<string, string>
                {
                    { "invalid_request", ex.Message }
                }, HttpStatusCode.BadRequest, Request);
            }
        }

        [HttpGet("{id}/terminate")]
        public async Task<IActionResult> Terminate(string id)
        {
            try
            {
                await _terminateCommandHandler.Handle(new TerminateCommand(id, null));
                return new OkResult();
            }
            catch (UnknownCaseInstanceException)
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
            catch (AggregateValidationException ex)
            {
                return ToError(ex.Errors, HttpStatusCode.BadRequest, Request);
            }
            catch (Exception ex)
            {
                return ToError(new Dictionary<string, string>
                {
                    { "invalid_request", ex.Message }
                }, HttpStatusCode.BadRequest, Request);
            }
        }

        [HttpGet("{id}/terminate/{elt}")]
        public async Task<IActionResult> Terminate(string id, string elt)
        {
            try
            {
                await _terminateCommandHandler.Handle(new TerminateCommand(id, elt));
                return new OkResult();
            }
            catch (UnknownCaseInstanceException)
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
            catch (AggregateValidationException ex)
            {
                return ToError(ex.Errors, HttpStatusCode.BadRequest, Request);
            }
            catch (Exception ex)
            {
                return ToError(new Dictionary<string, string>
                {
                    { "invalid_request", ex.Message }
                }, HttpStatusCode.BadRequest, Request);
            }
        }

        [HttpGet("{id}/close")]
        public async Task<IActionResult> Close(string id)
        {
            try
            {
                await _closeCommandHandler.Handle(new CloseCommand(id));
                return new OkResult();
            }
            catch (UnknownCaseInstanceException)
            {
                return ToError(new Dictionary<string, string>
                {
                    { "bad_request", "case instance doesn't exist" }
                }, HttpStatusCode.NotFound, Request);
            }
            catch (AggregateValidationException ex)
            {
                return ToError(ex.Errors, HttpStatusCode.BadRequest, Request);
            }
            catch (Exception ex)
            {
                return ToError(new Dictionary<string, string>
                {
                    { "invalid_request", ex.Message }
                }, HttpStatusCode.BadRequest, Request);
            }
        }

        [HttpPost("{id}/confirm/{elt}")]
        [Authorize("IsConnected")]
        public async Task<IActionResult> ConfirmForm(string id, string elt, [FromBody] JObject jObj)
        {
            try
            {
                await _confirmFormCommandHandler.Handle(new ConfirmFormCommand { CaseInstanceId = id, CaseElementInstanceId = elt, Content = jObj, UserIdentifier = this.GetNameIdentifier() });
                return new OkResult();
            }
            catch (UnknownCaseInstanceException)
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
            catch (AggregateValidationException ex)
            {
                return ToError(ex.Errors, HttpStatusCode.BadRequest, Request);
            }
            catch (UnauthorizedCaseWorkerException)
            {
                return ToError(new Dictionary<string, string>
                {
                    { "unauthorized_request", "you're not authorized to confirm the human task" }
                }, HttpStatusCode.Unauthorized, Request);
            }
            catch (Exception ex)
            {
                return ToError(new Dictionary<string, string>
                {
                    { "invalid_request", ex.Message }
                }, HttpStatusCode.BadRequest, Request);
            }
        }

        /*
        [HttpGet("{id}/stop")]
        public async Task<IActionResult> Stop(string id)
        {
            await _stopCaseInstanceCommandHandler.Handle(new StopCaseInstanceCommand { CaseInstanceId = id });
            return new OkResult();
        }

        [HttpPost("{id}/confirm/{elt}")]
        [Authorize("IsConnected")]
        public async Task<IActionResult> ConfirmForm(string id, string elt, [FromBody] JObject jObj)
        {
            try
            {
                await _confirmFormCommandHandler.Handle(new ConfirmFormCommand { CaseInstanceId = id, CaseElementInstanceId = elt, Content = jObj, UserIdentifier = this.GetNameIdentifier() });
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
            catch(UnauthorizedCaseWorkerException)
            {
                return ToError(new Dictionary<string, string>
                {
                    { "unauthorized_request", "you're not authorized to confirm the human task" }
                }, HttpStatusCode.Unauthorized, Request);
            }
            catch(Exception ex)
            {
                return ToError(new Dictionary<string, string>
                {
                    { "invalid_request", ex.Message }
                }, HttpStatusCode.BadRequest, Request);
            }
        }

        [HttpGet("{id}/activate/{elt}")]
        public async Task<IActionResult> Activate(string id, string elt)
        {
            try
            {
                await _activateCommandHandler.Handle(new ActivateCommand(id, elt));
                return new OkResult();
            }
            catch (UnknownCaseInstanceException)
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
            catch (Exception ex)
            {
                return ToError(new Dictionary<string, string>
                {
                    { "invalid_request", ex.Message }
                }, HttpStatusCode.BadRequest, Request);
            }
        }

        [HttpGet("{id}/terminate/{elt}")]
        public async Task<IActionResult> Terminate(string id, string elt)
        {
            try
            {
                await _terminateCommandHandler.Handle(new TerminateCommand(id, elt));
                return new OkResult();
            }
            catch (UnknownCaseInstanceException)
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
            catch (Exception ex)
            {
                return ToError(new Dictionary<string, string>
                {
                    { "invalid_request", ex.Message }
                }, HttpStatusCode.BadRequest, Request);
            }
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
            parameter.ExtractFindParameter(query);
            return parameter;
        }

        private static FindExecutionStepsParameter ExtractFindExecutionStepsParameter(IQueryCollection query)
        {
            var parameter = new FindExecutionStepsParameter();
            parameter.ExtractFindParameter(query);
            return parameter;
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
        */

        private static JObject ToDto(CMMNWorkflowInstance workflowInstance)
        {
            var result = new JObject
            {
                { "id", workflowInstance.Id },
                { "create_datetime", workflowInstance.CreateDateTime},
                { "definition_id", workflowInstance.WorkflowDefinitionId },
                { "context", ToDto(workflowInstance.ExecutionContext) },
                { "state", workflowInstance.State }
            };
            var stateHistories = new JArray();
            var transitionHistories = new JArray();
            var executionHistories = new JArray();
            var elts = new JArray();
            foreach(var stateHistory in workflowInstance.StateHistories)
            {
                stateHistories.Add(new JObject
                {
                    { "state", stateHistory.State },
                    { "datetime", stateHistory.UpdateDateTime }
                });
            }

            foreach(var transitionHistory in workflowInstance.TransitionHistories)
            {
                transitionHistories.Add(new JObject
                {
                    { "transition", Enum.GetName(typeof(CMMNTransitions), transitionHistory.Transition) },
                    { "datetime", transitionHistory.CreateDateTime }
                });
            }

            foreach (var executionHistory in workflowInstance.ExecutionHistories)
            {
                executionHistories.Add(new JObject
                {
                    { "start_datetime", executionHistory.StartDateTime },
                    { "end_datetime", executionHistory.EndDateTime },
                    { "id", executionHistory.WorkflowElementDefinitionId }
                });
            }

            foreach(var elt in workflowInstance.WorkflowElementInstances)
            {
                elts.Add(ToDto(elt));
            }

            result.Add("state_histories", stateHistories);
            result.Add("transition_histories", transitionHistories);
            result.Add("execution_histories", executionHistories);
            result.Add("elements", elts);
            return result;
        }

        private static JObject ToDto(CMMNWorkflowElementInstance elt)
        {
            var result = new JObject
            {
                { "id", elt.Id },
                { "version", elt.Version },
                { "create_datetime", elt.CreateDateTime},
                { "definition_id", elt.WorkflowElementDefinitionId },
                { "form_instanceid", elt.FormInstanceId },
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
                    { "transition", Enum.GetName(typeof(CMMNTransitions), transitionHistory.Transition) },
                    { "datetime", transitionHistory.CreateDateTime }
                });
            }

            result.Add("state_histories", stateHistories);
            result.Add("transition_histories", transitionHistories);
            return result;
        }

        private static JObject ToDto(CMMNWorkflowInstanceExecutionContext context)
        {
            var jObj = new JObject();
            foreach (var kvp in context.Variables)
            {
                jObj.Add(kvp.Key, kvp.Value);
            }

            return jObj;
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

        /*

        private static JObject ToDto(CMMNCaseFileItem fileItem)
        {
            var result = new JObject();
            if (fileItem.Status != null)
            {
                result.Add("status", Enum.GetName(typeof(ProcessFlowInstanceElementStatus), fileItem.Status).ToLowerInvariant());
            }

            var metadata = new JObject();
            foreach(var metadataValue in fileItem.MetadataLst)
            {
                metadata.Add(metadataValue.Key, metadataValue.Value);
            }

            result.Add("metadata", metadata);
            return result;
        }
        */
    }
}