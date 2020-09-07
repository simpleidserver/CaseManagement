using CaseManagement.CMMN.AspNetCore.Extensions;
using CaseManagement.CMMN.CasePlanInstance;
using CaseManagement.CMMN.CasePlanInstance.Commands;
using CaseManagement.CMMN.CasePlanInstance.Exceptions;
using CaseManagement.CMMN.Infrastructures;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.AspNetCore.Controllers
{
    [Route(CMMNConstants.RouteNames.CasePlanInstances)]
    public class CasePlanInstancesController : Controller
    {
        private readonly ICasePlanInstanceService _casePlanInstanceService;

        public CasePlanInstancesController(ICasePlanInstanceService casePlanInstanceService)
        {
            _casePlanInstanceService = casePlanInstanceService;
        }

        [HttpGet("search")]
        [Authorize("search_caseplaninstance")]
        public async Task<IActionResult> Search()
        {
            var query = HttpContext.Request.Query.ToEnumerable();
            var result = await _casePlanInstanceService.Search(query);
            return new OkObjectResult(result);
        }

        [HttpGet("me/search")]
        [Authorize("me_search_caseplaninstance")]
        public async Task<IActionResult> SearchMe()
        {
            var query = HttpContext.Request.Query.ToEnumerable();
            var result = await _casePlanInstanceService.SearchMe(query, this.GetNameIdentifier());
            return new OkObjectResult(result);
        }

        [HttpGet("me/{id}")]
        [Authorize("me_get_caseplaninstance")]
        public async Task<IActionResult> GetMe(string id)
        {
            try
            {
                var result = await _casePlanInstanceService.GetMe(id, this.GetNameIdentifier());
                return new OkObjectResult(result);
            }
            catch(UnknownCaseInstanceException)
            {
                return new NotFoundResult();
            }
            catch(UnauthorizedCaseWorkerException)
            {
                return new UnauthorizedResult();
            }
        }

        [HttpGet("{id}")]
        [Authorize("get_caseplaninstance")]
        public async Task<IActionResult> Get(string id)
        {
            try
            {
                var result = await _casePlanInstanceService.Get(id);
                return new OkObjectResult(result);
            }
            catch (UnknownCaseInstanceException)
            {
                return new NotFoundResult();
            }
        }

        [HttpGet("{id}/casefileitems")]
        [Authorize("get_casefileitems")]
        public async Task<IActionResult> GetCaseFileItems(string id)
        {
            var result = await _casePlanInstanceService.GetCaseFileItems(id);
            return new OkObjectResult(result);
        }

        [HttpPost("me")]
        [Authorize("me_add_caseplaninstance")]
        public async Task<IActionResult> CreateMe([FromBody] CreateCaseInstanceCommand createCaseInstance)
        {
            createCaseInstance.Performer = this.GetNameIdentifier();
            try
            {
                var result = await _casePlanInstanceService.CreateMe(createCaseInstance);
                return new OkObjectResult(result);
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

        [HttpPost]
        [Authorize("add_caseplaninstance")]
        public async Task<IActionResult> Create([FromBody] CreateCaseInstanceCommand createCaseInstance)
        {
            try
            {
                var result = await _casePlanInstanceService.Create(createCaseInstance);
                return new OkObjectResult(result);
            }
            catch (UnknownCaseDefinitionException)
            {
                return this.ToError(new Dictionary<string, string>
                {
                    { "bad_request", "case definition doesn't exist" }
                }, HttpStatusCode.NotFound, Request);
            }
        }

        [HttpGet("me/{id}/launch")]
        [Authorize("me_launch_caseplaninstance")]
        public async Task<IActionResult> LaunchMe(string id)
        {
            try
            {
                await _casePlanInstanceService.LaunchMe(new LaunchCaseInstanceCommand { Performer = this.GetNameIdentifier(), CasePlanInstanceId = id });
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

        [HttpGet("{id}/launch")]
        [Authorize("launch_caseplaninstance")]
        public async Task<IActionResult> Launch(string id)
        {
            try
            {
                await _casePlanInstanceService.Launch(new LaunchCaseInstanceCommand { CasePlanInstanceId = id });
                return new OkResult();
            }
            catch (UnknownCaseInstanceException)
            {
                return this.ToError(new Dictionary<string, string>
                {
                    { "bad_request", "case instance doesn't exist" }
                }, HttpStatusCode.NotFound, Request);
            }
        }

        [HttpGet("me/{id}/suspend")]
        [Authorize("me_suspend_caseplaninstance")]
        public async Task<IActionResult> SuspendMe(string id)
        {
            try
            {
                await _casePlanInstanceService.SuspendMe(new SuspendCommand(id, null) { Performer = this.GetNameIdentifier() });
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

        [HttpGet("{id}/suspend")]
        [Authorize("suspend_caseplaninstance")]
        public async Task<IActionResult> Suspend(string id)
        {
            try
            {
                await _casePlanInstanceService.Suspend(new SuspendCommand(id, null));
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

        [HttpGet("me/{id}/suspend/{elt}")]
        [Authorize("me_suspend_caseplaninstance")]
        public async Task<IActionResult> SuspendMe(string id, string elt)
        {
            try
            {
                await _casePlanInstanceService.SuspendMe(new SuspendCommand(id, elt) { Performer = this.GetNameIdentifier() });
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

        [HttpGet("{id}/suspend/{elt}")]
        [Authorize("suspend_caseplaninstance")]
        public async Task<IActionResult> Suspend(string id, string elt)
        {
            try
            {
                await _casePlanInstanceService.Suspend(new SuspendCommand(id, elt));
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

        [HttpGet("me/{id}/reactivate")]
        [Authorize("me_reactivate_caseplaninstance")]
        public async Task<IActionResult> ReactivateMe(string id)
        {
            try
            {
                await _casePlanInstanceService.ReactivateMe(new ReactivateCommand(id, null) { Performer = this.GetNameIdentifier() });
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

        [HttpGet("{id}/reactivate")]
        [Authorize("reactivate_caseplaninstance")]
        public async Task<IActionResult> Reactivate(string id)
        {
            try
            {
                await _casePlanInstanceService.Reactivate(new ReactivateCommand(id, null));
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

        [HttpGet("me/{id}/reactivate/{elt}")]
        [Authorize("me_reactivate_caseplaninstance")]
        public async Task<IActionResult> ReactivateMe(string id, string elt)
        {
            try
            {
                await _casePlanInstanceService.ReactivateMe(new ReactivateCommand(id, elt) { Performer = this.GetNameIdentifier() });
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

        [HttpGet("{id}/reactivate/{elt}")]
        [Authorize("reactivate_caseplaninstance")]
        public async Task<IActionResult> Reactivate(string id, string elt)
        {
            try
            {
                await _casePlanInstanceService.Reactivate(new ReactivateCommand(id, elt));
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

        [HttpGet("me/{id}/resume")]
        [Authorize("me_resume_caseplaninstance")]
        public async Task<IActionResult> ResumeMe(string id)
        {
            try
            {
                await _casePlanInstanceService.ResumeMe(new ResumeCommand(id, null) { Performer = this.GetNameIdentifier() });
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

        [HttpGet("{id}/resume")]
        [Authorize("resume_caseplaninstance")]
        public async Task<IActionResult> Resume(string id)
        {
            try
            {
                await _casePlanInstanceService.Resume(new ResumeCommand(id, null));
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

        [HttpGet("me/{id}/resume/{elt}")]
        [Authorize("resume_caseplaninstance")]
        public async Task<IActionResult> ResumeMe(string id, string elt)
        {
            try
            {
                await _casePlanInstanceService.ResumeMe(new ResumeCommand(id, elt) { Performer = this.GetNameIdentifier() });
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

        [HttpGet("{id}/resume/{elt}")]
        [Authorize("resume_caseplaninstance")]
        public async Task<IActionResult> Resume(string id, string elt)
        {
            try
            {
                await _casePlanInstanceService.Resume(new ResumeCommand(id, elt));
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

        [HttpGet("me/{id}/terminate")]
        [Authorize("me_terminate_caseplaninstance")]
        public async Task<IActionResult> TerminateMe(string id)
        {
            try
            {
                await _casePlanInstanceService.TerminateMe(new TerminateCommand(id, null) { Performer = this.GetNameIdentifier() });
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

        [HttpGet("{id}/terminate")]
        [Authorize("terminate_caseplaninstance")]
        public async Task<IActionResult> Terminate(string id)
        {
            try
            {
                await _casePlanInstanceService.Terminate(new TerminateCommand(id, null));
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

        [HttpGet("me/{id}/terminate/{elt}")]
        [Authorize("me_terminate_caseplaninstance")]
        public async Task<IActionResult> TerminateMe(string id, string elt)
        {
            try
            {
                await _casePlanInstanceService.TerminateMe(new TerminateCommand(id, elt) { Performer = this.GetNameIdentifier() });
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

        [HttpGet("{id}/terminate/{elt}")]
        [Authorize("terminate_caseplaninstance")]
        public async Task<IActionResult> Terminate(string id, string elt)
        {
            try
            {
                await _casePlanInstanceService.Terminate(new TerminateCommand(id, elt));
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

        [HttpGet("me/{id}/close")]
        [Authorize("me_close_caseplaninstance")]
        public async Task<IActionResult> CloseMe(string id)
        {
            try
            {
                await _casePlanInstanceService.CloseMe(new CloseCommand(id) { Performer = this.GetNameIdentifier() });
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

        [HttpGet("{id}/close")]
        [Authorize("close_caseplaninstance")]
        public async Task<IActionResult> Close(string id)
        {
            try
            {
                await _casePlanInstanceService.Close(new CloseCommand(id));
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

        [HttpPost("me/{id}/confirm/{elt}")]
        [Authorize("me_confirm_caseplaninstance")]
        public async Task<IActionResult> ConfirmFormMe(string id, string elt, [FromBody] JObject jObj)
        {
            try
            {
                await _casePlanInstanceService.ConfirmFormMe(new ConfirmFormCommand { CasePlanInstanceId = id, CasePlanElementInstanceId = elt, Content = jObj, Performer = this.GetNameIdentifier() });
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

        [HttpPost("{id}/confirm/{elt}")]
        [Authorize("confirm_caseplaninstance")]
        public async Task<IActionResult> ConfirmForm(string id, string elt, [FromBody] JObject jObj)
        {
            try
            {
                await _casePlanInstanceService.ConfirmForm(new ConfirmFormCommand { CasePlanInstanceId = id, CasePlanElementInstanceId = elt, Content = jObj });
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

        [HttpGet("me/{id}/activate/{elt}")]
        [Authorize("me_activate_caseplaninstance")]
        public async Task<IActionResult> ActivateMe(string id, string elt)
        {
            try
            {
                await _casePlanInstanceService.ActivateMe(new ActivateCommand(id, elt) { Performer = this.GetNameIdentifier() });
                return new OkResult();
            }
            catch (UnknownCaseWorkerTaskException)
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

        [HttpGet("{id}/activate/{elt}")]
        [Authorize("activate_caseplaninstance")]
        public async Task<IActionResult> Activate(string id, string elt)
        {
            try
            {
                await _casePlanInstanceService.Activate(new ActivateCommand(id, elt));
                return new OkResult();
            }
            catch (UnknownCaseWorkerTaskException)
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
    }
}