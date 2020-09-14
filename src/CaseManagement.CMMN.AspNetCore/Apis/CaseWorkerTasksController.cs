using CaseManagement.CMMN.CaseWorkerTask.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.AspNetCore.Apis
{
    [Route(CMMNConstants.RouteNames.CaseWorkerTasks)]
    public class CaseWorkerTasksController : Controller
    {
        private readonly IMediator _mediator;

        public CaseWorkerTasksController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("search")]
        [Authorize("get_caseworkertasks")]
        public async Task<IActionResult> Search([FromBody] SearchCaseWorkerTaskQuery query, CancellationToken token)
        {
            var result = await _mediator.Send(query, token);
            return new OkObjectResult(result);
        }
    }
}