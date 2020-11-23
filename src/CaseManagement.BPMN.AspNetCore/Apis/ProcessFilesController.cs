using CaseManagement.BPMN.Exceptions;
using CaseManagement.BPMN.ProcessFile.Commands;
using CaseManagement.BPMN.ProcessFile.Queries;
using CaseManagement.Common.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.BPMN.AspNetCore.Apis
{
    [Route(BPMNConstants.RouteNames.ProcessFiles)]
    public class ProcessFilesController : Controller
    {
        private readonly IMediator _mediator;

        public ProcessFilesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateProcessFileCommand parameter, CancellationToken token)
        {
            try
            {
                var result = await _mediator.Send(parameter, token);
                return new CreatedResult(string.Empty, result);
            }
            catch (AggregateValidationException ex)
            {
                return this.ToError(ex.Errors, HttpStatusCode.BadRequest, Request);
            }
        }

        [HttpPost("search")]
        public async Task<IActionResult> Search([FromBody] SearchProcessFilesQuery query, CancellationToken token)
        {
            var result = await _mediator.Send(query, token);
            return new OkObjectResult(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id, CancellationToken token)
        {
            try
            {
                var result = await _mediator.Send(new GetProcessFileQuery { Id = id }, token);
                return new OkObjectResult(result);
            }
            catch(UnknownProcessFileException ex)
            {
                return this.ToError(new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("bad_request", ex.Message)
                }, HttpStatusCode.NotFound, Request);
            }
        }

        [HttpGet("{id}/publish")]
        public async Task<IActionResult> Publish(string id, CancellationToken token)
        {
            try
            {
                var cmd = new PublishProcessFileCommand
                {
                    Id = id
                };
                var result = await _mediator.Send(cmd, token);
                return new OkObjectResult(result);
            }
            catch(UnknownProcessFileException ex)
            {
                return this.ToError(new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("bad_request", ex.Message)
                }, HttpStatusCode.NotFound, Request);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateProcessFileCommand parameter, CancellationToken token)
        {
            try
            {
                parameter.Id = id;
                await _mediator.Send(parameter, token);
                return new OkResult();
            }
            catch (UnknownProcessFileException ex)
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

        [HttpPut("{id}/payload")]
        public async Task<IActionResult> UpdatePayload(string id, [FromBody] UpdateProcessFilePayloadCommand parameter, CancellationToken token)
        {
            try
            {
                parameter.Id = id;
                await _mediator.Send(parameter, token);
                return new OkResult();
            }
            catch (UnknownProcessFileException ex)
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
    }
}
