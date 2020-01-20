using CaseManagement.CMMN.AspNetCore.Extensions;
using CaseManagement.CMMN.CaseInstance.CommandHandlers;
using CaseManagement.CMMN.CaseInstance.Commands;
using CaseManagement.CMMN.CaseInstance.Exceptions;
using CaseManagement.CMMN.CaseInstance.Repositories;
using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Infrastructures;
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
using System.Net;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.AspNetCore.Controllers
{
    [Route(CMMNApiConstants.RouteNames.CaseInstances)]
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
        private readonly IActivateCommandHandler _activateCommandHandler;
        private readonly ICaseInstanceQueryRepository _cmmnWorkflowInstanceQueryRepository;
        private readonly ICaseFileItemRepository _caseFileItemRepository;

        public CaseInstancesController(ICreateCaseInstanceCommandHandler createCaseInstanceCommandHandler, ILaunchCaseInstanceCommandHandler launchCaseInstanceCommandHandler, ISuspendCommandHandler suspendCommandHandler, IResumeCommandHandler resumeCommandHandler, ITerminateCommandHandler terminateCommandHandler, IReactivateCommandHandler reactivateCommandHandler, ICloseCommandHandler closeCommandHandler, IConfirmFormCommandHandler confirmFormCommandHandler, IActivateCommandHandler activateCommandHandler, ICaseInstanceQueryRepository cmmnWorkflowInstanceQueryRepository, ICaseFileItemRepository caseFileItemRepository)
        {
            _createCaseInstanceCommandHandler = createCaseInstanceCommandHandler;
            _launchCaseInstanceCommandHandler = launchCaseInstanceCommandHandler;
            _suspendCommandHandler = suspendCommandHandler;
            _resumeCommandHandler = resumeCommandHandler;
            _terminateCommandHandler = terminateCommandHandler;
            _reactivateCommandHandler = reactivateCommandHandler;
            _closeCommandHandler = closeCommandHandler;
            _confirmFormCommandHandler = confirmFormCommandHandler;
            _activateCommandHandler = activateCommandHandler;
            _cmmnWorkflowInstanceQueryRepository = cmmnWorkflowInstanceQueryRepository;
            _caseFileItemRepository = caseFileItemRepository;
        }

        [HttpGet(".search")]
        public async Task<IActionResult> Search()
        {
            var query = HttpContext.Request.Query;
            var result = await _cmmnWorkflowInstanceQueryRepository.Find(ExtractFindParameter(query));
            return new OkObjectResult(ToDto(result));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var result = await _cmmnWorkflowInstanceQueryRepository.FindFlowInstanceById(id);
            if (result == null)
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(ToDto(result));
        }

        [HttpGet("{id}/casefileitems")]
        public async Task<IActionResult> GetCaseFileItems(string id)
        {
            var result = await _caseFileItemRepository.FindByCaseInstance(id);
            return new OkObjectResult(ToDto(result));
        }

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

        private static JObject ToDto(FindResponse<Domains.CaseInstance> resp)
        {
            return new JObject
            {
                { "start_index", resp.StartIndex },
                { "total_length", resp.TotalLength },
                { "count", resp.Count },
                { "content", new JArray(resp.Content.Select(r => ToDto(r))) }
            };
        }

        private static JObject ToDto(IEnumerable<CaseFileItem> caseFileItems)
        {
            var jArr = new JArray();
            var jObj = new JObject
            {
                { "casefileitems", jArr }
            };
            foreach(var caseFileItem in caseFileItems)
            {
                jArr.Add(ToDto(caseFileItem));
            }

            return jObj;
        }

        private static JObject ToDto(CaseFileItem caseFileItem)
        {
            var result = new JObject
            {
                { "element_definition_id", caseFileItem.CaseElementDefinitionId },
                { "element_instance_id", caseFileItem.CaseElementInstanceId },
                { "case_instance_id", caseFileItem.CaseInstanceId },
                { "value", caseFileItem.Value },
                { "id", caseFileItem.Id },
                { "type", caseFileItem.Type },
                { "create_datetime", caseFileItem.CreateDateTime }
            };
            return result;
        }


        private static JObject ToDto(Domains.CaseInstance workflowInstance)
        {
            var result = new JObject
            {
                { "id", workflowInstance.Id },
                { "create_datetime", workflowInstance.CreateDateTime},
                { "definition_id", workflowInstance.CaseDefinitionId },
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
                    { "id", executionHistory.CaseElementDefinitionId }
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

        private static JObject ToDto(CaseElementInstance elt)
        {
            var result = new JObject
            {
                { "id", elt.Id },
                { "version", elt.Version },
                { "create_datetime", elt.CreateDateTime},
                { "definition_id", elt.CaseElementDefinitionId },
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

        private static JObject ToDto(CaseInstanceExecutionContext context)
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

        private static FindWorkflowInstanceParameter ExtractFindParameter(IQueryCollection query)
        {
            int startIndex;
            int count;
            string orderBy;
            FindOrders findOrder;
            string caseDefinitionId;
            var parameter = new FindWorkflowInstanceParameter();
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

            if (query.TryGet("case_definition_id", out caseDefinitionId))
            {
                parameter.CaseDefinitionId = caseDefinitionId;
            }

            return parameter;
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