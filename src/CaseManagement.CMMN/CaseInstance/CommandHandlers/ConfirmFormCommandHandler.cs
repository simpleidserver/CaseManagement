using CaseManagement.CMMN.CaseInstance.Commands;
using CaseManagement.CMMN.Domains;
using CaseManagement.Workflow.Domains;
using CaseManagement.Workflow.Infrastructure.EvtBus;
using CaseManagement.Workflow.Infrastructure.EvtStore;
using Microsoft.Extensions.Options;
using NEventStore;
using System.Linq;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CaseInstance.CommandHandlers
{
    public class ConfirmFormCommandHandler : BaseCommandHandler<ProcessFlowInstance>, IConfirmFormCommandHandler
    {
        private readonly IEventStoreRepository<ProcessFlowInstance> _eventStoreRepository;

        public ConfirmFormCommandHandler(IStoreEvents storeEvents, IEventBus eventBus, IEventStoreRepository<ProcessFlowInstance> eventStoreRepository, IAggregateSnapshotStore<ProcessFlowInstance> aggregateSnapshotStore, IOptions<SnapshotConfiguration> snapshotConfiguration) : base(storeEvents, eventBus, aggregateSnapshotStore, snapshotConfiguration)
        {
            _eventStoreRepository = eventStoreRepository;
        }
        
        public async Task<bool> Handle(ConfirmFormCommand confirmFormCommand)
        {
            var caseInstance = await _eventStoreRepository.GetLastAggregate(confirmFormCommand.CaseInstanceId, ProcessFlowInstance.GetStreamName(confirmFormCommand.CaseInstanceId));
            if (caseInstance == null || string.IsNullOrWhiteSpace(caseInstance.Id))
            {
                // TODO : THROW EXCEPTION.
            }

            var flowInstanceElt = caseInstance.Elements.FirstOrDefault(e => e.Id == confirmFormCommand.CaseElementInstanceId) as CMMNPlanItem;
            if (flowInstanceElt == null)
            {
                // TODO : THROW EXCEPTION.
            }

            caseInstance.ConfirmForm(confirmFormCommand.CaseElementInstanceId);
            await Commit(caseInstance, caseInstance.GetStreamName());
            return true;
        }
    }
}
