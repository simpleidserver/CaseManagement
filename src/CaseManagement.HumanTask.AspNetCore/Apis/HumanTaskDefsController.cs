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

        [HttpPost("{id}/callbackoperations")]
        public async Task<IActionResult> AddCallbackOperation(string id, [FromBody] AddCallbackOperationCommand parameter, CancellationToken token)
        {
            try
            {
                parameter.HumanTaskDefId = id;
                var result = await _mediator.Send(parameter, token);
                return new CreatedResult(string.Empty, new { id = result });
            }
            catch (BadRequestException ex)
            {
                return this.ToError(new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("bad_request", ex.Message)
                }, HttpStatusCode.BadRequest, Request);
            }
            catch (UnknownHumanTaskDefException ex)
            {
                return this.ToError(new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("bad_request", ex.Message)
                }, HttpStatusCode.NotFound, Request);
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

        [HttpPut("{id}/presentationelts")]
        public async Task<IActionResult> UpdatePresentationElements(string id, [FromBody] UpdateHumanTaskDefPresentationParametersCommand parameter, CancellationToken token)
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

        [HttpPost("{id}/deadlines/start")]
        public async Task<IActionResult> AddStartDeadline(string id, [FromBody] AddHumanTaskDefStartDeadLineCommand cmd, CancellationToken token)
        {
            try
            {
                cmd.Id = id;
                var newId = await _mediator.Send(cmd, token);
                return new ObjectResult(new { id = newId })
                {
                    StatusCode = (int)HttpStatusCode.Created
                };
            }
            catch (BadRequestException ex)
            {
                return this.ToError(new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("bad_request", ex.Message)
                }, HttpStatusCode.BadRequest, Request);
            }
            catch (UnknownHumanTaskDefException ex)
            {
                return this.ToError(new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("bad_request", ex.Message)
                }, HttpStatusCode.NotFound, Request);
            }
            catch(AggregateValidationException ex)
            {
                return this.ToError(ex.Errors, HttpStatusCode.BadRequest, Request);
            }
        }

        [HttpPost("{id}/deadlines/completion")]
        public async Task<IActionResult> AddCompletionDeadline(string id, [FromBody] AddHumanTaskDefCompletionDeadLineCommand cmd, CancellationToken token)
        {
            try
            {
                cmd.Id = id;
                var newId = await _mediator.Send(cmd, token);
                return new ObjectResult(new { id = newId })
                {
                    StatusCode = (int)HttpStatusCode.Created
                };
            }
            catch (BadRequestException ex)
            {
                return this.ToError(new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("bad_request", ex.Message)
                }, HttpStatusCode.BadRequest, Request);
            }
            catch (UnknownHumanTaskDefException ex)
            {
                return this.ToError(new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("bad_request", ex.Message)
                }, HttpStatusCode.NotFound, Request);
            }
            catch (AggregateValidationException ex)
            {
                return this.ToError(ex.Errors, HttpStatusCode.BadRequest, Request);
            }
        }

        [HttpDelete("{id}/deadlines/start/{deadLineId}")]
        public async Task<IActionResult> DeleteStartDeadline(string id, string deadLineId, CancellationToken token)
        {
            try
            {
                var newId = await _mediator.Send(new DeleteHumanTaskDefStartDeadlineCommand { DeadLineId = deadLineId, Id = id }, token);
                return new NoContentResult();
            }
            catch (UnknownHumanTaskDefException ex)
            {
                return this.ToError(new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("bad_request", ex.Message)
                }, HttpStatusCode.NotFound, Request);
            }
            catch (AggregateValidationException ex)
            {
                return this.ToError(ex.Errors, HttpStatusCode.BadRequest, Request);
            }
        }

        [HttpDelete("{id}/deadlines/completion/{deadLineId}")]
        public async Task<IActionResult> DeleteCompletionDeadline(string id, string deadLineId, CancellationToken token)
        {
            try
            {
                var newId = await _mediator.Send(new DeleteHumanTaskDefCompletionDeadlineCommand { DeadLineId = deadLineId, Id = id }, token);
                return new NoContentResult();
            }
            catch (UnknownHumanTaskDefException ex)
            {
                return this.ToError(new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("bad_request", ex.Message)
                }, HttpStatusCode.NotFound, Request);
            }
            catch (AggregateValidationException ex)
            {
                return this.ToError(ex.Errors, HttpStatusCode.BadRequest, Request);
            }
        }

        [HttpPut("{id}/deadlines/start/{deadLineId}")]
        public async Task<IActionResult> UpdateStartDeadline(string id, string deadLineId, [FromBody] UpdateHumanTaskDefStartDeadlineCommand updateHumanTaskDefStartDeadlineCommand, CancellationToken token)
        {
            try
            {
                updateHumanTaskDefStartDeadlineCommand.Id = id;
                updateHumanTaskDefStartDeadlineCommand.DeadLineId = deadLineId;
                var newId = await _mediator.Send(updateHumanTaskDefStartDeadlineCommand, token);
                return new NoContentResult();
            }
            catch (BadRequestException ex)
            {
                return this.ToError(new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("bad_request", ex.Message)
                }, HttpStatusCode.BadRequest, Request);
            }
            catch (UnknownHumanTaskDefException ex)
            {
                return this.ToError(new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("bad_request", ex.Message)
                }, HttpStatusCode.NotFound, Request);
            }
            catch (AggregateValidationException ex)
            {
                return this.ToError(ex.Errors, HttpStatusCode.BadRequest, Request);
            }
        }

        [HttpPut("{id}/deadlines/completion/{deadLineId}")]
        public async Task<IActionResult> UpdateCompletionDeadline(string id, string deadLineId, [FromBody] UpdateHumanTaskDefCompletionDeadlineCommand updateHumanTaskDefCompletionDeadlineCommand, CancellationToken token)
        {
            try
            {
                updateHumanTaskDefCompletionDeadlineCommand.Id = id;
                updateHumanTaskDefCompletionDeadlineCommand.DeadLineId = deadLineId;
                var newId = await _mediator.Send(updateHumanTaskDefCompletionDeadlineCommand, token);
                return new NoContentResult();
            }
            catch (BadRequestException ex)
            {
                return this.ToError(new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("bad_request", ex.Message)
                }, HttpStatusCode.BadRequest, Request);
            }
            catch (UnknownHumanTaskDefException ex)
            {
                return this.ToError(new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("bad_request", ex.Message)
                }, HttpStatusCode.NotFound, Request);
            }
            catch (AggregateValidationException ex)
            {
                return this.ToError(ex.Errors, HttpStatusCode.BadRequest, Request);
            }
        }

        [HttpPost("{id}/deadlines/start/{deadLineId}/escalations")]
        public async Task<IActionResult> AddStartDeadlineEscalation(string id, string deadLineId, [FromBody] AddHumanTaskDefEscalationStartDeadlineCommand addHumanTaskDefEscalationStartDeadlineCommand, CancellationToken token)
        {
            try
            {
                addHumanTaskDefEscalationStartDeadlineCommand.Id = id;
                addHumanTaskDefEscalationStartDeadlineCommand.StartDeadlineId = deadLineId;
                var newId = await _mediator.Send(addHumanTaskDefEscalationStartDeadlineCommand, token);
                return new ObjectResult(new { id = newId })
                {
                    StatusCode = (int)HttpStatusCode.Created
                };
            }
            catch (BadRequestException ex)
            {
                return this.ToError(new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("bad_request", ex.Message)
                }, HttpStatusCode.BadRequest, Request);
            }
            catch (UnknownHumanTaskDefException ex)
            {
                return this.ToError(new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("bad_request", ex.Message)
                }, HttpStatusCode.NotFound, Request);
            }
            catch (AggregateValidationException ex)
            {
                return this.ToError(ex.Errors, HttpStatusCode.BadRequest, Request);
            }
        }

        [HttpPost("{id}/deadlines/completion/{deadLineId}/escalations")]
        public async Task<IActionResult> AddCompletionDeadlineEscalation(string id, string deadLineId, [FromBody] AddHumanTaskDefEscalationCompletionDeadlineCommand addHumanTaskDefEscalationCompletionDeadlineCommand, CancellationToken token)
        {
            try
            {
                addHumanTaskDefEscalationCompletionDeadlineCommand.Id = id;
                addHumanTaskDefEscalationCompletionDeadlineCommand.CompletionDeadlineId = deadLineId;
                var newId = await _mediator.Send(addHumanTaskDefEscalationCompletionDeadlineCommand, token);
                return new ObjectResult(new { id = newId })
                {
                    StatusCode = (int)HttpStatusCode.Created
                };
            }
            catch (BadRequestException ex)
            {
                return this.ToError(new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("bad_request", ex.Message)
                }, HttpStatusCode.BadRequest, Request);
            }
            catch (UnknownHumanTaskDefException ex)
            {
                return this.ToError(new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("bad_request", ex.Message)
                }, HttpStatusCode.NotFound, Request);
            }
            catch (AggregateValidationException ex)
            {
                return this.ToError(ex.Errors, HttpStatusCode.BadRequest, Request);
            }
        }

        [HttpDelete("{id}/deadlines/start/{deadLineId}/escalations/{escalationId}")]
        public async Task<IActionResult> DeleteStartDeadlineEscalation(string id, string deadLineId, string escalationId, CancellationToken token)
        {
            try
            {
                await _mediator.Send(new DeleteHumanTaskDefEscalationStartDeadlineCommand { Id = id, DeadLineId = deadLineId, EscalationId = escalationId }, token);
                return new NoContentResult();
            }
            catch (UnknownHumanTaskDefException ex)
            {
                return this.ToError(new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("bad_request", ex.Message)
                }, HttpStatusCode.NotFound, Request);
            }
            catch (AggregateValidationException ex)
            {
                return this.ToError(ex.Errors, HttpStatusCode.BadRequest, Request);
            }
        }

        [HttpDelete("{id}/deadlines/completion/{deadLineId}/escalations/{escalationId}")]
        public async Task<IActionResult> DeleteCompletionDeadlineEscalation(string id, string deadLineId, string escalationId, CancellationToken token)
        {
            try
            {
                await _mediator.Send(new DeleteHumanTaskDefEscalationCompletionDeadlineCommand { Id = id, DeadLineId = deadLineId, EscalationId = escalationId }, token);
                return new NoContentResult();
            }
            catch (UnknownHumanTaskDefException ex)
            {
                return this.ToError(new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("bad_request", ex.Message)
                }, HttpStatusCode.NotFound, Request);
            }
            catch (AggregateValidationException ex)
            {
                return this.ToError(ex.Errors, HttpStatusCode.BadRequest, Request);
            }
        }

        [HttpPut("{id}/deadlines/start/{deadLineId}/escalations/{escalationId}")]
        public async Task<IActionResult> UpdateStartDeadlineEscalation(string id, string deadLineId, string escalationId, [FromBody] UpdateHumanTaskDefEscalationStartDeadlineCommand cmd, CancellationToken token)
        {
            try
            {
                cmd.Id = id;
                cmd.DeadLineId = deadLineId;
                cmd.EscalationId = escalationId;
                await _mediator.Send(cmd, token);
                return new NoContentResult();
            }
            catch (BadRequestException ex)
            {
                return this.ToError(new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("bad_request", ex.Message)
                }, HttpStatusCode.BadRequest, Request);
            }
            catch (UnknownHumanTaskDefException ex)
            {
                return this.ToError(new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("bad_request", ex.Message)
                }, HttpStatusCode.NotFound, Request);
            }
            catch (AggregateValidationException ex)
            {
                return this.ToError(ex.Errors, HttpStatusCode.BadRequest, Request);
            }
        }

        [HttpPut("{id}/deadlines/completion/{deadLineId}/escalations/{escalationId}")]
        public async Task<IActionResult> UpdateCompletionDeadlineEscalation(string id, string deadLineId, string escalationId, [FromBody] UpdateHumanTaskDefEscalationCompletionDeadlineCommand cmd, CancellationToken token)
        {
            try
            {
                cmd.Id = id;
                cmd.DeadLineId = deadLineId;
                cmd.EscalationId = escalationId;
                await _mediator.Send(cmd, token);
                return new NoContentResult();
            }
            catch (BadRequestException ex)
            {
                return this.ToError(new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("bad_request", ex.Message)
                }, HttpStatusCode.BadRequest, Request);
            }
            catch (UnknownHumanTaskDefException ex)
            {
                return this.ToError(new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("bad_request", ex.Message)
                }, HttpStatusCode.NotFound, Request);
            }
            catch (AggregateValidationException ex)
            {
                return this.ToError(ex.Errors, HttpStatusCode.BadRequest, Request);
            }
        }

        [HttpPut("{id}/rendering")]
        public async Task<IActionResult> UpdateRendering(string id, [FromBody] UpdateHumanTaskDefRenderingCommand cmd, CancellationToken token)
        {
            try
            {
                cmd.Id = id;
                await _mediator.Send(cmd, token);
                return new NoContentResult();
            }
            catch (BadRequestException ex)
            {
                return this.ToError(new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("bad_request", ex.Message)
                }, HttpStatusCode.BadRequest, Request);
            }
            catch (UnknownHumanTaskDefException ex)
            {
                return this.ToError(new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("bad_request", ex.Message)
                }, HttpStatusCode.NotFound, Request);
            }
        }
    }
}