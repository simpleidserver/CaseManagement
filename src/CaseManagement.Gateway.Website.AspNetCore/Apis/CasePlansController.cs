using CaseManagement.Gateway.Website.AspNetCore.Extensions;
using CaseManagement.Gateway.Website.CasePlanInstance.DTOs;
using CaseManagement.Gateway.Website.CasePlans.CommandHandlers;
using CaseManagement.Gateway.Website.CasePlans.Commands;
using CaseManagement.Gateway.Website.CasePlans.DTOs;
using CaseManagement.Gateway.Website.CasePlans.Queries;
using CaseManagement.Gateway.Website.CasePlans.QueryHandlers;
using CaseManagement.Gateway.Website.CaseWorkerTask.DTOs;
using CaseManagement.Gateway.Website.FormInstance.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Threading.Tasks;

namespace CaseManagement.Gateway.Website.AspNetCore.Apis
{
    [Route(ServerConstants.RouteNames.CasePlans)]
    public class CasePlansController : Controller
    {
        private readonly ISearchMyLatestCasePlanQueryHandler _searchMyLatestCasePlanQueryHandler;
        private readonly IGetCasePlanQueryHandler _getCasePlanQueryHandler;
        private readonly ILaunchCasePlanInstanceCommandHandler _launchCasePlanInstanceCommandHandler;
        private readonly ISearchCasePlanInstanceQueryHandler _searchCasePlanInstanceQueryHandler;
        private readonly ISearchFormInstanceQueryHandler _searchFormInstanceQueryHandler;
        private readonly ISearchCaseWorkerTaskQueryHandler _searchCaseWorkerTaskQueryHandler;
        private readonly ICloseCasePlanInstanceCommandHandler _closeCasePlanInstanceCommandHandler;
        private readonly IReactivateCasePlanInstanceCommandHandler _reactivateCasePlanInstanceCommandHandler;
        private readonly IResumeCasePlanInstanceCommandHandler _resumeCasePlanInstanceCommandHandler;
        private readonly ISuspendCasePlanInstanceCommandHandler _suspendCasePlanInstanceCommandHandler;
        private readonly ITerminateCasePlanInstanceCommandHandler _terminateCasePlanInstanceCommandHandler;
        private readonly ISearchCasePlanHistoryQueryHandler _searchCasePlanHistoryQueryHandler;

        public CasePlansController(ISearchMyLatestCasePlanQueryHandler searchMyLatestCasePlanQueryHandler, IGetCasePlanQueryHandler getCasePlanQueryHandler, ILaunchCasePlanInstanceCommandHandler launchCasePlanInstanceCommandHandler, ISearchCasePlanInstanceQueryHandler searchCasePlanInstanceQueryHandler, ISearchFormInstanceQueryHandler searchFormInstanceQueryHandler, ISearchCaseWorkerTaskQueryHandler searchCaseWorkerTaskQueryHandler, ICloseCasePlanInstanceCommandHandler closeCasePlanInstanceCommandHandler, IReactivateCasePlanInstanceCommandHandler reactivateCasePlanInstanceCommandHandler, IResumeCasePlanInstanceCommandHandler resumeCasePlanInstanceCommandHandler, ISuspendCasePlanInstanceCommandHandler suspendCasePlanInstanceCommandHandler, ITerminateCasePlanInstanceCommandHandler terminateCasePlanInstanceCommandHandler, ISearchCasePlanHistoryQueryHandler searchCasePlanHistoryQueryHandler)
        {
            _searchMyLatestCasePlanQueryHandler = searchMyLatestCasePlanQueryHandler;
            _getCasePlanQueryHandler = getCasePlanQueryHandler;
            _launchCasePlanInstanceCommandHandler = launchCasePlanInstanceCommandHandler;
            _searchCasePlanInstanceQueryHandler = searchCasePlanInstanceQueryHandler;
            _searchFormInstanceQueryHandler = searchFormInstanceQueryHandler;
            _searchCaseWorkerTaskQueryHandler = searchCaseWorkerTaskQueryHandler;
            _closeCasePlanInstanceCommandHandler = closeCasePlanInstanceCommandHandler;
            _reactivateCasePlanInstanceCommandHandler = reactivateCasePlanInstanceCommandHandler;
            _resumeCasePlanInstanceCommandHandler = resumeCasePlanInstanceCommandHandler;
            _suspendCasePlanInstanceCommandHandler = suspendCasePlanInstanceCommandHandler;
            _terminateCasePlanInstanceCommandHandler = terminateCasePlanInstanceCommandHandler;
            _searchCasePlanHistoryQueryHandler = searchCasePlanHistoryQueryHandler;
        }

        [HttpGet("me/search")]
        [Authorize("get_caseplan")]
        public async Task<IActionResult> SearchMyLatest()
        {
            var query = Request.Query.ToEnumerable();
            var nameIdentifier = this.GetNameIdentifier();
            var result = await _searchMyLatestCasePlanQueryHandler.Handle(new SearchMyLatestCasePlanQuery { Queries = query, NameIdentifier = nameIdentifier });
            return new OkObjectResult(ToDto(result));
        }

        [HttpGet("{id}/history/search")]
        [Authorize("get_caseplan")]
        public async Task<IActionResult> SearchHistory()
        {
            var query = Request.Query.ToEnumerable();
            var nameIdentifier = this.GetNameIdentifier();
            var result = await _searchCasePlanHistoryQueryHandler.Handle(new SearchCasePlanHistoryQuery { Queries = query, NameIdentifier = nameIdentifier });
            return new OkObjectResult(ToDto(result));
        }

        [HttpGet("{id}")]
        [Authorize("get_caseplan")]
        public async Task<IActionResult> Get(string id)
        {
            var result = await _getCasePlanQueryHandler.Handle(id, this.GetIdentityToken());
            return new OkObjectResult(ToDto(result));
        }

        [HttpGet("{id}/caseplaninstances/search")]
        [Authorize("get_caseplan")]
        public async Task<IActionResult> SearchCasePlanInstances(string id)
        {
            var query = Request.Query.ToEnumerable();
            var nameIdentifier = this.GetNameIdentifier();
            var result = await _searchCasePlanInstanceQueryHandler.Handle(new SearchCasePlanInstanceQuery { CasePlanId = id, Queries = query, Owner = nameIdentifier });
            return new OkObjectResult(ToDto(result));
        }

        [HttpGet("{id}/forminstances/search")]
        [Authorize("get_caseplan")]
        public async Task<IActionResult> SearchCaseFormInstances(string id)
        {
            var query = Request.Query.ToEnumerable();
            var result = await _searchFormInstanceQueryHandler.Handle(new SearchFormInstanceQuery { CasePlanId = id, Queries = query });
            return new OkObjectResult(ToDto(result));
        }

        [HttpGet("{id}/caseworkertasks/search")]
        [Authorize("get_caseplan")]
        public async Task<IActionResult> SearchCaseWorkerTasks(string id)
        {
            var query = Request.Query.ToEnumerable();
            var result = await _searchCaseWorkerTaskQueryHandler.Handle(new SearchCaseWorkerTaskQuery { CasePlanId = id, Queries = query });
            return new OkObjectResult(ToDto(result));
        }

        [HttpGet("{id}/caseplaninstances/launch")]
        [Authorize("launch_caseplaninstance")]
        public async Task<IActionResult> Launch(string id)
        {
            var result = await _launchCasePlanInstanceCommandHandler.Handle(id, this.GetIdentityToken());
            return new OkObjectResult(ToDto(result));
        }

        [HttpGet("{id}/caseplaninstances/{casePlanInstanceId}/close")]
        [Authorize("close_caseplaninstance")]
        public async Task<IActionResult> Close(string id, string casePlanInstanceId)
        {
            await _closeCasePlanInstanceCommandHandler.Handle(new CloseCasePlanInstanceCommand(casePlanInstanceId, this.GetIdentityToken()));
            return new OkResult();
        }

        [HttpGet("{id}/caseplaninstances/{casePlanInstanceId}/reactivate")]
        [Authorize("reactivate_caseplaninstance")]
        public async Task<IActionResult> Reactivate(string id, string casePlanInstanceId)
        {
            await _reactivateCasePlanInstanceCommandHandler.Handle(new ReactivateCasePlanInstanceCommand(casePlanInstanceId, this.GetIdentityToken()));
            return new OkResult();
        }

        [HttpGet("{id}/caseplaninstances/{casePlanInstanceId}/resume")]
        [Authorize("resume_caseplaninstance")]
        public async Task<IActionResult> Resume(string id, string casePlanInstanceId)
        {
            await _resumeCasePlanInstanceCommandHandler.Handle(new ResumeCasePlanInstanceCommand(casePlanInstanceId, this.GetIdentityToken()));
            return new OkResult();
        }

        [HttpGet("{id}/caseplaninstances/{casePlanInstanceId}/suspend")]
        [Authorize("suspend_caseplaninstance")]
        public async Task<IActionResult> Suspend(string id, string casePlanInstanceId)
        {
            await _suspendCasePlanInstanceCommandHandler.Handle(new SuspendCasePlanInstanceCommand(casePlanInstanceId, this.GetIdentityToken()));
            return new OkResult();
        }

        [HttpGet("{id}/caseplaninstances/{casePlanInstanceId}/terminate")]
        [Authorize("terminate_caseplaninstance")]
        public async Task<IActionResult> Terminate(string id, string casePlanInstanceId)
        {
            await _terminateCasePlanInstanceCommandHandler.Handle(new TerminateCasePlanInstanceCommand(casePlanInstanceId, this.GetIdentityToken()));
            return new OkResult();
        }

        public static JObject ToDto(FindResponse<CasePlanResponse> resp)
        {
            return new JObject
            {
                { "start_index", resp.StartIndex },
                { "total_length", resp.TotalLength },
                { "count", resp.Count },
                { "content", new JArray(resp.Content.Select(r => ToDto(r))) }
            };
        }

        public static JObject ToDto(FindResponse<CasePlanInstanceResponse> resp)
        {
            return new JObject
            {
                { "start_index", resp.StartIndex },
                { "total_length", resp.TotalLength },
                { "count", resp.Count },
                { "content", new JArray(resp.Content.Select(r => ToDto(r))) }
            };
        }

        public static JObject ToDto(FindResponse<FormInstanceResponse> resp)
        {
            return new JObject
            {
                { "start_index", resp.StartIndex },
                { "total_length", resp.TotalLength },
                { "count", resp.Count },
                { "content", new JArray(resp.Content.Select(r => {
                    var result = new JObject
                    {
                        { "id", r.Id },
                        { "create_datetime", r.CreateDateTime },
                        { "update_datetime", r.UpdateDateTime },
                        { "performer", r.PerformerRole },
                        { "case_plan_id", r.CasePlanId },
                        { "case_plan_instance_id", r.CasePlanInstanceId },
                        { "case_plan_element_instance_id", r.CaseElementInstanceId },
                        { "form_id", r.FormId },
                        { "status", r.Status }
                    };
                    var content = new JArray();
                    foreach(var formElt in r.Content)
                    {
                        var record = new JObject
                        {
                            { "form_element_id", formElt.FormElementId },
                            { "value", formElt.Value }
                        };
                        content.Add(record);
                    }

                    result.Add("content", content);
                    return result;
                })) }
            };
        }

        public static JObject ToDto(FindResponse<CaseWorkerTaskResponse> resp)
        {
            return new JObject
            {
                { "start_index", resp.StartIndex },
                { "total_length", resp.TotalLength },
                { "count", resp.Count },
                { "content", new JArray(resp.Content.Select(r => {
                    var result = new JObject
                    {
                        { "case_plan_id", r.CasePlanId },
                        { "case_plan_instance_id", r.CasePlanInstanceId },
                        { "case_plan_element_instance_id", r.CasePlanElementInstanceId},
                        { "create_datetime", r.CreateDateTime },
                        { "update_datetime", r.UpdateDateTime },
                        { "performer", r.PerformerRole },
                        { "type", r.TaskType },
                        { "status", r.Status }
                    };
                    return result;
                })) }
            };
        }

        public static JObject ToDto(CasePlanInstanceResponse casePlanInstance)
        {
            var result = new JObject
            {
                { "id", casePlanInstance.Id },
                { "name", casePlanInstance.Name },
                { "create_datetime", casePlanInstance.CreateDateTime},
                { "case_plan_id", casePlanInstance.CasePlanId },
                { "context", casePlanInstance.Context },
                { "state", casePlanInstance.State }
            };
            var stateHistories = new JArray();
            var transitionHistories = new JArray();
            var executionHistories = new JArray();
            var elts = new JArray();
            foreach (var stateHistory in casePlanInstance.StateHistories)
            {
                stateHistories.Add(new JObject
                {
                    { "state", stateHistory.State },
                    { "datetime", stateHistory.UpdateDateTime }
                });
            }

            foreach (var transitionHistory in casePlanInstance.TransitionHistories)
            {
                transitionHistories.Add(new JObject
                {
                    { "transition", transitionHistory.Transition },
                    { "datetime", transitionHistory.CreateDateTime }
                });
            }

            foreach (var elt in casePlanInstance.Elements)
            {
                elts.Add(ToDto(elt));
            }

            result.Add("state_histories", stateHistories);
            result.Add("transition_histories", transitionHistories);
            result.Add("execution_histories", executionHistories);
            result.Add("elements", elts);
            return result;
        }

        public static JObject ToDto(CasePlanElementInstanceResponse elt)
        {
            var result = new JObject
            {
                { "id", elt.Id },
                { "name", elt.Name },
                { "type", elt.Type },
                { "version", elt.Version },
                { "create_datetime", elt.CreateDateTime},
                { "case_plan_element_id", elt.CasePlanElementId },
                { "state", elt.State }
            };
            var stateHistories = new JArray();
            var transitionHistories = new JArray();
            foreach (var stateHistory in elt.StateHistories)
            {
                stateHistories.Add(new JObject
                {
                    { "state", stateHistory.State },
                    { "datetime", stateHistory.UpdateDateTime }
                });
            }

            foreach (var transitionHistory in elt.TransitionHistories)
            {
                transitionHistories.Add(new JObject
                {
                    { "transition", transitionHistory.Transition },
                    { "datetime", transitionHistory.CreateDateTime }
                });
            }

            result.Add("state_histories", stateHistories);
            result.Add("transition_histories", transitionHistories);
            return result;
        }

        public static JObject ToDto(CasePlanResponse def)
        {
            return new JObject
            {
                { "id", def.Id },
                { "name", def.Name },
                { "description", def.Description },
                { "case_file", def.CaseFileId },
                { "create_datetime", def.CreateDateTime },
                { "version", def.Version },
                { "owner", def.Owner }
            };
        }
    }
}
