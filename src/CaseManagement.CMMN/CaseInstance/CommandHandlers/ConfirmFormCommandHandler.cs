using CaseManagement.CMMN.CaseInstance.Commands;
using CaseManagement.CMMN.CaseInstance.Exceptions;
using CaseManagement.CMMN.Domains;
using CaseManagement.Workflow.Domains;
using CaseManagement.Workflow.Infrastructure.EvtBus;
using CaseManagement.Workflow.Infrastructure.EvtStore;
using CaseManagement.Workflow.Persistence;
using Microsoft.Extensions.Options;
using NEventStore;
using System.Linq;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CaseInstance.CommandHandlers
{
    public class ConfirmFormCommandHandler : BaseCommandHandler<ProcessFlowInstance>, IConfirmFormCommandHandler
    {
        private readonly IFormQueryRepository _formQueryRepository;
        private readonly IEventStoreRepository<ProcessFlowInstance> _eventStoreRepository;

        public ConfirmFormCommandHandler(IFormQueryRepository formQueryRepository, IStoreEvents storeEvents, IEventBus eventBus, IEventStoreRepository<ProcessFlowInstance> eventStoreRepository, IAggregateSnapshotStore<ProcessFlowInstance> aggregateSnapshotStore, IOptions<SnapshotConfiguration> snapshotConfiguration) : base(storeEvents, eventBus, aggregateSnapshotStore, snapshotConfiguration)
        {
            _formQueryRepository = formQueryRepository;
            _eventStoreRepository = eventStoreRepository;
        }
        
        public async Task<bool> Handle(ConfirmFormCommand confirmFormCommand)
        {
            var caseInstance = await _eventStoreRepository.GetLastAggregate(confirmFormCommand.CaseInstanceId, ProcessFlowInstance.GetStreamName(confirmFormCommand.CaseInstanceId));
            if (caseInstance == null || string.IsNullOrWhiteSpace(caseInstance.Id))
            {
                throw new UnknownCaseInstanceException(confirmFormCommand.CaseInstanceId);
            }

            var flowInstanceElt = caseInstance.Elements.FirstOrDefault(e => e.Id == confirmFormCommand.CaseElementInstanceId) as CMMNPlanItem;
            if (flowInstanceElt == null)
            {
                throw new UnknownCaseInstanceElementException(caseInstance.Id, confirmFormCommand.CaseElementInstanceId);
            }

            var humanTask = flowInstanceElt.PlanItemDefinition as CMMNHumanTask;
            if (humanTask == null)
            {
                throw new NotSupportedTaskException("Task must be a HumanTask");
            }

            var form = await _formQueryRepository.FindFormById(humanTask.FormId);
            if (form == null)
            {
                throw new UnknownFormException(humanTask.FormId);
            }

            caseInstance.ConfirmForm(confirmFormCommand.CaseElementInstanceId, form, confirmFormCommand.Content);
            await Commit(caseInstance, caseInstance.GetStreamName());
            return true;
        }
    }
}
