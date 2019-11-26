using CaseManagement.CMMN.CaseInstance.Commands;
using CaseManagement.Workflow.Domains;
using CaseManagement.Workflow.Infrastructure.EvtBus;
using CaseManagement.Workflow.Infrastructure.EvtStore;
using Microsoft.Extensions.Options;
using NEventStore;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CaseInstance.CommandHandlers
{
    public class LaunchCaseInstanceCommandHandler : BaseCommandHandler<ProcessFlowInstance>, ILaunchCaseInstanceCommandHandler
    {
        private readonly IEventStoreRepository<ProcessFlowInstance> _eventStoreRepository;

        public LaunchCaseInstanceCommandHandler(IStoreEvents storeEvents, IEventBus eventBus, IEventStoreRepository<ProcessFlowInstance> eventStoreRepository, IAggregateSnapshotStore<ProcessFlowInstance> aggregateSnapshotStore, IOptions<SnapshotConfiguration> snapshotConfiguration) : base(storeEvents, eventBus, aggregateSnapshotStore, snapshotConfiguration)
        {
            _eventStoreRepository = eventStoreRepository;
        }

        public async Task Handle(LaunchCaseInstanceCommand launchCaseInstanceCommand)
        {
            var caseInstance = await _eventStoreRepository.GetLastAggregate(launchCaseInstanceCommand.CaseInstanceId, ProcessFlowInstance.GetStreamName(launchCaseInstanceCommand.CaseInstanceId));
            if (caseInstance == null || string.IsNullOrWhiteSpace(caseInstance.Id))
            {
                // TODO : THROW EXCEPTION.
            }


            caseInstance.Launch();
            await Commit(caseInstance, caseInstance.GetStreamName());
        }
    }
}
