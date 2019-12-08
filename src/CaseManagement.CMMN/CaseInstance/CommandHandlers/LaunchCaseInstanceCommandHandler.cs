using CaseManagement.CMMN.CaseInstance.Commands;
using CaseManagement.CMMN.CaseInstance.Exceptions;
using CaseManagement.CMMN.Domains;
using CaseManagement.Workflow.Infrastructure;
using CaseManagement.Workflow.Infrastructure.Bus;
using CaseManagement.Workflow.Infrastructure.EvtStore;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CaseInstance.CommandHandlers
{
    public class LaunchCaseInstanceCommandHandler : ILaunchCaseInstanceCommandHandler
    {
        private readonly IQueueProvider _queueProvider;
        private readonly IEventStoreRepository _eventStoreRepository;
        private readonly ICommitAggregateHelper _commitAggregateHelper;

        public LaunchCaseInstanceCommandHandler(IQueueProvider queueProvider, IEventStoreRepository eventStoreRepository, ICommitAggregateHelper commitAggregateHelper)
        {
            _queueProvider = queueProvider;
            _eventStoreRepository = eventStoreRepository;
            _commitAggregateHelper = commitAggregateHelper;
        }

        public async Task Handle(LaunchCaseInstanceCommand launchCaseInstanceCommand)
        {
            var caseInstance = await _eventStoreRepository.GetLastAggregate<CMMNProcessFlowInstance>(launchCaseInstanceCommand.CaseInstanceId, CMMNProcessFlowInstance.GetCMMNStreamName(launchCaseInstanceCommand.CaseInstanceId));
            if (caseInstance == null || string.IsNullOrWhiteSpace(caseInstance.Id))
            {
                throw new UnknownCaseInstanceException(launchCaseInstanceCommand.CaseInstanceId);
            }


            caseInstance.Launch();
            await _commitAggregateHelper.Commit(caseInstance, caseInstance.GetStreamName());
            await _queueProvider.QueueLaunchProcess(caseInstance.Id);
        }
    }
}
