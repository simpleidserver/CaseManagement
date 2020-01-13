using CaseManagement.CMMN.CaseInstance.Commands;
using CaseManagement.CMMN.CaseInstance.Exceptions;
using CaseManagement.CMMN.Domains;
using CaseManagement.Workflow.Infrastructure.Bus;
using CaseManagement.Workflow.Infrastructure.EvtStore;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CaseInstance.CommandHandlers
{
    public class LaunchCaseInstanceCommandHandler : ILaunchCaseInstanceCommandHandler
    {
        private readonly IQueueProvider _queueProvider;
        private readonly IEventStoreRepository _eventStoreRepository;

        public LaunchCaseInstanceCommandHandler(IQueueProvider queueProvider, IEventStoreRepository eventStoreRepository)
        {
            _queueProvider = queueProvider;
            _eventStoreRepository = eventStoreRepository;
        }

        public async Task Handle(LaunchCaseInstanceCommand launchCaseInstanceCommand)
        {
            var caseInstance = await _eventStoreRepository.GetLastAggregate<Domains.CaseInstance>(launchCaseInstanceCommand.CaseInstanceId, Domains.CaseInstance.GetStreamName(launchCaseInstanceCommand.CaseInstanceId));
            if (caseInstance == null || string.IsNullOrWhiteSpace(caseInstance.Id))
            {
                throw new UnknownCaseInstanceException(launchCaseInstanceCommand.CaseInstanceId);
            }
            
            await _queueProvider.QueueLaunchProcess(caseInstance.Id);
        }
    }
}
