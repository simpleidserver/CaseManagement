using CaseManagement.CMMN.CaseInstance.CommandHandlers;
using CaseManagement.CMMN.CaseInstance.Commands;
using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Extensions;
using CaseManagement.Workflow.Domains;
using CaseManagement.Workflow.Persistence;
using CaseManagement.Workflow.Persistence.Parameters;
using CaseManagement.Workflow.Persistence.Responses;
using Microsoft.AspNetCore.Http;
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

        [HttpGet(".search")]
        public async Task<IActionResult> Search()
        {
            var query = HttpContext.Request.Query;
            var result = await _processFlowInstanceQueryRepository.Find(ExtractFindParameter(query));
            return new OkObjectResult(ToDto(result));
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

        private static FindWorkflowInstanceParameter ExtractFindParameter(IQueryCollection query)
        {
            int startIndex;
            int count;
            string orderBy;
            string templateId;
            FindOrders findOrder;
            var parameter = new FindWorkflowInstanceParameter();
            if (query.TryGet("start_index", out startIndex))
            {
                parameter.StartIndex = startIndex;
            }

            if (query.TryGet("count", out count))
            {
                parameter.Count = count;
            }

            if (query.TryGet("order_by", out orderBy))
            {
                parameter.OrderBy = orderBy;
            }

            if (query.TryGet("order", out findOrder))
            {
                parameter.Order = findOrder;
            }

            if (query.TryGet("template_id", out templateId))
            {
                parameter.ProcessFlowTemplateId = templateId;
            }

            return parameter;
        }

        private static JObject ToDto(FindResponse<ProcessFlowInstance> resp)
        {
            return new JObject
            {
                { "start_index", resp.StartIndex },
                { "total_length", resp.TotalLength },
                { "count", resp.Count },
                { "content", new JArray(resp.Content.Select(r => new JObject
                {
                    { "id", r.Id },
                    { "name", r.ProcessFlowName },
                    { "status", Enum.GetName(typeof(ProcessFlowInstanceStatus), r.Status).ToLowerInvariant() },
                    { "template_id", r.ProcessFlowTemplateId },
                    { "create_datetime", r.CreateDateTime }
                })) }
            };
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