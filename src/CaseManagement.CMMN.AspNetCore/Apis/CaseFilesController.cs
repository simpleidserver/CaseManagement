using CaseManagement.CMMN.AspNetCore.Extensions;
using CaseManagement.CMMN.CaseFile.Commands;
using CaseManagement.CMMN.CaseFile.Exceptions;
using CaseManagement.CMMN.CaseFile.Queries;
using CaseManagement.Common.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.AspNetCore.Apis
{
    [Route(CMMNConstants.RouteNames.CaseFiles)]
    public class CaseFilesController : Controller
    {
        private readonly IMediator _mediator;

        public CaseFilesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("count")]
        public async Task<IActionResult> Count(CancellationToken token)
        {
            var result = await _mediator.Send(new CountCaseFileQuery(), token);
            return new OkObjectResult(result);
        }

        [HttpPost]
        [Authorize("add_casefile")]
        public async Task<IActionResult> Add([FromBody] AddCaseFileCommand parameter, CancellationToken token)
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

        [HttpPut("{id}")]
        [Authorize("update_casefile")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateCaseFileCommand parameter, CancellationToken token)
        {
            try
            {
                parameter.Id = id;
                await _mediator.Send(parameter, token);
                return new OkResult();
            }
            catch (UnknownCaseFileException)
            {
                return new NotFoundResult();
            }
            catch (AggregateValidationException ex)
            {
                return this.ToError(ex.Errors, HttpStatusCode.BadRequest, Request);
            }
        }

        [HttpGet("{id}/publish")]
        [Authorize("publish_casefile")]
        public async Task<IActionResult> Publish(string id, CancellationToken token)
        {
            try
            {
                var cmd = new PublishCaseFileCommand
                {
                    Id = id
                };
                var result = await _mediator.Send(cmd, token);
                return new OkObjectResult(result);
            }
            catch (UnknownCaseFileException)
            {
                return new NotFoundResult();
            }
            catch (AggregateValidationException ex)
            {
                return this.ToError(ex.Errors, HttpStatusCode.BadRequest, Request);
            }
        }

        [HttpPost("search")]
        [Authorize("get_casefile")]
        public async Task<IActionResult> Search([FromBody] SearchCaseFileQuery query, CancellationToken token)
        {
            var result = await _mediator.Send(query, token);
            return new OkObjectResult(result);
        }

        [HttpGet("{id}")]
        [Authorize("get_casefile")]
        public async Task<IActionResult> Get(string id, CancellationToken token)
        {
            try
            {
                var result = await _mediator.Send(new GetCaseFileQuery { Id = id }, token);
                return new OkObjectResult(result);
            }
            catch (UnknownCaseFileException)
            {
                return new NotFoundResult();
            }
        }
    }
}