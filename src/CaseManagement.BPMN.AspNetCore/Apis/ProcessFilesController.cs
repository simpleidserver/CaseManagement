using CaseManagement.BPMN.ProcessFile.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
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
    }
}
