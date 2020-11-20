using CaseManagement.BPMN.Exceptions;
using CaseManagement.BPMN.ProcessFile.Queries;
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
    }
}
