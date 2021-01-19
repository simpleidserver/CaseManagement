using CaseManagement.Common.Exceptions;
using CaseManagement.HumanTask.Domains;
using CaseManagement.HumanTask.Exceptions;
using CaseManagement.HumanTask.NotificationDef.Commands;
using CaseManagement.HumanTask.NotificationDef.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.HumanTask.AspNetCore.Apis
{
    [Route(HumanTaskConstants.RouteNames.NotificationDefs)]
    public class NotificationDefsController : Controller
    {
        private readonly IMediator _mediator;

        public NotificationDefsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] AddNotificationDefCommand parameter, CancellationToken token)
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
        public async Task<IActionResult> Search([FromBody] SearchNotificationDefQuery parameter, CancellationToken token)
        {
            var result = await _mediator.Send(parameter, token);
            return new OkObjectResult(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id, CancellationToken token)
        {
            try
            {
                var result = await _mediator.Send(new GetNotificationDefQuery { Id = id }, token);
                return new OkObjectResult(result);
            }
            catch (UnknownNotificationDefException ex)
            {
                return this.ToError(new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("bad_request", ex.Message)
                }, HttpStatusCode.NotFound, Request);
            }
        }

        [HttpPut("{id}/info")]
        public async Task<IActionResult> UpdateInfo(string id, [FromBody] UpdateNotificationDefInfoCommand cmd, CancellationToken token)
        {
            try
            {
                cmd.Id = id;
                var result = await _mediator.Send(cmd, token);
                return new NoContentResult();
            }
            catch (UnknownNotificationDefException ex)
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

        [HttpPost("{id}/assignments")]
        public async Task<IActionResult> AddPeopleAssignment(string id, [FromBody] AddNotificationDefPeopleAssignmentCommand cmd, CancellationToken token)
        {
            try
            {
                cmd.Id = id;
                var result = await _mediator.Send(cmd, token);
                return new OkObjectResult(result);
            }
            catch (UnknownNotificationDefException ex)
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

        [HttpDelete("{id}/assignments/{assignmentId}")]
        public async Task<IActionResult> DeletePeopleAssignment(string id, string assignmentId, CancellationToken token)
        {
            try
            {
                var cmd = new DeleteNotificationDefPeopleAssignmentCommand { Id = id, AssignmentId = assignmentId };
                await _mediator.Send(cmd, token);
                return new NoContentResult();
            }
            catch (UnknownNotificationDefException ex)
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

        [HttpPost("{id}/presentationelts")]
        public async Task<IActionResult> AddPresentationElement(string id, [FromBody] AddNotificationDefPresentationElementCommand parameter, CancellationToken token)
        {
            try
            {
                parameter.Id = id;
                await _mediator.Send(parameter, token);
                return new NoContentResult();
            }
            catch (UnknownNotificationDefException ex)
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


        [HttpDelete("{id}/presentationelts/{usage}/{language}")]
        public async Task<IActionResult> AddPresentationElement(string id, PresentationElementUsages usage, string language, CancellationToken token)
        {
            try
            {
                var cmd = new DeleteNotificationDefPresentationElementCommand { Id = id, Usage = usage, Language = language };
                await _mediator.Send(cmd, token);
                return new NoContentResult();
            }
            catch (UnknownNotificationDefException ex)
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

        [HttpPost("{id}/presentationparameters")]
        public async Task<IActionResult> AddPresentationParameter(string id, [FromBody] AddNotificationDefPresentationParameterCommand parameter, CancellationToken token)
        {
            try
            {
                parameter.Id = id;
                await _mediator.Send(parameter, token);
                return new NoContentResult();
            }
            catch (UnknownNotificationDefException ex)
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

        [HttpDelete("{id}/presentationparameters/{name}")]
        public async Task<IActionResult> DeletePresentationParameter(string id, string name, CancellationToken token)
        {
            try
            {
                var parameter = new DeleteNotificationDefPresentationParameterCommand { Id = id, Name = name };
                await _mediator.Send(parameter, token);
                return new NoContentResult();
            }
            catch (UnknownNotificationDefException ex)
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
