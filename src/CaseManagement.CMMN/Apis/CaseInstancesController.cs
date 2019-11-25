using CaseManagement.CMMN.CaseInstance.Commands;
using CaseManagement.CMMN.CaseInstance.Handlers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Apis
{
    [Route(CMMNConstants.RouteNames.CaseInstances)]
    public class CaseInstancesController : Controller
    {
        private readonly ICreateCaseInstanceCommandHandler _createCaseInstanceCommandHandler;
        private readonly ILaunchCaseInstanceCommandHandler _launchCaseInstanceCommandHandler;

        public CaseInstancesController(ICreateCaseInstanceCommandHandler createCaseInstanceCommandHandler, ILaunchCaseInstanceCommandHandler launchCaseInstanceCommandHandler)
        {
            _createCaseInstanceCommandHandler = createCaseInstanceCommandHandler;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCaseInstanceCommand createCaseInstance)
        {
            var result = await _createCaseInstanceCommandHandler.Handle(createCaseInstance);
            return new ContentResult
            {
                StatusCode = (int)HttpStatusCode.Created,
                Content = new JObject
                {
                    { "id", result }
                }.ToString()
            };
        }

        [HttpGet("{id}/launch")]
        public async Task<IActionResult> Launch(string id)
        {
            await _launchCaseInstanceCommandHandler.Handle(new LaunchCaseInstanceCommand { CaseInstanceId = id });
            return null;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            return null;
        }
    }
}
