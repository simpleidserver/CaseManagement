using CaseManagement.CMMN.AspNetCore.Extensions;
using CaseManagement.CMMN.CasePlanInstance.Commands;
using CaseManagement.CMMN.CasePlanInstance.Exceptions;
using CaseManagement.CMMN.CasePlanInstance.Queries;
using CaseManagement.CMMN.Common.Exceptions;
using CaseManagement.Common.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.AspNetCore.Controllers
{
    [Route(CMMNConstants.RouteNames.CasePlanInstances)]
    public class CasePlanInstancesController : Controller
    {
        private readonly IMediator _mediator;

        public CasePlanInstancesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("search")]
        [Authorize("search_caseplaninstance")]
        public async Task<IActionResult> Search([FromBody] SearchCasePlanInstanceQuery query, CancellationToken token)
        {
            var result = await _mediator.Send(query, token);
            return new OkObjectResult(result);
        }

        [HttpGet("{id}")]
        [Authorize("get_caseplaninstance")]
        public async Task<IActionResult> Get(string id, CancellationToken token)
        {
            try
            {
                var result = await _mediator.Send(new GetCasePlanInstanceQuery { CasePlanInstanceId = id }, token);
                return new OkObjectResult(result);
            }
            catch (UnknownCasePlanInstanceException)
            {
                return new NotFoundResult();
            }
        }

        [HttpPost]
        [Authorize("add_caseplaninstance")]
        public async Task<IActionResult> Create([FromBody] CreateCaseInstanceCommand createCaseInstance, CancellationToken token)
        {
            try
            {
                var nameIdentifier = User.GetClaims().GetUserNameIdentifier();
                createCaseInstance.NameIdentifier = nameIdentifier;
                var result = await _mediator.Send(createCaseInstance, token);
                return new OkObjectResult(result);
            }
            catch (UnknownCasePlanException)
            {
                return this.ToError(new Dictionary<string, string>
                {
                    { "bad_request", "case definition doesn't exist" }
                }, HttpStatusCode.NotFound, Request);
            }
        }

        [HttpGet("{id}/launch")]
        [Authorize("launch_caseplaninstance")]
        public async Task<IActionResult> Launch(string id, CancellationToken token)
        {
            try
            {
                await _mediator.Send(new LaunchCaseInstanceCommand { CasePlanInstanceId = id }, token);
                return new OkResult();
            }
            catch (UnknownCasePlanInstanceException)
            {
                return this.ToError(new Dictionary<string, string>
                {
                    { "bad_request", "case instance doesn't exist" }
                }, HttpStatusCode.NotFound, Request);
            }
        }

        [HttpPost("{id}/suspend")]
        [Authorize("suspend_caseplaninstance")]
        public async Task<IActionResult> Suspend(string id, [FromBody] SuspendCommand suspendCommand, CancellationToken token)
        {
            try
            {
                suspendCommand.CasePlanInstanceId = id;
                await _mediator.Send(new SuspendCommand(id, null), token);
                return new OkResult();
            }
            catch (UnknownCasePlanInstanceException)
            {
                return this.ToError(new Dictionary<string, string>
                {
                    { "bad_request", "case instance doesn't exist" }
                }, HttpStatusCode.NotFound, Request);
            }
            catch (UnknownCasePlanElementInstanceException)
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

        [HttpPost("{id}/suspend/{elt}")]
        [Authorize("suspend_caseplaninstance")]
        public async Task<IActionResult> Suspend(string id, string elt, [FromBody] SuspendCommand suspendCommand, CancellationToken token)
        {
            try
            {
                suspendCommand.CasePlanInstanceId = id;
                suspendCommand.CasePlanInstanceElementId = elt;
                await _mediator.Send(suspendCommand, token);
                return new OkResult();
            }
            catch (UnknownCasePlanInstanceException)
            {
                return this.ToError(new Dictionary<string, string>
                {
                    { "bad_request", "case instance doesn't exist" }
                }, HttpStatusCode.NotFound, Request);
            }
            catch (UnknownCasePlanElementInstanceException)
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

        [HttpPost("{id}/reactivate")]
        [Authorize("reactivate_caseplaninstance")]
        public async Task<IActionResult> Reactivate(string id, [FromBody] ReactivateCommand reactivateCommand, CancellationToken token)
        {
            try
            {
                reactivateCommand.CaseInstanceId = id;
                await _mediator.Send(reactivateCommand, token);
                return new OkResult();
            }
            catch (UnknownCasePlanInstanceException)
            {
                return this.ToError(new Dictionary<string, string>
                {
                    { "bad_request", "case instance doesn't exist" }
                }, HttpStatusCode.NotFound, Request);
            }
            catch (UnknownCasePlanElementInstanceException)
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

        [HttpPost("{id}/reactivate/{elt}")]
        [Authorize("reactivate_caseplaninstance")]
        public async Task<IActionResult> Reactivate(string id, string elt, [FromBody] ReactivateCommand reactivateCommand, CancellationToken token)
        {
            try
            {
                reactivateCommand.CaseInstanceId = id;
                reactivateCommand.CaseInstanceElementId = elt;
                await _mediator.Send(reactivateCommand, token);
                return new OkResult();
            }
            catch (UnknownCasePlanInstanceException)
            {
                return this.ToError(new Dictionary<string, string>
                {
                    { "bad_request", "case instance doesn't exist" }
                }, HttpStatusCode.NotFound, Request);
            }
            catch (UnknownCasePlanElementInstanceException)
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

        [HttpPost("{id}/resume")]
        [Authorize("resume_caseplaninstance")]
        public async Task<IActionResult> Resume(string id, [FromBody] ResumeCommand resumeCommand, CancellationToken token)
        {
            try
            {
                resumeCommand.CasePlanInstanceId = id;
                await _mediator.Send(resumeCommand, token);
                return new OkResult();
            }
            catch (UnknownCasePlanInstanceException)
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

        [HttpPost("{id}/resume/{elt}")]
        [Authorize("resume_caseplaninstance")]
        public async Task<IActionResult> Resume(string id, string elt, [FromBody] ResumeCommand resumeCommand, CancellationToken token)
        {
            try
            {
                resumeCommand.CasePlanInstanceId = id;
                resumeCommand.CasePlanInstanceElementId = elt;
                await _mediator.Send(resumeCommand, token);
                return new OkResult();
            }
            catch (UnknownCasePlanInstanceException)
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

        [HttpPost("{id}/complete/{elt}")]
        [Authorize("complete_caseplaninstance")]
        public async Task<IActionResult> Complete(string id, string elt, [FromBody] CompleteCommand command, CancellationToken token)
        {
            try
            {
                command.CaseInstanceId = id;
                command.CaseInstanceElementId = elt;
                await _mediator.Send(command, token);
                return new OkResult();
            }
            catch (UnknownCasePlanInstanceException)
            {
                return this.ToError(new Dictionary<string, string>
                {
                    { "bad_request", "case instance doesn't exist" }
                }, HttpStatusCode.NotFound, Request);
            }
            catch (UnknownCasePlanElementInstanceException)
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

        [HttpPost("{id}/terminate")]
        [Authorize("terminate_caseplaninstance")]
        public async Task<IActionResult> Terminate(string id, [FromBody] TerminateCommand terminateCommand, CancellationToken token)
        {
            try
            {
                terminateCommand.CaseInstanceId = id;
                await _mediator.Send(terminateCommand, token);
                return new OkResult();
            }
            catch (UnknownCasePlanInstanceException)
            {
                return this.ToError(new Dictionary<string, string>
                {
                    { "bad_request", "case instance doesn't exist" }
                }, HttpStatusCode.NotFound, Request);
            }
            catch (UnknownCasePlanElementInstanceException)
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

        [HttpPost("{id}/terminate/{elt}")]
        [Authorize("terminate_caseplaninstance")]
        public async Task<IActionResult> Terminate(string id, string elt, [FromBody] TerminateCommand terminateCommand, CancellationToken token)
        {
            try
            {
                terminateCommand.CaseInstanceId = id;
                terminateCommand.CaseInstanceElementId = elt;
                await _mediator.Send(terminateCommand, token);
                return new OkResult();
            }
            catch (UnknownCasePlanInstanceException)
            {
                return this.ToError(new Dictionary<string, string>
                {
                    { "bad_request", "case instance doesn't exist" }
                }, HttpStatusCode.NotFound, Request);
            }
            catch (UnknownCasePlanElementInstanceException)
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

        [HttpPost("{id}/close")]
        [Authorize("close_caseplaninstance")]
        public async Task<IActionResult> Close(string id, [FromBody] CloseCommand closeCommand, CancellationToken token)
        {
            try
            {
                closeCommand.CasePlanInstanceId = id;
                await _mediator.Send(closeCommand, token);
                return new OkResult();
            }
            catch (UnknownCasePlanInstanceException)
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

        [HttpPost("{id}/activate/{elt}")]
        [Authorize("activate_caseplaninstance")]
        public async Task<IActionResult> Activate(string id, string elt, [FromBody] ActivateCommand activateCommand, CancellationToken token)
        {
            try
            {
                activateCommand.CasePlanInstanceId = id;
                activateCommand.CasePlanElementInstanceId = elt;
                await _mediator.Send(activateCommand, token);
                return new OkResult();
            }
            catch (UnknownCasePlanInstanceException)
            {
                return new NotFoundResult();
            }
            catch (UnknownCasePlanElementInstanceException)
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

        [HttpPost("{id}/disable/{elt}")]
        [Authorize("disable_caseplaninstance")]
        public async Task<IActionResult> Disable(string id, string elt, [FromBody] DisableCommand disableCommand, CancellationToken token)
        {
            try
            {
                disableCommand.CasePlanInstanceId = id;
                disableCommand.CasePlanElementInstanceId = elt;
                await _mediator.Send(disableCommand, token);
                return new OkResult();
            }
            catch (UnknownCasePlanInstanceException)
            {
                return new NotFoundResult();
            }
            catch (UnknownCasePlanElementInstanceException)
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

        [HttpPost("{id}/reenable/{elt}")]
        [Authorize("reenable_caseplaninstance")]
        public async Task<IActionResult> Reenable(string id, string elt, [FromBody] ReenableCommand reenableCommand, CancellationToken token)
        {
            try
            {
                reenableCommand.CasePlanInstanceId = id;
                reenableCommand.CasePlanElementInstanceId = elt;
                await _mediator.Send(reenableCommand, token);
                return new OkResult();
            }
            catch (UnknownCasePlanInstanceException)
            {
                return new NotFoundResult();
            }
            catch (UnknownCasePlanElementInstanceException)
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