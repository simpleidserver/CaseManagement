using CaseManagement.CMMN.CaseInstance.Commands;
using CaseManagement.CMMN.CaseInstance.Exceptions;
using CaseManagement.CMMN.Domains;
using CaseManagement.Workflow.Domains;
using CaseManagement.Workflow.Infrastructure.Bus;
using CaseManagement.Workflow.Infrastructure.EvtStore;
using System.Linq;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CaseInstance.CommandHandlers
{
    public class TerminateCommandHandler : ITerminateCommandHandler
    {
        private readonly IEventStoreRepository _eventStoreRepository;
        private readonly IQueueProvider _queueProvider;

        public TerminateCommandHandler(IEventStoreRepository eventStoreRepository, IQueueProvider queueProvider)
        {
            _eventStoreRepository = eventStoreRepository;
            _queueProvider = queueProvider;
        }

        public async Task<bool> Handle(TerminateCommand terminateCommand)
        {
            var caseInstance = await _eventStoreRepository.GetLastAggregate<CMMNProcessFlowInstance>(terminateCommand.CaseInstanceId, CMMNProcessFlowInstance.GetCMMNStreamName(terminateCommand.CaseInstanceId));
            if (caseInstance == null || string.IsNullOrWhiteSpace(caseInstance.Id))
            {
                throw new UnknownCaseInstanceException(terminateCommand.CaseInstanceId);
            }

            var flowInstanceElt = caseInstance.Elements.FirstOrDefault(e => e.Id == terminateCommand.CaseElementInstanceId) as CMMNPlanItem;
            if (flowInstanceElt == null)
            {
                throw new UnknownCaseInstanceElementException(caseInstance.Id, terminateCommand.CaseElementInstanceId);
            }

            if (flowInstanceElt.PlanItemDefinitionType != CMMNPlanItemDefinitionTypes.HumanTask &&
                flowInstanceElt.PlanItemDefinitionType != CMMNPlanItemDefinitionTypes.ProcessTask &&
                flowInstanceElt.PlanItemDefinitionType != CMMNPlanItemDefinitionTypes.Task)
            {
                throw new NotSupportedTaskException("Element must be a Task");
            }

            if (!IsActivated(flowInstanceElt))
            {
                throw new CaseInvalidOperationException("Element must be activated first");
            }

            caseInstance.TerminatePlanItem(flowInstanceElt.Id);
            await _queueProvider.QueueRaiseEvent(caseInstance.Id, caseInstance.DomainEvents.Last());
            return true;
        }

        private bool IsActivated(CMMNPlanItem planItem)
        {
            switch (planItem.PlanItemDefinitionType)
            {
                case CMMNPlanItemDefinitionTypes.HumanTask:
                    return planItem.PlanItemDefinitionHumanTask.State == CMMNTaskStates.Active;
                case CMMNPlanItemDefinitionTypes.Task:
                    return planItem.PlanItemDefinitionTask.State == CMMNTaskStates.Active;
                case CMMNPlanItemDefinitionTypes.ProcessTask:
                    return planItem.PlanItemDefinitionProcessTask.State == CMMNTaskStates.Active;
            }

            return false;
        }
    }
}
