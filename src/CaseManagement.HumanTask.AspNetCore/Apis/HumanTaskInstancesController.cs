using CaseManagement.HumanTask.Exceptions;
using CaseManagement.HumanTask.HumanTaskInstance.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.HumanTask.AspNetCore.Apis
{
    [Route(HumanTaskConstants.RouteNames.HumanTaskInstances)]
    public class HumanTaskInstancesController : Controller
    {
        private readonly IMediator _mediator;

        public HumanTaskInstancesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize("Authenticated")]
        public async Task<IActionResult> Add([FromBody] CreateHumanTaskInstanceCommand parameter, CancellationToken token)
        {
            try
            {
                parameter.Claims = User.GetClaims();
                var result = await _mediator.Send(parameter, token);
                return new CreatedResult(string.Empty, result);
            }
            catch(UnknownHumanTaskDefException ex)
            {
                return this.ToError(new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("bad_request", ex.Message)
                }, System.Net.HttpStatusCode.BadRequest, Request);
            }
        }

        [HttpPost("nominate")]
        [Authorize("Authenticated")]
        public async Task<IActionResult> Nominate([FromBody] NominateHumanTaskInstanceCommand parameter, CancellationToken token)
        {
            parameter.Claims = User.GetClaims();
            await _mediator.Send(parameter, token);
            return new NoContentResult();
        }

        [HttpPost("start")]
        [Authorize("Authenticated")]
        public async Task<IActionResult> Start([FromBody] StartHumanTaskInstanceCommand parameter, CancellationToken token)
        {
            await _mediator.Send(parameter, token);
            return new NoContentResult();
        }
    }
}
