using CaseManagement.CMMN.AspNet.Extensions;
using CaseManagement.CMMN.CasePlanInstance;
using CaseManagement.CMMN.CasePlanInstance.Commands;
using CaseManagement.CMMN.CasePlanInstance.Exceptions;
using CaseManagement.CMMN.Infrastructures;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace CaseManagement.CMMN.AspNet.Controllers
{
    [RoutePrefix(CMMNConstants.RouteNames.CasePlanInstances)]
    public class CasePlanInstancesController : ApiController
    {
        private readonly ICasePlanInstanceService _casePlanInstanceService;

        public CasePlanInstancesController(ICasePlanInstanceService casePlanInstanceService)
        {
            _casePlanInstanceService = casePlanInstanceService;
        }

        [HttpGet]
        [Route("search")]
        public async Task<IHttpActionResult> Search()
        {
            var query = this.Request.GetQueryNameValuePairs();
            var result = await _casePlanInstanceService.Search(query);
            return Ok(result);
        }

        [HttpGet]
        [Route("me/search")]
        public async Task<IHttpActionResult> SearchMe()
        {
            var query = this.Request.GetQueryNameValuePairs();
            var result = await _casePlanInstanceService.SearchMe(query, this.GetNameIdentifier());
            return Ok(result);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IHttpActionResult> GetMe(string id)
        {
            try
            {
                var result = await _casePlanInstanceService.GetMe(id, this.GetNameIdentifier());
                return Ok(result);
            }
            catch (UnknownCasePlanInstanceException)
            {
                return NotFound();
            }
            catch (UnauthorizedCaseWorkerException)
            {
                return Unauthorized();
            }
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IHttpActionResult> Get(string id)
        {
            try
            {
                var result = await _casePlanInstanceService.Get(id);
                return Ok(result);
            }
            catch (UnknownCasePlanInstanceException)
            {
                return NotFound();
            }
        }

        [HttpGet]
        [Route("{id}/casefileitems")]
        public async Task<IHttpActionResult> GetCaseFileItems(string id)
        {
            var result = await _casePlanInstanceService.GetCaseFileItems(id);
            return Ok(result);
        }

        [HttpPost]
        [Route("me")]
        public async Task<IHttpActionResult> CreateMe([FromBody] CreateCaseInstanceCommand createCaseInstance)
        {
            createCaseInstance.Performer = this.GetNameIdentifier();
            try
            {
                var result = await _casePlanInstanceService.CreateMe(createCaseInstance);
                return Ok(result);
            }
            catch (UnauthorizedCaseWorkerException)
            {
                return Content(HttpStatusCode.Unauthorized, this.ToError(new Dictionary<string, string>
                {
                    { "unauthorized_request", "you're not authorized to create the case instance" }
                }, HttpStatusCode.Unauthorized, Request));
            }
            catch (UnknownCasePlanException)
            {
                return Content(HttpStatusCode.NotFound, this.ToError(new Dictionary<string, string>
                {
                    { "bad_request", "case definition doesn't exist" }
                }, HttpStatusCode.NotFound, Request));
            }
        }

        [HttpPost]
        [Route]
        public async Task<IHttpActionResult> Create([FromBody] CreateCaseInstanceCommand createCaseInstance)
        {
            try
            {
                var result = await _casePlanInstanceService.Create(createCaseInstance);
                return Ok(result);
            }
            catch (UnknownCasePlanException)
            {
                return Content(HttpStatusCode.NotFound, this.ToError(new Dictionary<string, string>
                {
                    { "bad_request", "case definition doesn't exist" }
                }, HttpStatusCode.NotFound, Request));
            }
        }

        [HttpGet]
        [Route("me/{id}/launch")]
        public async Task<IHttpActionResult> LaunchMe(string id)
        {
            try
            {
                await _casePlanInstanceService.LaunchMe(new LaunchCaseInstanceCommand { Performer = this.GetNameIdentifier(), CasePlanInstanceId = id });
                return Ok();
            }
            catch (UnauthorizedCaseWorkerException)
            {
                return Content(HttpStatusCode.Unauthorized, this.ToError(new Dictionary<string, string>
                {
                    { "unauthorized_request", "you're not authorized to launch the case instance" }
                }, HttpStatusCode.Unauthorized, Request));
            }
            catch (UnknownCasePlanInstanceException)
            {
                return Content(HttpStatusCode.NotFound, this.ToError(new Dictionary<string, string>
                {
                    { "bad_request", "case instance doesn't exist" }
                }, HttpStatusCode.NotFound, Request));
            }
        }

        [HttpGet]
        [Route("{id}/launch")]
        public async Task<IHttpActionResult> Launch(string id)
        {
            try
            {
                await _casePlanInstanceService.Launch(new LaunchCaseInstanceCommand { CasePlanInstanceId = id });
                return Ok();
            }
            catch (UnknownCasePlanInstanceException)
            {
                return Content(HttpStatusCode.NotFound, this.ToError(new Dictionary<string, string>
                {
                    { "bad_request", "case instance doesn't exist" }
                }, HttpStatusCode.NotFound, Request));
            }
        }


        [HttpGet]
        [Route("me/{id}/suspend")]
        public async Task<IHttpActionResult> SuspendMe(string id)
        {
            try
            {
                await _casePlanInstanceService.SuspendMe(new SuspendCommand(id, null) { Performer = this.GetNameIdentifier() });
                return Ok();
            }
            catch (UnknownCasePlanInstanceException)
            {
                return Content(HttpStatusCode.NotFound, this.ToError(new Dictionary<string, string>
                {
                    { "bad_request", "case instance doesn't exist" }
                }, HttpStatusCode.NotFound, Request));
            }
            catch (UnknownCasePlanInstanceElementException)
            {
                return Content(HttpStatusCode.NotFound, this.ToError(new Dictionary<string, string>
                {
                    { "bad_request", "case instance element doesn't exist" }
                }, HttpStatusCode.NotFound, Request));
            }
            catch (AggregateValidationException ex)
            {
                return Content(HttpStatusCode.BadRequest, this.ToError(ex.Errors, HttpStatusCode.BadRequest, Request));
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, this.ToError(new Dictionary<string, string>
                {
                    { "invalid_request", ex.Message }
                }, HttpStatusCode.BadRequest, Request));
            }
        }

        [HttpGet]
        [Route("{id}/suspend")]
        public async Task<IHttpActionResult> Suspend(string id)
        {
            try
            {
                await _casePlanInstanceService.Suspend(new SuspendCommand(id, null));
                return Ok();
            }
            catch (UnknownCasePlanInstanceException)
            {
                return Content(HttpStatusCode.NotFound, this.ToError(new Dictionary<string, string>
                {
                    { "bad_request", "case instance doesn't exist" }
                }, HttpStatusCode.NotFound, Request));
            }
            catch (UnknownCasePlanInstanceElementException)
            {
                return Content(HttpStatusCode.NotFound, this.ToError(new Dictionary<string, string>
                {
                    { "bad_request", "case instance element doesn't exist" }
                }, HttpStatusCode.NotFound, Request));
            }
            catch (AggregateValidationException ex)
            {
                return Content(HttpStatusCode.BadRequest, this.ToError(ex.Errors, HttpStatusCode.BadRequest, Request));
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, this.ToError(new Dictionary<string, string>
                {
                    { "invalid_request", ex.Message }
                }, HttpStatusCode.BadRequest, Request));
            }
        }

        [HttpGet]
        [Route("me/{id}/suspend/{elt}")]
        public async Task<IHttpActionResult> SuspendMe(string id, string elt)
        {
            try
            {
                await _casePlanInstanceService.SuspendMe(new SuspendCommand(id, elt) { Performer = this.GetNameIdentifier() });
                return Ok();
            }
            catch (UnknownCasePlanInstanceException)
            {
                return Content(HttpStatusCode.NotFound, this.ToError(new Dictionary<string, string>
                {
                    { "bad_request", "case instance doesn't exist" }
                }, HttpStatusCode.NotFound, Request));
            }
            catch (UnknownCasePlanInstanceElementException)
            {
                return Content(HttpStatusCode.NotFound, this.ToError(new Dictionary<string, string>
                {
                    { "bad_request", "case instance element doesn't exist" }
                }, HttpStatusCode.NotFound, Request));
            }
            catch (AggregateValidationException ex)
            {
                return Content(HttpStatusCode.BadRequest, this.ToError(ex.Errors, HttpStatusCode.BadRequest, Request));
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, this.ToError(new Dictionary<string, string>
                {
                    { "invalid_request", ex.Message }
                }, HttpStatusCode.BadRequest, Request));
            }
        }

        [HttpGet]
        [Route("{id}/suspend/{elt}")]
        public async Task<IHttpActionResult> Suspend(string id, string elt)
        {
            try
            {
                await _casePlanInstanceService.Suspend(new SuspendCommand(id, elt));
                return Ok();
            }
            catch (UnknownCasePlanInstanceException)
            {
                return Content(HttpStatusCode.NotFound, this.ToError(new Dictionary<string, string>
                {
                    { "bad_request", "case instance doesn't exist" }
                }, HttpStatusCode.NotFound, Request));
            }
            catch (UnknownCasePlanInstanceElementException)
            {
                return Content(HttpStatusCode.NotFound, this.ToError(new Dictionary<string, string>
                {
                    { "bad_request", "case instance element doesn't exist" }
                }, HttpStatusCode.NotFound, Request));
            }
            catch (AggregateValidationException ex)
            {
                return Content(HttpStatusCode.BadRequest, this.ToError(ex.Errors, HttpStatusCode.BadRequest, Request));
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, this.ToError(new Dictionary<string, string>
                {
                    { "invalid_request", ex.Message }
                }, HttpStatusCode.BadRequest, Request));
            }
        }

        [HttpGet]
        [Route("me/{id}/reactivate")]
        public async Task<IHttpActionResult> ReactivateMe(string id)
        {
            try
            {
                await _casePlanInstanceService.ReactivateMe(new ReactivateCommand(id, null) { Performer = this.GetNameIdentifier() });
                return Ok();
            }
            catch (UnknownCasePlanInstanceException)
            {
                return Content(HttpStatusCode.NotFound, this.ToError(new Dictionary<string, string>
                {
                    { "bad_request", "case instance doesn't exist" }
                }, HttpStatusCode.NotFound, Request));
            }
            catch (UnknownCasePlanInstanceElementException)
            {
                return Content(HttpStatusCode.NotFound, this.ToError(new Dictionary<string, string>
                {
                    { "bad_request", "case instance element doesn't exist" }
                }, HttpStatusCode.NotFound, Request));
            }
            catch (AggregateValidationException ex)
            {
                return Content(HttpStatusCode.BadRequest, this.ToError(ex.Errors, HttpStatusCode.BadRequest, Request));
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, this.ToError(new Dictionary<string, string>
                {
                    { "invalid_request", ex.Message }
                }, HttpStatusCode.BadRequest, Request));
            }
        }

        [HttpGet]
        [Route("{id}/reactivate")]
        public async Task<IHttpActionResult> Reactivate(string id)
        {
            try
            {
                await _casePlanInstanceService.Reactivate(new ReactivateCommand(id, null));
                return Ok();
            }
            catch (UnknownCasePlanInstanceException)
            {
                return Content(HttpStatusCode.NotFound, this.ToError(new Dictionary<string, string>
                {
                    { "bad_request", "case instance doesn't exist" }
                }, HttpStatusCode.NotFound, Request));
            }
            catch (UnknownCasePlanInstanceElementException)
            {
                return Content(HttpStatusCode.NotFound, this.ToError(new Dictionary<string, string>
                {
                    { "bad_request", "case instance element doesn't exist" }
                }, HttpStatusCode.NotFound, Request));
            }
            catch (AggregateValidationException ex)
            {
                return Content(HttpStatusCode.BadRequest, this.ToError(ex.Errors, HttpStatusCode.BadRequest, Request));
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, this.ToError(new Dictionary<string, string>
                {
                    { "invalid_request", ex.Message }
                }, HttpStatusCode.BadRequest, Request));
            }
        }

        [HttpGet]
        [Route("{id}/reactivate/{elt}")]
        public async Task<IHttpActionResult> Reactivate(string id, string elt)
        {
            try
            {
                await _casePlanInstanceService.Reactivate(new ReactivateCommand(id, elt));
                return Ok();
            }
            catch (UnknownCasePlanInstanceException)
            {
                return Content(HttpStatusCode.NotFound, this.ToError(new Dictionary<string, string>
                {
                    { "bad_request", "case instance doesn't exist" }
                }, HttpStatusCode.NotFound, Request));
            }
            catch (UnknownCasePlanInstanceElementException)
            {
                return Content(HttpStatusCode.NotFound, this.ToError(new Dictionary<string, string>
                {
                    { "bad_request", "case instance element doesn't exist" }
                }, HttpStatusCode.NotFound, Request));
            }
            catch (AggregateValidationException ex)
            {
                return Content(HttpStatusCode.BadRequest, this.ToError(ex.Errors, HttpStatusCode.BadRequest, Request));
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, this.ToError(new Dictionary<string, string>
                {
                    { "invalid_request", ex.Message }
                }, HttpStatusCode.BadRequest, Request));
            }
        }

        [HttpGet]
        [Route("me/{id}/resume")]
        public async Task<IHttpActionResult> ResumeMe(string id)
        {
            try
            {
                await _casePlanInstanceService.ResumeMe(new ResumeCommand(id, null) { Performer = this.GetNameIdentifier() });
                return Ok();
            }
            catch (UnknownCasePlanInstanceException)
            {
                return Content(HttpStatusCode.NotFound, this.ToError(new Dictionary<string, string>
                {
                    { "bad_request", "case instance doesn't exist" }
                }, HttpStatusCode.NotFound, Request));
            }
            catch (AggregateValidationException ex)
            {
                return Content(HttpStatusCode.BadRequest, this.ToError(ex.Errors, HttpStatusCode.BadRequest, Request));
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, this.ToError(new Dictionary<string, string>
                {
                    { "invalid_request", ex.Message }
                }, HttpStatusCode.BadRequest, Request));
            }
        }

        [HttpGet]
        [Route("{id}/resume")]
        public async Task<IHttpActionResult> Resume(string id)
        {
            try
            {
                await _casePlanInstanceService.Resume(new ResumeCommand(id, null));
                return Ok();
            }
            catch (UnknownCasePlanInstanceException)
            {
                return Content(HttpStatusCode.NotFound, this.ToError(new Dictionary<string, string>
                {
                    { "bad_request", "case instance doesn't exist" }
                }, HttpStatusCode.NotFound, Request));
            }
            catch (AggregateValidationException ex)
            {
                return Content(HttpStatusCode.BadRequest, this.ToError(ex.Errors, HttpStatusCode.BadRequest, Request));
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, this.ToError(new Dictionary<string, string>
                {
                    { "invalid_request", ex.Message }
                }, HttpStatusCode.BadRequest, Request));
            }
        }

        [HttpGet]
        [Route("me/{id}/resume/{elt}")]
        public async Task<IHttpActionResult> ResumeMe(string id, string elt)
        {
            try
            {
                await _casePlanInstanceService.ResumeMe(new ResumeCommand(id, elt) { Performer = this.GetNameIdentifier() });
                return Ok();
            }
            catch (UnknownCasePlanInstanceException)
            {
                return Content(HttpStatusCode.NotFound, this.ToError(new Dictionary<string, string>
                {
                    { "bad_request", "case instance doesn't exist" }
                }, HttpStatusCode.NotFound, Request));
            }
            catch (AggregateValidationException ex)
            {
                return Content(HttpStatusCode.BadRequest, this.ToError(ex.Errors, HttpStatusCode.BadRequest, Request));
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, this.ToError(new Dictionary<string, string>
                {
                    { "invalid_request", ex.Message }
                }, HttpStatusCode.BadRequest, Request));
            }
        }

        [HttpGet]
        [Route("{id}/resume/{elt}")]
        public async Task<IHttpActionResult> Resume(string id, string elt)
        {
            try
            {
                await _casePlanInstanceService.Resume(new ResumeCommand(id, elt));
                return Ok();
            }
            catch (UnknownCasePlanInstanceException)
            {
                return Content(HttpStatusCode.NotFound, this.ToError(new Dictionary<string, string>
                {
                    { "bad_request", "case instance doesn't exist" }
                }, HttpStatusCode.NotFound, Request));
            }
            catch (AggregateValidationException ex)
            {
                return Content(HttpStatusCode.BadRequest, this.ToError(ex.Errors, HttpStatusCode.BadRequest, Request));
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, this.ToError(new Dictionary<string, string>
                {
                    { "invalid_request", ex.Message }
                }, HttpStatusCode.BadRequest, Request));
            }
        }

        [HttpGet]
        [Route("me/{id}/terminate")]
        public async Task<IHttpActionResult> TerminateMe(string id)
        {
            try
            {
                await _casePlanInstanceService.TerminateMe(new TerminateCommand(id, null) { Performer = this.GetNameIdentifier() });
                return Ok();
            }
            catch (UnknownCasePlanInstanceException)
            {
                return Content(HttpStatusCode.NotFound, this.ToError(new Dictionary<string, string>
                {
                    { "bad_request", "case instance doesn't exist" }
                }, HttpStatusCode.NotFound, Request));
            }
            catch (UnknownCasePlanInstanceElementException)
            {
                return Content(HttpStatusCode.NotFound, this.ToError(new Dictionary<string, string>
                {
                    { "bad_request", "case instance element doesn't exist" }
                }, HttpStatusCode.NotFound, Request));
            }
            catch (AggregateValidationException ex)
            {
                return Content(HttpStatusCode.BadRequest, this.ToError(ex.Errors, HttpStatusCode.BadRequest, Request));
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, this.ToError(new Dictionary<string, string>
                {
                    { "invalid_request", ex.Message }
                }, HttpStatusCode.BadRequest, Request));
            }
        }

        [HttpGet]
        [Route("{id}/terminate")]
        public async Task<IHttpActionResult> Terminate(string id)
        {
            try
            {
                await _casePlanInstanceService.Terminate(new TerminateCommand(id, null));
                return Ok();
            }
            catch (UnknownCasePlanInstanceException)
            {
                return Content(HttpStatusCode.NotFound, this.ToError(new Dictionary<string, string>
                {
                    { "bad_request", "case instance doesn't exist" }
                }, HttpStatusCode.NotFound, Request));
            }
            catch (UnknownCasePlanInstanceElementException)
            {
                return Content(HttpStatusCode.NotFound, this.ToError(new Dictionary<string, string>
                {
                    { "bad_request", "case instance element doesn't exist" }
                }, HttpStatusCode.NotFound, Request));
            }
            catch (AggregateValidationException ex)
            {
                return Content(HttpStatusCode.BadRequest, this.ToError(ex.Errors, HttpStatusCode.BadRequest, Request));
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, this.ToError(new Dictionary<string, string>
                {
                    { "invalid_request", ex.Message }
                }, HttpStatusCode.BadRequest, Request));
            }
        }

        [HttpGet]
        [Route("me/{id}/terminate/{elt}")]
        public async Task<IHttpActionResult> TerminateMe(string id, string elt)
        {
            try
            {
                await _casePlanInstanceService.TerminateMe(new TerminateCommand(id, elt) { Performer = this.GetNameIdentifier() });
                return Ok();
            }
            catch (UnknownCasePlanInstanceException)
            {
                return Content(HttpStatusCode.NotFound, this.ToError(new Dictionary<string, string>
                {
                    { "bad_request", "case instance doesn't exist" }
                }, HttpStatusCode.NotFound, Request));
            }
            catch (UnknownCasePlanInstanceElementException)
            {
                return Content(HttpStatusCode.NotFound, this.ToError(new Dictionary<string, string>
                {
                    { "bad_request", "case instance element doesn't exist" }
                }, HttpStatusCode.NotFound, Request));
            }
            catch (AggregateValidationException ex)
            {
                return Content(HttpStatusCode.BadRequest, this.ToError(ex.Errors, HttpStatusCode.BadRequest, Request));
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, this.ToError(new Dictionary<string, string>
                {
                    { "invalid_request", ex.Message }
                }, HttpStatusCode.BadRequest, Request));
            }
        }

        [HttpGet]
        [Route("{id}/terminate/{elt}")]
        public async Task<IHttpActionResult> Terminate(string id, string elt)
        {
            try
            {
                await _casePlanInstanceService.Terminate(new TerminateCommand(id, elt));
                return Ok();
            }
            catch (UnknownCasePlanInstanceException)
            {
                return Content(HttpStatusCode.NotFound, this.ToError(new Dictionary<string, string>
                {
                    { "bad_request", "case instance doesn't exist" }
                }, HttpStatusCode.NotFound, Request));
            }
            catch (UnknownCasePlanInstanceElementException)
            {
                return Content(HttpStatusCode.NotFound, this.ToError(new Dictionary<string, string>
                {
                    { "bad_request", "case instance element doesn't exist" }
                }, HttpStatusCode.NotFound, Request));
            }
            catch (AggregateValidationException ex)
            {
                return Content(HttpStatusCode.BadRequest, this.ToError(ex.Errors, HttpStatusCode.BadRequest, Request));
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, this.ToError(new Dictionary<string, string>
                {
                    { "invalid_request", ex.Message }
                }, HttpStatusCode.BadRequest, Request));
            }
        }

        [HttpGet]
        [Route("me/{id}/close")]
        public async Task<IHttpActionResult> CloseMe(string id)
        {
            try
            {
                await _casePlanInstanceService.CloseMe(new CloseCommand(id) { Performer = this.GetNameIdentifier() });
                return Ok();
            }
            catch (UnknownCasePlanInstanceException)
            {
                return Content(HttpStatusCode.NotFound, this.ToError(new Dictionary<string, string>
                {
                    { "bad_request", "case instance doesn't exist" }
                }, HttpStatusCode.NotFound, Request));
            }
            catch (AggregateValidationException ex)
            {
                return Content(HttpStatusCode.BadRequest, this.ToError(ex.Errors, HttpStatusCode.BadRequest, Request));
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, this.ToError(new Dictionary<string, string>
                {
                    { "invalid_request", ex.Message }
                }, HttpStatusCode.BadRequest, Request));
            }
        }

        [HttpGet]
        [Route("{id}/close")]
        public async Task<IHttpActionResult> Close(string id)
        {
            try
            {
                await _casePlanInstanceService.Close(new CloseCommand(id));
                return Ok();
            }
            catch (UnknownCasePlanInstanceException)
            {
                return Content(HttpStatusCode.NotFound, this.ToError(new Dictionary<string, string>
                {
                    { "bad_request", "case instance doesn't exist" }
                }, HttpStatusCode.NotFound, Request));
            }
            catch (AggregateValidationException ex)
            {
                return Content(HttpStatusCode.BadRequest, this.ToError(ex.Errors, HttpStatusCode.BadRequest, Request));
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, this.ToError(new Dictionary<string, string>
                {
                    { "invalid_request", ex.Message }
                }, HttpStatusCode.BadRequest, Request));
            }
        }

        [HttpPost]
        [Route("me/{id}/confirm/{elt}")]
        public async Task<IHttpActionResult> ConfirmFormMe(string id, string elt, [FromBody] JObject jObj)
        {
            try
            {
                await _casePlanInstanceService.ConfirmFormMe(new ConfirmFormCommand { CasePlanInstanceId = id, CasePlanElementInstanceId = elt, Content = jObj, Performer = this.GetNameIdentifier() });
                return Ok();
            }
            catch (UnknownCasePlanInstanceException)
            {
                return Content(HttpStatusCode.NotFound, this.ToError(new Dictionary<string, string>
                {
                    { "bad_request", "case instance doesn't exist" }
                }, HttpStatusCode.NotFound, Request));
            }
            catch (UnknownCasePlanInstanceElementException)
            {
                return Content(HttpStatusCode.NotFound, this.ToError(new Dictionary<string, string>
                {
                    { "bad_request", "case instance element doesn't exist" }
                }, HttpStatusCode.NotFound, Request));
            }
            catch (AggregateValidationException ex)
            {
                return Content(HttpStatusCode.BadRequest, this.ToError(ex.Errors, HttpStatusCode.BadRequest, Request));
            }
            catch (UnauthorizedCaseWorkerException)
            {
                return Content(HttpStatusCode.Unauthorized, this.ToError(new Dictionary<string, string>
                {
                    { "unauthorized_request", "you're not authorized to confirm the human task" }
                }, HttpStatusCode.Unauthorized, Request));
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, this.ToError(new Dictionary<string, string>
                {
                    { "invalid_request", ex.Message }
                }, HttpStatusCode.BadRequest, Request));
            }
        }

        [HttpPost]
        [Route("{id}/confirm/{elt}")]
        public async Task<IHttpActionResult> ConfirmForm(string id, string elt, [FromBody] JObject jObj)
        {
            try
            {
                await _casePlanInstanceService.ConfirmForm(new ConfirmFormCommand { CasePlanInstanceId = id, CasePlanElementInstanceId = elt, Content = jObj });
                return Ok();
            }
            catch (UnknownCasePlanInstanceException)
            {
                return Content(HttpStatusCode.NotFound, this.ToError(new Dictionary<string, string>
                {
                    { "bad_request", "case instance doesn't exist" }
                }, HttpStatusCode.NotFound, Request));
            }
            catch (UnknownCasePlanInstanceElementException)
            {
                return Content(HttpStatusCode.NotFound, this.ToError(new Dictionary<string, string>
                {
                    { "bad_request", "case instance element doesn't exist" }
                }, HttpStatusCode.NotFound, Request));
            }
            catch (AggregateValidationException ex)
            {
                return Content(HttpStatusCode.BadRequest, this.ToError(ex.Errors, HttpStatusCode.BadRequest, Request));
            }
            catch (UnauthorizedCaseWorkerException)
            {
                return Content(HttpStatusCode.Unauthorized, this.ToError(new Dictionary<string, string>
                {
                    { "unauthorized_request", "you're not authorized to confirm the human task" }
                }, HttpStatusCode.Unauthorized, Request));
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, this.ToError(new Dictionary<string, string>
                {
                    { "invalid_request", ex.Message }
                }, HttpStatusCode.BadRequest, Request));
            }
        }

        [HttpGet]
        [Route("me/{id}/activate/{elt}")]
        public async Task<IHttpActionResult> ActivateMe(string id, string elt)
        {
            try
            {
                await _casePlanInstanceService.ActivateMe(new ActivateCommand(id, elt) { Performer = this.GetNameIdentifier() });
                return Ok();
            }
            catch (UnknownCasePlanInstanceException)
            {
                return Content(HttpStatusCode.NotFound, this.ToError(new Dictionary<string, string>
                {
                    { "bad_request", "case instance doesn't exist" }
                }, HttpStatusCode.NotFound, Request));
            }
            catch (UnknownCasePlanInstanceElementException)
            {
                return Content(HttpStatusCode.NotFound, this.ToError(new Dictionary<string, string>
                {
                    { "bad_request", "case instance element doesn't exist" }
                }, HttpStatusCode.NotFound, Request));
            }
            catch (AggregateValidationException ex)
            {
                return Content(HttpStatusCode.BadRequest, this.ToError(ex.Errors, HttpStatusCode.BadRequest, Request));
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, this.ToError(new Dictionary<string, string>
                {
                    { "invalid_request", ex.Message }
                }, HttpStatusCode.BadRequest, Request));
            }
        }

        [HttpGet]
        [Route("{id}/activate/{elt}")]
        public async Task<IHttpActionResult> Activate(string id, string elt)
        {
            try
            {
                await _casePlanInstanceService.Activate(new ActivateCommand(id, elt));
                return Ok();
            }
            catch (UnknownCasePlanInstanceException)
            {
                return Content(HttpStatusCode.NotFound, this.ToError(new Dictionary<string, string>
                {
                    { "bad_request", "case instance doesn't exist" }
                }, HttpStatusCode.NotFound, Request));
            }
            catch (UnknownCasePlanInstanceElementException)
            {
                return Content(HttpStatusCode.NotFound, this.ToError(new Dictionary<string, string>
                {
                    { "bad_request", "case instance element doesn't exist" }
                }, HttpStatusCode.NotFound, Request));
            }
            catch (AggregateValidationException ex)
            {
                return Content(HttpStatusCode.BadRequest, this.ToError(ex.Errors, HttpStatusCode.BadRequest, Request));
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, this.ToError(new Dictionary<string, string>
                {
                    { "invalid_request", ex.Message }
                }, HttpStatusCode.BadRequest, Request));
            }
        }
    }
}