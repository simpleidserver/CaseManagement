using CaseManagement.BPMN.Exceptions;
using CaseManagement.BPMN.ProcessInstance.Commands;
using CaseManagement.BPMN.ProcessInstance.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.BPMN.AspNetCore.Apis
{
    [Route(BPMNConstants.RouteNames.ProcessInstances)]
    public class ProcessInstancesController : Controller
    {
        private readonly IMediator _mediator;

        public ProcessInstancesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateProcessInstanceCommand createProcessInstanceCommand, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _mediator.Send(createProcessInstanceCommand, cancellationToken);
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

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _mediator.Send(new GetProcessInstanceQuery { Id = id }, cancellationToken);
                return new OkObjectResult(result);
            }
            catch (UnknownFlowInstanceException ex)
            {
                return this.ToError(new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("bad_request", ex.Message)
                }, HttpStatusCode.NotFound, Request);
            }
        }

        [HttpPost("search")]
        public async Task<IActionResult> Search([FromBody] SearchProcessInstancesQuery searchProcessInstancesQuery, CancellationToken token)
        {
            var result = await _mediator.Send(searchProcessInstancesQuery, token);
            return new OkObjectResult(result);
        }

        [HttpGet("{id}/start")]
        [Authorize("Authenticated")]
        public async Task<IActionResult> Start(string id, CancellationToken cancellationToken)
        {
            try
            {
                var nameIdentifier = User.GetClaims().GetUserNameIdentifier();
                var cmd = new StartProcessInstanceCommand { ProcessInstanceId = id, NameIdentifier = ""};
                await _mediator.Send(cmd, cancellationToken);
                return new NoContentResult();
            }
            catch (UnknownFlowInstanceException ex)
            {
                return this.ToError(new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("bad_request", ex.Message)
                }, HttpStatusCode.NotFound, Request);
            }
        }

        [HttpPost("{id}/messages")]
        public async Task<IActionResult> ConsumeMessage(string id, [FromBody] ConsumeMessageInstanceCommand consumeMessageInstanceCommand, CancellationToken token)
        {
            try
            {
                consumeMessageInstanceCommand.FlowNodeInstanceId = id;
                await _mediator.Send(consumeMessageInstanceCommand, token);
                return new OkResult();
            }
            catch (UnknownFlowInstanceException ex)
            {
                return this.ToError(new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("bad_request", ex.Message)
                }, HttpStatusCode.NotFound, Request);
            }
        }

        [HttpPost("{id}/statetransitions")]
        public async Task<IActionResult> MakeTransition(string id, [FromBody] MakeStateTransitionCommand stateTransitionCommand, CancellationToken token)
        {
            try
            {
                stateTransitionCommand.FlowNodeInstanceId = id;
                await _mediator.Send(stateTransitionCommand, token);
                return new OkResult();
            }
            catch (UnknownFlowInstanceException ex)
            {
                return this.ToError(new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("bad_request", ex.Message)
                }, HttpStatusCode.NotFound, Request);
            }
        }

        [HttpPost("{id}/complete/{eltId}")]
        public async Task<IActionResult> Complete(string id, string eltId, [FromBody] MakeStateTransitionCommand stateTransitionCommand, CancellationToken token)
        {
            try
            {
                stateTransitionCommand.FlowNodeInstanceId = id;
                stateTransitionCommand.FlowNodeElementInstanceId = eltId;
                stateTransitionCommand.State = "COMPLETED";
                await _mediator.Send(stateTransitionCommand, token);
                return new OkResult();
            }
            catch (UnknownFlowInstanceException ex)
            {
                return this.ToError(new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("bad_request", ex.Message)
                }, HttpStatusCode.NotFound, Request);
            }
        }
    }
}