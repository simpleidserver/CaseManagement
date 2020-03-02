using CaseManagement.CMMN.CasePlanInstance.CommandHandlers;
using CaseManagement.CMMN.CasePlanInstance.Commands;
using CaseManagement.CMMN.CasePlanInstance.Exceptions;
using CaseManagement.CMMN.CasePlanInstance.Repositories;
using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Extensions;
using CaseManagement.CMMN.Persistence;
using CaseManagement.CMMN.Persistence.Parameters;
using CaseManagement.CMMN.Persistence.Responses;
using CaseManagement.CMMN.Roles;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CasePlanInstance
{
    public class CasePlanInstanceService : ICasePlanInstanceService
    {
        private readonly ICreateCaseInstanceCommandHandler _createCaseInstanceCommandHandler;
        private readonly ILaunchCaseInstanceCommandHandler _launchCaseInstanceCommandHandler;
        private readonly ISuspendCommandHandler _suspendCommandHandler;
        private readonly IResumeCommandHandler _resumeCommandHandler;
        private readonly ITerminateCommandHandler _terminateCommandHandler;
        private readonly IReactivateCommandHandler _reactivateCommandHandler;
        private readonly ICloseCommandHandler _closeCommandHandler;
        private readonly IConfirmFormCommandHandler _confirmFormCommandHandler;
        private readonly IActivateCommandHandler _activateCommandHandler;
        private readonly ICasePlanInstanceQueryRepository _cmmnWorkflowInstanceQueryRepository;
        private readonly IRoleService _roleService;
        private readonly ICaseFileItemRepository _caseFileItemRepository;

        public CasePlanInstanceService(ICreateCaseInstanceCommandHandler createCaseInstanceCommandHandler, ILaunchCaseInstanceCommandHandler launchCaseInstanceCommandHandler, ISuspendCommandHandler suspendCommandHandler, IResumeCommandHandler resumeCommandHandler, ITerminateCommandHandler terminateCommandHandler, IReactivateCommandHandler reactivateCommandHandler, ICloseCommandHandler closeCommandHandler, IConfirmFormCommandHandler confirmFormCommandHandler, IActivateCommandHandler activateCommandHandler, ICasePlanInstanceQueryRepository cmmnWorkflowInstanceQueryRepository, ICaseFileItemRepository caseFileItemRepository, IRoleService roleService)
        {
            _createCaseInstanceCommandHandler = createCaseInstanceCommandHandler;
            _launchCaseInstanceCommandHandler = launchCaseInstanceCommandHandler;
            _suspendCommandHandler = suspendCommandHandler;
            _resumeCommandHandler = resumeCommandHandler;
            _terminateCommandHandler = terminateCommandHandler;
            _reactivateCommandHandler = reactivateCommandHandler;
            _closeCommandHandler = closeCommandHandler;
            _confirmFormCommandHandler = confirmFormCommandHandler;
            _activateCommandHandler = activateCommandHandler;
            _cmmnWorkflowInstanceQueryRepository = cmmnWorkflowInstanceQueryRepository;
            _roleService = roleService;
            _caseFileItemRepository = caseFileItemRepository;
        }

        public async Task<JObject> Search(IEnumerable<KeyValuePair<string, string>> query)
        {
            var result = await _cmmnWorkflowInstanceQueryRepository.Find(ExtractFindParameter(query));
            return ToDto(result);
        }

        public async Task<JObject> SearchMe(IEnumerable<KeyValuePair<string, string>> query, string nameIdentifier)
        {
            var parameter = ExtractFindParameter(query);
            var roles = await _roleService.FindRolesByUser(nameIdentifier);
            parameter.Roles = roles.Select(r => r.Id).ToList();
            var result = await _cmmnWorkflowInstanceQueryRepository.Find(parameter);
            return ToDto(result);
        }

        public async Task<JObject> GetMe(string id, string nameIdentifier)
        {
            var result = await _cmmnWorkflowInstanceQueryRepository.FindFlowInstanceById(id);
            if (result == null)
            {
                throw new UnknownCaseInstanceException(id);
            }

            if (result.CaseOwner != nameIdentifier)
            {
                var roles = await _roleService.FindRolesByUser(nameIdentifier);
                var roleIds = roles.Select(r => r.Id);
                if (!roleIds.Any(roleId => result.Roles.Contains(roleId)))
                {
                    throw new UnauthorizedCaseWorkerException(nameIdentifier, id, null);
                }
            }

            return ToDto(result);
        }

        public async Task<JObject> Get(string id)
        {
            var result = await _cmmnWorkflowInstanceQueryRepository.FindFlowInstanceById(id);
            if (result == null)
            {
                throw new UnknownCaseInstanceException(id);
            }

            return ToDto(result);
        }

        public async Task<JObject> GetCaseFileItems(string id)
        {
            var result = await _caseFileItemRepository.FindByCaseInstance(id);
            return ToDto(result);
        }

        public Task<JObject> CreateMe(CreateCaseInstanceCommand createCaseInstance)
        {
            createCaseInstance.BypassUserValidation = false;
            return InternalCreate(createCaseInstance);
        }

        public Task<JObject> Create(CreateCaseInstanceCommand createCaseInstance)
        {
            createCaseInstance.BypassUserValidation = true;
            return InternalCreate(createCaseInstance);
        }

        public Task LaunchMe(LaunchCaseInstanceCommand launchCaseInstanceCommand)
        {
            launchCaseInstanceCommand.BypassUserValidation = false;
            return InternalLaunch(launchCaseInstanceCommand);
        }

        public Task Launch(LaunchCaseInstanceCommand launchCaseInstanceCommand)
        {
            launchCaseInstanceCommand.BypassUserValidation = true;
            return InternalLaunch(launchCaseInstanceCommand);
        }

        public Task SuspendMe(SuspendCommand suspendCommand)
        {
            suspendCommand.BypassUserValidation = false;
            return InternalSuspend(suspendCommand);
        }

        public Task Suspend(SuspendCommand suspendCommand)
        {
            suspendCommand.BypassUserValidation = true;
            return InternalSuspend(suspendCommand);
        }

        public Task ReactivateMe(ReactivateCommand reactivateCommand)
        {
            reactivateCommand.BypassUserValidation = false;
            return InternalReactivate(reactivateCommand);
        }

        public Task Reactivate(ReactivateCommand reactivateCommand)
        {
            reactivateCommand.BypassUserValidation = true;
            return InternalReactivate(reactivateCommand);
        }

        public Task ResumeMe(ResumeCommand resumeCommand)
        {
            resumeCommand.BypassUserValidation = false;
            return InternalResume(resumeCommand);
        }

        public Task Resume(ResumeCommand resumeCommand)
        {
            resumeCommand.BypassUserValidation = true;
            return InternalResume(resumeCommand);
        }

        public Task TerminateMe(TerminateCommand terminateCommand)
        {
            terminateCommand.BypassUserValidation = false;
            return InternalTerminate(terminateCommand);
        }

        public Task Terminate(TerminateCommand terminateCommand)
        {
            terminateCommand.BypassUserValidation = true;
            return InternalTerminate(terminateCommand);
        }
        
        public Task CloseMe(CloseCommand closeCommand)
        {
            closeCommand.BypassUserValidation = false;
            return InternalClose(closeCommand);
        }

        public Task Close(CloseCommand closeCommand)
        {
            closeCommand.BypassUserValidation = true;
            return InternalClose(closeCommand);
        }

        public Task ConfirmFormMe(ConfirmFormCommand confirmFormCommand)
        {
            confirmFormCommand.BypassUserValidation = false;
            return InternalConfirmForm(confirmFormCommand);
        }

        public Task ConfirmForm(ConfirmFormCommand confirmFormCommand)
        {
            confirmFormCommand.BypassUserValidation = true;
            return InternalConfirmForm(confirmFormCommand);
        }

        public Task ActivateMe(ActivateCommand activateCommand)
        {
            activateCommand.BypassUserValidation = false;
            return InternalActivate(activateCommand);
        }

        public Task Activate(ActivateCommand activateCommand)
        {
            activateCommand.BypassUserValidation = true;
            return InternalActivate(activateCommand);
        }

        private async Task<JObject> InternalCreate(CreateCaseInstanceCommand createCaseInstance)
        {
            var result = await _createCaseInstanceCommandHandler.Handle(createCaseInstance);
            return ToDto(result);
        }

        private Task InternalLaunch(LaunchCaseInstanceCommand launchCaseInstanceCommand)
        {
            return _launchCaseInstanceCommandHandler.Handle(launchCaseInstanceCommand);
        }

        private Task InternalSuspend(SuspendCommand suspendCommand)
        {
            return _suspendCommandHandler.Handle(suspendCommand);
        }

        private Task InternalReactivate(ReactivateCommand cmd)
        {
            return _reactivateCommandHandler.Handle(cmd);
        }

        private Task InternalResume(ResumeCommand cmd)
        {
            return _resumeCommandHandler.Handle(cmd);
        }

        private Task InternalTerminate(TerminateCommand cmd)
        {
            return _terminateCommandHandler.Handle(cmd);
        }

        private Task InternalClose(CloseCommand cmd)
        {
            return _closeCommandHandler.Handle(cmd);
        }

        private Task InternalConfirmForm(ConfirmFormCommand cmd)
        {
            return _confirmFormCommandHandler.Handle(cmd);
        }

        private Task InternalActivate(ActivateCommand cmd)
        {
            return _activateCommandHandler.Handle(cmd);
        }

        private static JObject ToDto(FindResponse<CasePlanInstanceAggregate> resp)
        {
            return new JObject
            {
                { "start_index", resp.StartIndex },
                { "total_length", resp.TotalLength },
                { "count", resp.Count },
                { "content", new JArray(resp.Content.Select(r => ToDto(r))) }
            };
        }

        private static JObject ToDto(IEnumerable<CaseFileItem> caseFileItems)
        {
            var jArr = new JArray();
            var jObj = new JObject
            {
                { "casefileitems", jArr }
            };
            foreach (var caseFileItem in caseFileItems)
            {
                jArr.Add(ToDto(caseFileItem));
            }

            return jObj;
        }

        private static JObject ToDto(CaseFileItem caseFileItem)
        {
            var result = new JObject
            {
                { "element_definition_id", caseFileItem.CaseElementDefinitionId },
                { "element_instance_id", caseFileItem.CaseElementInstanceId },
                { "case_instance_id", caseFileItem.CaseInstanceId },
                { "value", caseFileItem.Value },
                { "id", caseFileItem.Id },
                { "type", caseFileItem.Type },
                { "create_datetime", caseFileItem.CreateDateTime }
            };
            return result;
        }

        private static JObject ToDto(CasePlanInstanceAggregate workflowInstance)
        {
            var result = new JObject
            {
                { "id", workflowInstance.Id },
                { "name", workflowInstance.CasePlanName },
                { "create_datetime", workflowInstance.CreateDateTime},
                { "case_plan_id", workflowInstance.CasePlanId },
                { "context", ToDto(workflowInstance.ExecutionContext) },
                { "state", workflowInstance.State }
            };
            var stateHistories = new JArray();
            var transitionHistories = new JArray();
            var executionHistories = new JArray();
            var elts = new JArray();
            foreach (var stateHistory in workflowInstance.StateHistories)
            {
                stateHistories.Add(new JObject
                {
                    { "state", stateHistory.State },
                    { "datetime", stateHistory.UpdateDateTime }
                });
            }

            foreach (var transitionHistory in workflowInstance.TransitionHistories)
            {
                transitionHistories.Add(new JObject
                {
                    { "transition", Enum.GetName(typeof(CMMNTransitions), transitionHistory.Transition) },
                    { "datetime", transitionHistory.CreateDateTime }
                });
            }

            foreach (var executionHistory in workflowInstance.ExecutionHistories)
            {
                executionHistories.Add(new JObject
                {
                    { "start_datetime", executionHistory.StartDateTime },
                    { "end_datetime", executionHistory.EndDateTime },
                    { "id", executionHistory.CaseElementDefinitionId }
                });
            }

            foreach (var elt in workflowInstance.WorkflowElementInstances)
            {
                elts.Add(ToDto(elt));
            }

            result.Add("state_histories", stateHistories);
            result.Add("transition_histories", transitionHistories);
            result.Add("execution_histories", executionHistories);
            result.Add("elements", elts);
            return result;
        }

        private static JObject ToDto(CaseElementInstance elt)
        {
            var result = new JObject
            {
                { "id", elt.Id },
                { "name", elt.CasePlanElementName },
                { "type", Enum.GetName(typeof(CaseElementTypes), elt.CasePlanElementType).ToLowerInvariant() },
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
                    { "transition", Enum.GetName(typeof(CMMNTransitions), transitionHistory.Transition) },
                    { "datetime", transitionHistory.CreateDateTime }
                });
            }

            result.Add("state_histories", stateHistories);
            result.Add("transition_histories", transitionHistories);
            return result;
        }

        private static JObject ToDto(CaseInstanceExecutionContext context)
        {
            var jObj = new JObject();
            foreach (var kvp in context.Variables)
            {
                jObj.Add(kvp.Key, kvp.Value);
            }

            return jObj;
        }

        private static FindWorkflowInstanceParameter ExtractFindParameter(IEnumerable<KeyValuePair<string, string>> query)
        {
            string casePlanId;
            string owner;
            bool takeLatest;
            var parameter = new FindWorkflowInstanceParameter();
            parameter.ExtractFindParameter(query);
            if (query.TryGet("case_plan_id", out casePlanId))
            {
                parameter.CasePlanId = casePlanId;
            }

            if (query.TryGet("owner", out owner))
            {
                parameter.CaseOwner = owner;
            }

            if (query.TryGet("take_latest", out takeLatest))
            {
                parameter.TakeLatest = takeLatest;
            }

            return parameter;
        }
    }
}
