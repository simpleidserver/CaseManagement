using CaseManagement.Common.Exceptions;
using CaseManagement.HumanTask.Domains;
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

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken token)
        {
            var result = await _mediator.Send(new GetAllHumanTaskDefQuery(), token);
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

        [HttpPost("{id}/assignments")]
        public async Task<IActionResult> AddPeopleAssignment(string id, [FromBody] AddHumanTaskDefPeopleAssignmentCommand cmd, CancellationToken token)
        {
            try
            {
                cmd.Id = id;
                var result = await _mediator.Send(cmd, token);
                return new OkObjectResult(result);
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

        [HttpDelete("{id}/assignments/{assignmentId}")]
        public async Task<IActionResult> DeletePeopleAssignment(string id, string assignmentId, CancellationToken token)
        {
            try
            {
                var cmd = new DeleteHumanTaskDefPeopleAssignmentCommand { Id = id, AssignmentId = assignmentId };
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

        [HttpPost("{id}/parameters")]
        public async Task<IActionResult> AddParameter(string id, [FromBody] AddHumanTaskDefParameterCommand parameter, CancellationToken token)
        {
            try
            {
                parameter.Id = id;
                var result = await _mediator.Send(parameter, token);
                return new OkObjectResult(result);
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
            catch (AggregateValidationException ex)
            {
                return this.ToError(ex.Errors, HttpStatusCode.BadRequest, Request);
            }
        }

        [HttpDelete("{id}/parameters/{parameterId}")]
        public async Task<IActionResult> DeleteParameter(string id, string parameterId, CancellationToken token)
        {
            try
            {
                var cmd = new DeleteHumanTaskDefParameterCommand { Id = id, ParameterId = parameterId };
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
            catch (AggregateValidationException ex)
            {
                return this.ToError(ex.Errors, HttpStatusCode.BadRequest, Request);
            }
        }

        [HttpPost("{id}/presentationelts")]
        public async Task<IActionResult> AddPresentationElement(string id, [FromBody] AddHumanTaskDefPresentationElementCommand parameter, CancellationToken token)
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


        [HttpDelete("{id}/presentationelts/{usage}/{language}")]
        public async Task<IActionResult> AddPresentationElement(string id, PresentationElementUsages usage, string language, CancellationToken token)
        {
            try
            {
                var cmd = new DeleteHumanTaskDefPresentationElementCommand { Id = id, Usage = usage, Language = language };
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

        [HttpPost("{id}/presentationparameters")]
        public async Task<IActionResult> AddPresentationParameter(string id, [FromBody] AddHumanTaskDefPresentationParameterCommand parameter, CancellationToken token)
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

        [HttpDelete("{id}/presentationparameters/{name}")]
        public async Task<IActionResult> DeletePresentationParameter(string id, string name, CancellationToken token)
        {
            try
            {
                var parameter = new DeleteHumanTaskDefPresentationParameterCommand { Id = id, Name = name };
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

        [HttpPost("{id}/deadlines")]
        public async Task<IActionResult> AddDeadline(string id, [FromBody] AddHumanTaskDefDeadLineCommand cmd, CancellationToken token)
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

        [HttpDelete("{id}/deadlines/{deadLineId}")]
        public async Task<IActionResult> DeleteDeadline(string id, string deadLineId, CancellationToken token)
        {
            try
            {
                var newId = await _mediator.Send(new DeleteHumanTaskDefDeadlineCommand { DeadLineId = deadLineId, Id = id }, token);
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

        [HttpPut("{id}/deadlines/{deadLineId}")]
        public async Task<IActionResult> UpdateDeadline(string id, string deadLineId, [FromBody] UpdateHumanTaskDefDeadlineCommand updateHumanTaskDefStartDeadlineCommand, CancellationToken token)
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

        [HttpPost("{id}/deadlines/{deadLineId}/escalations")]
        public async Task<IActionResult> AddDeadlineEscalation(string id, string deadLineId, [FromBody] AddHumanTaskDefEscalationDeadlineCommand addHumanTaskDefEscalationStartDeadlineCommand, CancellationToken token)
        {
            try
            {
                addHumanTaskDefEscalationStartDeadlineCommand.Id = id;
                addHumanTaskDefEscalationStartDeadlineCommand.DeadlineId = deadLineId;
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

        [HttpPut("{id}/deadlines/{deadLineId}/escalations/{escalationId}")]
        public async Task<IActionResult> UpdateDeadlineEscalation(string id, string deadLineId, string escalationId, [FromBody] UpdateHumanTaskDefEscalationDeadlineCommand updateHumanTaskDefEscalationStartDeadlineCommand, CancellationToken token)
        {
            try
            {
                updateHumanTaskDefEscalationStartDeadlineCommand.Id = id;
                updateHumanTaskDefEscalationStartDeadlineCommand.DeadlineId = deadLineId;
                updateHumanTaskDefEscalationStartDeadlineCommand.EscalationId = escalationId;
                var newId = await _mediator.Send(updateHumanTaskDefEscalationStartDeadlineCommand, token);
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

        [HttpDelete("{id}/deadlines/{deadLineId}/escalations/{escalationId}")]
        public async Task<IActionResult> DeleteStartDeadlineEscalation(string id, string deadLineId, string escalationId, CancellationToken token)
        {
            try
            {
                await _mediator.Send(new DeleteHumanTaskDefEscalationDeadlineCommand { Id = id, DeadLineId = deadLineId, EscalationId = escalationId }, token);
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

        [HttpPost("{id}/deadlines/{deadLineId}/escalations/{escalationId}/toparts")]
        public async Task<IActionResult> AddEscalationToPart(string id, string deadLineId, string escalationId, [FromBody] AddHumanTaskDefToPartCommand addHumanTaskDefToPartCommand, CancellationToken token)
        {
            try
            {
                addHumanTaskDefToPartCommand.Id = id;
                addHumanTaskDefToPartCommand.DeadlineId = deadLineId;
                addHumanTaskDefToPartCommand.EscalationId = escalationId;
                await _mediator.Send(addHumanTaskDefToPartCommand, token);
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

        [HttpDelete("{id}/deadlines/{deadLineId}/escalations/{escalationId}/toparts/{toPartName}")]
        public async Task<IActionResult> DeleteEscalationToPart(string id, string deadLineId, string escalationId, string toPartName, CancellationToken token)
        {
            try
            {
                var cmd = new DeleteHumanTaskDefToPartCommand { Id = id, DeadlineId = deadLineId, EscalationId = escalationId, ToPartName = toPartName };
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