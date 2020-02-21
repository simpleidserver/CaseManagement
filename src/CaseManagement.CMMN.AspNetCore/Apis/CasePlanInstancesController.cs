using CaseManagement.CMMN.AspNetCore.Extensions;
using CaseManagement.CMMN.CasePlanInstance.CommandHandlers;
using CaseManagement.CMMN.CasePlanInstance.Commands;
using CaseManagement.CMMN.CasePlanInstance.Exceptions;
using CaseManagement.CMMN.CasePlanInstance.Repositories;
using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Extensions;
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
    [Route(CMMNConstants.RouteNames.CasePlanInstances)]
    public class CasePlanInstancesController : Controller
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
        private readonly ICasePlanInstanceQueryRepository _cmmnWorkflowInstanceQueryRepository;
        private readonly ICaseFileItemRepository _caseFileItemRepository;
        private readonly IRoleQueryRepository _roleQueryRepository;

        public CasePlanInstancesController(ICreateCaseInstanceCommandHandler createCaseInstanceCommandHandler, ILaunchCaseInstanceCommandHandler launchCaseInstanceCommandHandler, ISuspendCommandHandler suspendCommandHandler, IResumeCommandHandler resumeCommandHandler, ITerminateCommandHandler terminateCommandHandler, IReactivateCommandHandler reactivateCommandHandler, ICloseCommandHandler closeCommandHandler, IConfirmFormCommandHandler confirmFormCommandHandler, IActivateCommandHandler activateCommandHandler, ICasePlanInstanceQueryRepository cmmnWorkflowInstanceQueryRepository, ICaseFileItemRepository caseFileItemRepository, IRoleQueryRepository roleQueryRepository)
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
            _roleQueryRepository = roleQueryRepository;
        }

        [HttpGet("search")]
        [Authorize("search_caseplaninstance")]
        public async Task<IActionResult> Search()
        {
            var query = HttpContext.Request.Query.ToEnumerable();
            var result = await _cmmnWorkflowInstanceQueryRepository.Find(ExtractFindParameter(query));
            return new OkObjectResult(ToDto(result));
        }

        [HttpGet("me/search")]
        [Authorize("me_search_caseplaninstance")]
        public async Task<IActionResult> SearchMe()
        {
            var query = HttpContext.Request.Query.ToEnumerable();
            var roles = await _roleQueryRepository.FindRolesByUser(this.GetNameIdentifier());
            var parameter = ExtractFindParameter(query);
            parameter.Roles = roles.Select(r => r.Id).ToList();
            var result = await _cmmnWorkflowInstanceQueryRepository.Find(parameter);
            return new OkObjectResult(ToDto(result));
        }

        [HttpGet("me/{id}")]
        [Authorize("me_get_caseplaninstance")]
        public async Task<IActionResult> GetMe(string id)
        {
            var result = await _cmmnWorkflowInstanceQueryRepository.FindFlowInstanceById(id);
            if (result == null)
            {
                return new NotFoundResult();
            }

            if (result.CaseOwner != this.GetNameIdentifier())
            {
                return new UnauthorizedResult();
            }

            return new OkObjectResult(ToDto(result));
        }

        [HttpGet("{id}")]
        [Authorize("get_caseplaninstance")]
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
        [Authorize("get_casefileitems")]
        public async Task<IActionResult> GetCaseFileItems(string id)
        {
            var result = await _caseFileItemRepository.FindByCaseInstance(id);
            return new OkObjectResult(ToDto(result));
        }

        [HttpPost("me")]
        [Authorize("me_add_caseplaninstance")]
        public Task<IActionResult> CreateMe([FromBody] CreateCaseInstanceCommand createCaseInstance)
        {
            createCaseInstance.Performer = this.GetNameIdentifier();
            return InternalCreate(createCaseInstance);
        }

        [HttpPost]
        [Authorize("add_caseplaninstance")]
        public Task<IActionResult> Create([FromBody] CreateCaseInstanceCommand createCaseInstance)
        {
            createCaseInstance.BypassUserValidation = true;
            return InternalCreate(createCaseInstance);
        }

        [HttpGet("me/{id}/launch")]
        [Authorize("me_launch_caseplaninstance")]
        public Task<IActionResult> LaunchMe(string id)
        {
            return InternalLaunch(new LaunchCaseInstanceCommand { Performer = this.GetNameIdentifier(), CasePlanInstanceId = id });
        }

        [HttpGet("{id}/launch")]
        [Authorize("launch_caseplaninstance")]
        public Task<IActionResult> Launch(string id)
        {
            return InternalLaunch(new LaunchCaseInstanceCommand { BypassUserValidation = true, CasePlanInstanceId = id });
        }

        [HttpGet("me/{id}/suspend")]
        [Authorize("me_suspend_caseplaninstance")]
        public Task<IActionResult> SuspendMe(string id)
        {
            return InternalSuspend(new SuspendCommand(id, null)
            {
                BypassUserValidation = false,
                Performer = this.GetNameIdentifier()
            });
        }

        [HttpGet("{id}/suspend")]
        [Authorize("suspend_caseplaninstance")]
        public Task<IActionResult> Suspend(string id)
        {
            return InternalSuspend(new SuspendCommand(id, null)
            {
                BypassUserValidation = true
            });
        }

        [HttpGet("me/{id}/suspend/{elt}")]
        [Authorize("me_suspend_caseplaninstance")]
        public Task<IActionResult> SuspendMe(string id, string elt)
        {
            return InternalSuspend(new SuspendCommand(id, elt)
            {
                Performer = this.GetNameIdentifier(),
                BypassUserValidation = false
            });
        }

        [HttpGet("{id}/suspend/{elt}")]
        [Authorize("suspend_caseplaninstance")]
        public Task<IActionResult> Suspend(string id, string elt)
        {
            return InternalSuspend(new SuspendCommand(id, elt)
            {
                BypassUserValidation = true
            });
        }

        [HttpGet("me/{id}/reactivate")]
        [Authorize("me_reactivate_caseplaninstance")]
        public Task<IActionResult> ReactivateMe(string id)
        {
            return InternalReactivate(new ReactivateCommand(id, null)
            {
                BypassUserValidation = false,
                Performer = this.GetNameIdentifier()
            });
        }

        [HttpGet("{id}/reactivate")]
        [Authorize("reactivate_caseplaninstance")]
        public Task<IActionResult> Reactivate(string id)
        {
            return InternalReactivate(new ReactivateCommand(id, null)
            {
                BypassUserValidation = true
            });
        }

        [HttpGet("me/{id}/reactivate/{elt}")]
        [Authorize("me_reactivate_caseplaninstance")]
        public Task<IActionResult> ReactivateMe(string id, string elt)
        {
            return InternalReactivate(new ReactivateCommand(id, elt)
            {
                BypassUserValidation = false,
                Performer = this.GetNameIdentifier()
            });
        }

        [HttpGet("{id}/reactivate/{elt}")]
        [Authorize("reactivate_caseplaninstance")]
        public Task<IActionResult> Reactivate(string id, string elt)
        {
            return InternalReactivate(new ReactivateCommand(id, elt)
            {
                BypassUserValidation = true
            });
        }

        [HttpGet("me/{id}/resume")]
        [Authorize("me_resume_caseplaninstance")]
        public Task<IActionResult> ResumeMe(string id)
        {
            return InternalResume(new ResumeCommand(id, null)
            {
                BypassUserValidation = false,
                Performer = this.GetNameIdentifier()
            });
        }

        [HttpGet("{id}/resume")]
        [Authorize("resume_caseplaninstance")]
        public Task<IActionResult> Resume(string id)
        {
            return InternalResume(new ResumeCommand(id, null)
            {
                BypassUserValidation = true
            });
        }

        [HttpGet("me/{id}/resume/{elt}")]
        [Authorize("resume_caseplaninstance")]
        public Task<IActionResult> ResumeMe(string id, string elt)
        {
            return InternalResume(new ResumeCommand(id, elt)
            {
                BypassUserValidation = false,
                Performer = this.GetNameIdentifier()
            });
        }

        [HttpGet("{id}/resume/{elt}")]
        [Authorize("resume_caseplaninstance")]
        public Task<IActionResult> Resume(string id, string elt)
        {
            return InternalResume(new ResumeCommand(id, elt)
            {
                BypassUserValidation = true
            });
        }

        [HttpGet("me/{id}/terminate")]
        [Authorize("me_terminate_caseplaninstance")]
        public Task<IActionResult> TerminateMe(string id)
        {
            return InternalTerminate(new TerminateCommand(id, null)
            {
                BypassUserValidation = true,
                Performer = this.GetNameIdentifier()
            });
        }

        [HttpGet("{id}/terminate")]
        [Authorize("terminate_caseplaninstance")]
        public Task<IActionResult> Terminate(string id)
        {
            return InternalTerminate(new TerminateCommand(id, null)
            {
                BypassUserValidation = true
            });
        }

        [HttpGet("me/{id}/terminate/{elt}")]
        [Authorize("me_terminate_caseplaninstance")]
        public Task<IActionResult> TerminateMe(string id, string elt)
        {
            return InternalTerminate(new TerminateCommand(id, elt)
            {
                BypassUserValidation = true,
                Performer = this.GetNameIdentifier()
            });
        }

        [HttpGet("{id}/terminate/{elt}")]
        [Authorize("terminate_caseplaninstance")]
        public Task<IActionResult> Terminate(string id, string elt)
        {
            return InternalTerminate(new TerminateCommand(id, elt)
            {
                BypassUserValidation = true
            });
        }

        [HttpGet("me/{id}/close")]
        [Authorize("me_close_caseplaninstance")]
        public Task<IActionResult> CloseMe(string id)
        {
            return InternalClose(new CloseCommand(id)
            {
                BypassUserValidation = false,
                Performer = this.GetNameIdentifier()
            });
        }

        [HttpGet("{id}/close")]
        [Authorize("close_caseplaninstance")]
        public Task<IActionResult> Close(string id)
        {
            return InternalClose(new CloseCommand(id)
            {
                BypassUserValidation = true
            });
        }

        [HttpPost("me/{id}/confirm/{elt}")]
        [Authorize("me_confirm_caseplaninstance")]
        public Task<IActionResult> ConfirmFormMe(string id, string elt, [FromBody] JObject jObj)
        {
            return InternalConfirmForm(new ConfirmFormCommand { CasePlanInstanceId = id, CasePlanElementInstanceId = elt, Content = jObj, BypassUserValidation = false, Performer = this.GetNameIdentifier() });
        }

        [HttpPost("{id}/confirm/{elt}")]
        [Authorize("confirm_caseplaninstance")]
        public Task<IActionResult> ConfirmForm(string id, string elt, [FromBody] JObject jObj)
        {
            return InternalConfirmForm(new ConfirmFormCommand { CasePlanInstanceId = id, CasePlanElementInstanceId = elt, Content = jObj, BypassUserValidation = true });
        }

        [HttpGet("me/{id}/activate/{elt}")]
        [HttpGet("me_activate_caseplaninstance")]
        public Task<IActionResult> ActivateMe(string id, string elt)
        {
            return InternalActivate(new ActivateCommand(id, elt)
            {
                BypassUserValidation = false,
                Performer = this.GetNameIdentifier()
            });
        }

        [HttpGet("{id}/activate/{elt}")]
        [HttpGet("activate_caseplaninstance")]
        public Task<IActionResult> Activate(string id, string elt)
        {
            return InternalActivate(new ActivateCommand(id, elt)
            {
                BypassUserValidation = true
            });
        }

        private async Task<IActionResult> InternalCreate([FromBody] CreateCaseInstanceCommand createCaseInstance)
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
            catch (UnauthorizedCaseWorkerException)
            {
                return this.ToError(new Dictionary<string, string>
                {
                    { "unauthorized_request", "you're not authorized to create the case instance" }
                }, HttpStatusCode.Unauthorized, Request);
            }
            catch (UnknownCaseDefinitionException)
            {
                return this.ToError(new Dictionary<string, string>
                {
                    { "bad_request", "case definition doesn't exist" }
                }, HttpStatusCode.NotFound, Request);
            }
        }
        
        private async Task<IActionResult> InternalLaunch(LaunchCaseInstanceCommand launchCaseInstanceCommand)
        {
            try
            {
                await _launchCaseInstanceCommandHandler.Handle(launchCaseInstanceCommand);
                return new OkResult();
            }
            catch (UnauthorizedCaseWorkerException)
            {
                return this.ToError(new Dictionary<string, string>
                {
                    { "unauthorized_request", "you're not authorized to launch the case instance" }
                }, HttpStatusCode.Unauthorized, Request);
            }
            catch (UnknownCaseInstanceException)
            {
                return this.ToError(new Dictionary<string, string>
                {
                    { "bad_request", "case instance doesn't exist" }
                }, HttpStatusCode.NotFound, Request);
            }
        }

        private async Task<IActionResult> InternalSuspend(SuspendCommand suspendCommand)
        {
            try
            {
                await _suspendCommandHandler.Handle(suspendCommand);
                return new OkResult();
            }
            catch (UnknownCaseInstanceException)
            {
                return this.ToError(new Dictionary<string, string>
                {
                    { "bad_request", "case instance doesn't exist" }
                }, HttpStatusCode.NotFound, Request);
            }
            catch (UnknownCaseInstanceElementException)
            {
                return this.ToError(new Dictionary<string, string>
                {
                    { "bad_request", "case instance element doesn't exist" }
                }, HttpStatusCode.NotFound, Request);
            }
            catch (AggregateValidationException ex)
            {
                return this.ToError(ex.Errors, HttpStatusCode.BadRequest, Request);
            }
            catch (Exception ex)
            {
                return this.ToError(new Dictionary<string, string>
                {
                    { "invalid_request", ex.Message }
                }, HttpStatusCode.BadRequest, Request);
            }
        }

        private async Task<IActionResult> InternalReactivate(ReactivateCommand cmd)
        {
            try
            {
                await _reactivateCommandHandler.Handle(cmd);
                return new OkResult();
            }
            catch (UnknownCaseInstanceException)
            {
                return this.ToError(new Dictionary<string, string>
                {
                    { "bad_request", "case instance doesn't exist" }
                }, HttpStatusCode.NotFound, Request);
            }
            catch(UnknownCaseInstanceElementException)
            {
                return this.ToError(new Dictionary<string, string>
                {
                    { "bad_request", "case instance element doesn't exist" }
                }, HttpStatusCode.NotFound, Request);
            }
            catch (AggregateValidationException ex)
            {
                return this.ToError(ex.Errors, HttpStatusCode.BadRequest, Request);
            }
            catch (Exception ex)
            {
                return this.ToError(new Dictionary<string, string>
                {
                    { "invalid_request", ex.Message }
                }, HttpStatusCode.BadRequest, Request);
            }
        }

        private async Task<IActionResult> InternalResume(ResumeCommand cmd)
        {
            try
            {
                await _resumeCommandHandler.Handle(cmd);
                return new OkResult();
            }
            catch (UnknownCaseInstanceException)
            {
                return this.ToError(new Dictionary<string, string>
                {
                    { "bad_request", "case instance doesn't exist" }
                }, HttpStatusCode.NotFound, Request);
            }
            catch (AggregateValidationException ex)
            {
                return this.ToError(ex.Errors, HttpStatusCode.BadRequest, Request);
            }
            catch (Exception ex)
            {
                return this.ToError(new Dictionary<string, string>
                {
                    { "invalid_request", ex.Message }
                }, HttpStatusCode.BadRequest, Request);
            }
        }

        private async Task<IActionResult> InternalTerminate(TerminateCommand cmd)
        {
            try
            {
                await _terminateCommandHandler.Handle(cmd);
                return new OkResult();
            }
            catch (UnknownCaseInstanceException)
            {
                return this.ToError(new Dictionary<string, string>
                {
                    { "bad_request", "case instance doesn't exist" }
                }, HttpStatusCode.NotFound, Request);
            }
            catch (UnknownCaseInstanceElementException)
            {
                return this.ToError(new Dictionary<string, string>
                {
                    { "bad_request", "case instance element doesn't exist" }
                }, HttpStatusCode.NotFound, Request);
            }
            catch (AggregateValidationException ex)
            {
                return this.ToError(ex.Errors, HttpStatusCode.BadRequest, Request);
            }
            catch (Exception ex)
            {
                return this.ToError(new Dictionary<string, string>
                {
                    { "invalid_request", ex.Message }
                }, HttpStatusCode.BadRequest, Request);
            }
        }

        private async Task<IActionResult> InternalClose(CloseCommand cmd)
        {
            try
            {
                await _closeCommandHandler.Handle(cmd);
                return new OkResult();
            }
            catch (UnknownCaseInstanceException)
            {
                return this.ToError(new Dictionary<string, string>
                {
                    { "bad_request", "case instance doesn't exist" }
                }, HttpStatusCode.NotFound, Request);
            }
            catch (AggregateValidationException ex)
            {
                return this.ToError(ex.Errors, HttpStatusCode.BadRequest, Request);
            }
            catch (Exception ex)
            {
                return this.ToError(new Dictionary<string, string>
                {
                    { "invalid_request", ex.Message }
                }, HttpStatusCode.BadRequest, Request);
            }
        }

        private async Task<IActionResult> InternalConfirmForm(ConfirmFormCommand cmd)
        {
            try
            {
                await _confirmFormCommandHandler.Handle(cmd);
                return new OkResult();
            }
            catch (UnknownCaseInstanceException)
            {
                return this.ToError(new Dictionary<string, string>
                {
                    { "bad_request", "case instance doesn't exist" }
                }, HttpStatusCode.NotFound, Request);
            }
            catch (UnknownCaseInstanceElementException)
            {
                return this.ToError(new Dictionary<string, string>
                {
                    { "bad_request", "case instance element doesn't exist" }
                }, HttpStatusCode.NotFound, Request);
            }
            catch (AggregateValidationException ex)
            {
                return this.ToError(ex.Errors, HttpStatusCode.BadRequest, Request);
            }
            catch (UnauthorizedCaseWorkerException)
            {
                return this.ToError(new Dictionary<string, string>
                {
                    { "unauthorized_request", "you're not authorized to confirm the human task" }
                }, HttpStatusCode.Unauthorized, Request);
            }
            catch (Exception ex)
            {
                return this.ToError(new Dictionary<string, string>
                {
                    { "invalid_request", ex.Message }
                }, HttpStatusCode.BadRequest, Request);
            }
        }

        private async Task<IActionResult> InternalActivate(ActivateCommand cmd)
        {
            try
            {
                await _activateCommandHandler.Handle(cmd);
                return new OkResult();
            }
            catch (UnknownCaseActivationException)
            {
                return this.ToError(new Dictionary<string, string>
                {
                    { "bad_request", "case activation doesn't exist" }
                }, HttpStatusCode.NotFound, Request);
            }
            catch (UnknownCaseInstanceElementException)
            {
                return this.ToError(new Dictionary<string, string>
                {
                    { "bad_request", "case instance element doesn't exist" }
                }, HttpStatusCode.NotFound, Request);
            }
            catch (AggregateValidationException ex)
            {
                return this.ToError(ex.Errors, HttpStatusCode.BadRequest, Request);
            }
            catch (Exception ex)
            {
                return this.ToError(new Dictionary<string, string>
                {
                    { "invalid_request", ex.Message }
                }, HttpStatusCode.BadRequest, Request);
            }
        }

        private static JObject ToDto(FindResponse<Domains.CasePlanInstanceAggregate> resp)
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
        
        private static JObject ToDto(CasePlanInstanceAggregate workflowInstance)
        {
            var result = new JObject
            {
                { "id", workflowInstance.Id },
                { "create_datetime", workflowInstance.CreateDateTime},
                { "case_plan_id", workflowInstance.CasePlanId },
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
                { "case_plan_element_id", elt.CaseElementDefinitionId },
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

        private static FindWorkflowInstanceParameter ExtractFindParameter(IEnumerable<KeyValuePair<string, string>> query)
        {
            int startIndex;
            int count;
            string orderBy;
            FindOrders findOrder;
            string casePlanId;
            string owner;
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

            if (query.TryGet("case_plan_id", out casePlanId))
            {
                parameter.CasePlanId = casePlanId;
            }

            if(query.TryGet("owner", out owner))
            {
                parameter.CaseOwner = owner;
            }

            return parameter;
        }
    }
}