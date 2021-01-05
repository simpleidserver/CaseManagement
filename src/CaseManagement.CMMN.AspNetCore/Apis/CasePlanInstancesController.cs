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

        [HttpGet("{id}/suspend")]
        [Authorize("suspend_caseplaninstance")]
        public async Task<IActionResult> Suspend(string id, CancellationToken token)
        {
            try
            {
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

        [HttpGet("{id}/suspend/{elt}")]
        [Authorize("suspend_caseplaninstance")]
        public async Task<IActionResult> Suspend(string id, string elt, CancellationToken token)
        {
            try
            {
                await _mediator.Send(new SuspendCommand(id, elt), token);
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

        [HttpGet("{id}/reactivate")]
        [Authorize("reactivate_caseplaninstance")]
        public async Task<IActionResult> Reactivate(string id, CancellationToken token)
        {
            try
            {
                await _mediator.Send(new ReactivateCommand(id, null), token);
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

        [HttpGet("{id}/reactivate/{elt}")]
        [Authorize("reactivate_caseplaninstance")]
        public async Task<IActionResult> Reactivate(string id, string elt, CancellationToken token)
        {
            try
            {
                await _mediator.Send(new ReactivateCommand(id, elt), token);
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

        [HttpGet("{id}/resume")]
        [Authorize("resume_caseplaninstance")]
        public async Task<IActionResult> Resume(string id, CancellationToken token)
        {
            try
            {
                await _mediator.Send(new ResumeCommand(id, null), token);
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

        [HttpGet("{id}/resume/{elt}")]
        [Authorize("resume_caseplaninstance")]
        public async Task<IActionResult> Resume(string id, string elt, CancellationToken token)
        {
            try
            {
                await _mediator.Send(new ResumeCommand(id, elt), token);
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

        [HttpGet("{id}/terminate")]
        [Authorize("terminate_caseplaninstance")]
        public async Task<IActionResult> Terminate(string id, CancellationToken token)
        {
            try
            {
                await _mediator.Send(new TerminateCommand(id, null), token);
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

        [HttpGet("{id}/terminate/{elt}")]
        [Authorize("terminate_caseplaninstance")]
        public async Task<IActionResult> Terminate(string id, string elt, CancellationToken token)
        {
            try
            {
                await _mediator.Send(new TerminateCommand(id, elt), token);
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

        [HttpGet("{id}/close")]
        [Authorize("close_caseplaninstance")]
        public async Task<IActionResult> Close(string id, CancellationToken token)
        {
            try
            {
                await _mediator.Send(new CloseCommand(id), token);
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

        [HttpGet("{id}/activate/{elt}")]
        [Authorize("activate_caseplaninstance")]
        public async Task<IActionResult> Activate(string id, string elt, CancellationToken token)
        {
            try
            {
                await _mediator.Send(new ActivateCommand(id, elt), token);
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

        [HttpGet("{id}/disable/{elt}")]
        [Authorize("disable_caseplaninstance")]
        public async Task<IActionResult> Disable(string id, string elt, CancellationToken token)
        {
            try
            {
                await _mediator.Send(new DisableCommand(id, elt), token);
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

        [HttpGet("{id}/reenable/{elt}")]
        [Authorize("reenable_caseplaninstance")]
        public async Task<IActionResult> Reenable(string id, string elt, CancellationToken token)
        {
            try
            {
                await _mediator.Send(new ReenableCommand(id, elt), token);
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