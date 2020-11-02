using CaseManagement.HumanTask.Exceptions;
using CaseManagement.HumanTask.NotificationInstance.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
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

        [HttpPost(".search")]
        [Authorize("Authenticated")]
        public async Task<IActionResult> Search([FromBody] SearchNotificationsQuery parameter, CancellationToken token)
        {
            parameter.Claims = User.GetClaims();
            var result = await _mediator.Send(parameter, token);
            return new OkObjectResult(result);
        }

        [HttpPost("{notificationId}")]
        [Authorize("Authenticated")]
        public async Task<IActionResult> Get(string notificationId, CancellationToken token)
        {
            try
            {
                var result = await _mediator.Send(new GetNotificationsDetailsQuery { NotificationId = notificationId }, token);
                return new OkObjectResult(result);
            }
            catch (UnknownNotificationException ex)
            {
                return this.ToError(new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("bad_request", ex.Message)
                }, HttpStatusCode.NotFound, Request);
            }
        }

        #endregion
    }
}
