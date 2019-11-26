using CaseManagement.CMMN.CaseInstance.CommandHandlers;
using CaseManagement.CMMN.CaseInstance.Commands;
using CaseManagement.CMMN.Domains;
using CaseManagement.Workflow.Domains;
using CaseManagement.Workflow.Persistence;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Apis
{
    [Route(CMMNConstants.RouteNames.CaseInstances)]
    public class CaseInstancesController : Controller
    {
        private readonly ICreateCaseInstanceCommandHandler _createCaseInstanceCommandHandler;
        private readonly ILaunchCaseInstanceCommandHandler _launchCaseInstanceCommandHandler;
        private readonly IConfirmFormCommandHandler _confirmFormCommandHandler;
        private readonly IProcessFlowInstanceQueryRepository _processFlowInstanceQueryRepository;

        public CaseInstancesController(ICreateCaseInstanceCommandHandler createCaseInstanceCommandHandler, ILaunchCaseInstanceCommandHandler launchCaseInstanceCommandHandler, IConfirmFormCommandHandler confirmFormCommandHandler, IProcessFlowInstanceQueryRepository processFlowInstanceQueryRepository)
        {
            _createCaseInstanceCommandHandler = createCaseInstanceCommandHandler;
            _launchCaseInstanceCommandHandler = launchCaseInstanceCommandHandler;
            _confirmFormCommandHandler = confirmFormCommandHandler;
            _processFlowInstanceQueryRepository = processFlowInstanceQueryRepository;
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
            return new OkResult();
        }

        [HttpPost("{id}/confirm/{elt}")]
        public async Task<IActionResult> ConfirmForm(string id, string elt, [FromBody] ConfirmFormCommand confirmForm)
        {
            await _confirmFormCommandHandler.Handle(new ConfirmFormCommand { CaseInstanceId = id, CaseElementInstanceId = elt });
            return new OkResult();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var flowInstance =  await _processFlowInstanceQueryRepository.FindFlowInstanceById(id);
            if (flowInstance == null)
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(ToDto(flowInstance));
        }

        private static JObject ToDto(ProcessFlowInstance flowInstance)
        {
            var result = new JObject
            {
                { "id", flowInstance.Id },
                { "create_datetime", flowInstance.CreateDateTime },
                { "status", Enum.GetName(typeof(ProcessFlowInstanceStatus), flowInstance.Status).ToLowerInvariant() }
            };
            var planItems = new JArray();
            foreach(var planItem in flowInstance.Elements.Where(e => e is CMMNPlanItem).Cast<CMMNPlanItem>())
            {
                planItems.Add(ToDto(planItem));
            }

            result.Add("items", planItems);
            return result;
        }

        private static JObject ToDto(CMMNPlanItem planItem)
        {
            return new JObject
            {
                { "id", planItem.Id },
                { "name", planItem.Name },
                { "status", Enum.GetName(typeof(ProcessFlowInstanceElementStatus), planItem.Status).ToLowerInvariant() }
            };
        }
    }
}