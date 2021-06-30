using CaseManagement.BPMN.DelegateConfiguration.Commands;
using CaseManagement.BPMN.DelegateConfiguration.Queries;
using CaseManagement.BPMN.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.BPMN.AspNetCore.Apis
{
    [Route(BPMNConstants.RouteNames.DelegateConfigurations)]
    public class DelegateConfigurationsController : Controller
    {
        private readonly IMediator _mediator;

        public DelegateConfigurationsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("search")]
        public async Task<IActionResult> Search([FromBody] SearchDelegateConfigurationQuery query, CancellationToken token)
        {
            var result = await _mediator.Send(query, token);
            return new OkObjectResult(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id, CancellationToken token)
        {
            try
            {
                var result = await _mediator.Send(new GetDelegateConfigurationQuery { Id = id }, token);
                return new OkObjectResult(result);
            }
            catch(UnknownDelegateConfigurationException ex)
            {
                return this.ToError(new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("bad_request", ex.Message)
                }, HttpStatusCode.NotFound, Request);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken token)
        {
            var result = await _mediator.Send(new GetAllDelegatesConfigurationsQuery(), token);
            return new OkObjectResult(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateDelegateConfigurationCommand cmd, CancellationToken token)
        {
            try
            {
                cmd.Id = id;
                await _mediator.Send(cmd, token);
                return new NoContentResult();
            }
            catch(UnknownDelegateConfigurationException ex)
            {
                return this.ToError(new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("bad_request", ex.Message)
                }, HttpStatusCode.NotFound, Request);
            }
        }
    }
}
