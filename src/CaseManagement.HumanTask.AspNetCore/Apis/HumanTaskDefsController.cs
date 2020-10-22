using CaseManagement.Common.Exceptions;
using CaseManagement.HumanTask.Exceptions;
using CaseManagement.HumanTask.HumanTaskDef.Commands;
using CaseManagement.HumanTask.HumanTaskDef.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.HumanTask.AspNetCore.Apis
{
    [Route(HumanTaskConstants.RouteNames.HumanTaskDefs)]
    public class HumanTaskDefsController : Controller
    {
        private readonly IMediator _mediator;

        public HumanTaskDefsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] AddHumanTaskDefCommand parameter, CancellationToken token)
        {
            try
            {
                var result = await _mediator.Send(parameter, token);
                return new CreatedResult(string.Empty, result);
            }
            catch (BadRequestException ex)
            {
                return this.ToError(new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("bad_request", ex.Message)
                }, HttpStatusCode.BadRequest, Request);
            }
        }

        [HttpPost(".search")]
        public async Task<IActionResult> Search([FromBody] SearchHumanTaskDefQuery parameter, CancellationToken token)
        {
            var result = await _mediator.Send(parameter, token);
            return new OkObjectResult(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id, CancellationToken token)
        {
            try
            {
                var result = await _mediator.Send(new GetHumanTaskDefQuery { Id = id }, token);
                return new OkObjectResult(result);
            }
            catch (UnknownHumanTaskDefException ex)
            {
                return this.ToError(new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("bad_request", ex.Message)
                }, HttpStatusCode.NotFound, Request);
            }
        }

        [HttpPut("{id}/info")]
        public async Task<IActionResult> UpdateInfo(string id, [FromBody] UpdateHumanTaskDefInfoCommand cmd, CancellationToken token)
        {
            try
            {
                cmd.Id = id;
                var result = await _mediator.Send(cmd, token);
                return new NoContentResult();
            }
            catch (UnknownHumanTaskDefException ex)
            {
                return this.ToError(new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("bad_request", ex.Message)
                }, HttpStatusCode.NotFound, Request);
            }
            catch (BadRequestException ex)
            {
                return this.ToError(new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("bad_request", ex.Message)
                }, HttpStatusCode.BadRequest, Request);
            }
        }

        [HttpPut("{id}/assignment")]
        public async Task<IActionResult> UpdatePeopleAssignment(string id, [FromBody] UpdateHumanTaskDefPeopleAssignmentCommand cmd, CancellationToken token)
        {
            try
            {
                cmd.Id = id;
                var result = await _mediator.Send(cmd, token);
                return new NoContentResult();
            }
            catch (UnknownHumanTaskDefException ex)
            {
                return this.ToError(new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("bad_request", ex.Message)
                }, HttpStatusCode.NotFound, Request);
            }
        }

        [HttpPost("{id}/parameters/input")]
        public async Task<IActionResult> AddInputParameter(string id, [FromBody] AddHumanTaskDefInputParameterCommand parameter, CancellationToken token)
        {
            try
            {
                parameter.Id = id;
                await _mediator.Send(parameter, token);
                return new NoContentResult();
            }
            catch (UnknownHumanTaskDefException ex)
            {
                return this.ToError(new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("bad_request", ex.Message)
                }, HttpStatusCode.NotFound, Request);
            }
            catch (BadRequestException ex)
            {
                return this.ToError(new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("bad_request", ex.Message)
                }, HttpStatusCode.BadRequest, Request);
            }
        }

        [HttpPost("{id}/parameters/output")]
        public async Task<IActionResult> AddOutputParameter(string id, [FromBody] AddHumanTaskDefOutputParameterCommand parameter, CancellationToken token)
        {
            try
            {
                parameter.Id = id;
                await _mediator.Send(parameter, token);
                return new NoContentResult();
            }
            catch (UnknownHumanTaskDefException ex)
            {
                return this.ToError(new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("bad_request", ex.Message)
                }, HttpStatusCode.NotFound, Request);
            }
            catch (BadRequestException ex)
            {
                return this.ToError(new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("bad_request", ex.Message)
                }, HttpStatusCode.BadRequest, Request);
            }
        }

        [HttpDelete("{id}/parameters/input/{name}")]
        public async Task<IActionResult> DeleteInputParameter(string id, string name, CancellationToken token)
        {
            try
            {
                var cmd = new DeleteHumanTaskDefInputParameterCommand { Id = id, ParameterName = name };
                await _mediator.Send(cmd, token);
                return new NoContentResult();
            }
            catch (UnknownHumanTaskDefException ex)
            {
                return this.ToError(new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("bad_request", ex.Message)
                }, HttpStatusCode.NotFound, Request);
            }
            catch (BadRequestException ex)
            {
                return this.ToError(new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("bad_request", ex.Message)
                }, HttpStatusCode.BadRequest, Request);
            }
        }

        [HttpDelete("{id}/parameters/output/{name}")]
        public async Task<IActionResult> DeleteOutputParameter(string id, string name, CancellationToken token)
        {
            try
            {
                var cmd = new DeleteHumanTaskDefOutputParameterCommand { Id = id, ParameterName = name };
                await _mediator.Send(cmd, token);
                return new NoContentResult();
            }
            catch (UnknownHumanTaskDefException ex)
            {
                return this.ToError(new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("bad_request", ex.Message)
                }, HttpStatusCode.NotFound, Request);
            }
            catch (BadRequestException ex)
            {
                return this.ToError(new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("bad_request", ex.Message)
                }, HttpStatusCode.BadRequest, Request);
            }
        }
    }
}