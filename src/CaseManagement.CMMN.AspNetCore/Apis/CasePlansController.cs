using CaseManagement.CMMN.CasePlan.Queries;
using CaseManagement.CMMN.Common.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.AspNetCore.Apis
{
    [Route(CMMNConstants.RouteNames.CasePlans)]
    public class CasePlansController : Controller
    {
        private readonly IMediator _mediator;

        public CasePlansController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("count")]
        public async Task<IActionResult> Count(CancellationToken token)
        {
            var result = await _mediator.Send(new CountCasePlanQuery(), token);
            return new OkObjectResult(result);
        }

        [HttpGet("{id}")]
        [Authorize("get_caseplan")]
        public async Task<IActionResult> Get(string id, CancellationToken token)
        {
            try
            {
                var result = await _mediator.Send(new GetCasePlanQuery { Id = id }, token);
                return new OkObjectResult(result);
            }
            catch (UnknownCasePlanException)
            {
                return new NotFoundResult();
            }
        }

        [HttpPost("search")]
        [Authorize("get_caseplan")]
        public async Task<IActionResult> Search([FromBody] SearchCasePlanQuery query, CancellationToken token)
        {
            var result = await _mediator.Send(query, token);
            return new OkObjectResult(result);
        }
    }
}
