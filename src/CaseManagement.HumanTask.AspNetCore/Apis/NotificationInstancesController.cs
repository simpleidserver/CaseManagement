using CaseManagement.HumanTask.NotificationInstance.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.HumanTask.AspNetCore.Apis
{
    [Route(HumanTaskConstants.RouteNames.NotificationInstances)]
    public class NotificationInstancesController : Controller
    {
        private readonly IMediator _mediator;

        public NotificationInstancesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        #region Getters

        [HttpPost("search")]
        [Authorize("Authenticated")]
        public async Task<IActionResult> Search([FromBody] GetNotificationsDetailsQuery parameter, CancellationToken token)
        {
            parameter.Claims = User.GetClaims();
            var result = await _mediator.Send(parameter, token);
            return new OkObjectResult(result);
        }

        #endregion
    }
}
