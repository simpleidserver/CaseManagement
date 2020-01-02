using CaseManagement.CMMN.CaseInstance.Commands;
using CaseManagement.CMMN.CaseInstance.Exceptions;
using CaseManagement.CMMN.Domains;
using CaseManagement.Workflow.Infrastructure.Bus;
using CaseManagement.Workflow.Infrastructure.EvtStore;
using System.Linq;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CaseInstance.CommandHandlers
{
    public class ActivateCommandHandler : IActivateCommandHandler
    {
        private readonly IEventStoreRepository _eventStoreRepository;
        private readonly IQueueProvider _queueProvider;

        public ActivateCommandHandler(IEventStoreRepository eventStoreRepository, IQueueProvider queueProvider)
        {
            _eventStoreRepository = eventStoreRepository;
            _queueProvider = queueProvider;
        }

        public async Task<bool> Handle(ActivateCommand activateCommand)
        {
            var caseInstance = await _eventStoreRepository.GetLastAggregate<CMMNWorkflowInstance>(activateCommand.CaseInstanceId, CMMNWorkflowInstance.GetStreamName(activateCommand.CaseInstanceId));
            if (caseInstance == null || string.IsNullOrWhiteSpace(caseInstance.Id))
            {
                throw new UnknownCaseInstanceException(activateCommand.CaseInstanceId);
            }

            /*
            var flowInstanceElt = caseInstance.Elements.FirstOrDefault(e => e.Id == activateCommand.CaseElementInstanceId) as CMMNPlanItemDefinition;
            if (flowInstanceElt == null)
            {
                throw new UnknownCaseInstanceElementException(caseInstance.Id, activateCommand.CaseElementInstanceId);
            }
            
            if (flowInstanceElt.PlanItemDefinitionType != CMMNPlanItemDefinitionTypes.HumanTask &&
                flowInstanceElt.PlanItemDefinitionType != CMMNPlanItemDefinitionTypes.ProcessTask &&
                flowInstanceElt.PlanItemDefinitionType != CMMNPlanItemDefinitionTypes.Task)
            {
                throw new NotSupportedTaskException("Element must be a Task");
            }
            
            if (flowInstanceElt.ManualActivationRule == null || (flowInstanceElt.ManualActivationRule != null && !IsEnabled(flowInstanceElt)))
            {
                throw new CaseInvalidOperationException("Element must be activated first");
            }

            caseInstance.ManuallyStartPlanItem(flowInstanceElt.Id);
            await _queueProvider.QueueRaiseEvent(caseInstance.Id, caseInstance.DomainEvents.Last());
            */
            return true;
        }

        /*
        private bool IsEnabled(CMMNPlanItemDefinition planItem)
        {
            switch(planItem.PlanItemDefinitionType)
            {
                case CMMNPlanItemDefinitionTypes.HumanTask:
                    return planItem.PlanItemDefinitionHumanTask.State == CMMNTaskStates.Enabled;
                case CMMNPlanItemDefinitionTypes.Task:
                    return planItem.PlanItemDefinitionTask.State == CMMNTaskStates.Enabled;
                case CMMNPlanItemDefinitionTypes.ProcessTask:
                    return planItem.PlanItemDefinitionProcessTask.State == CMMNTaskStates.Enabled;
            }

            return false;
        }
        */
    }
}
