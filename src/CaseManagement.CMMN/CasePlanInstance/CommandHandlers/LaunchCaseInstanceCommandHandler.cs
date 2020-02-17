using CaseManagement.CMMN.CasePlanInstance.Commands;
using CaseManagement.CMMN.CasePlanInstance.Exceptions;
using CaseManagement.CMMN.Infrastructures.Bus;
using CaseManagement.CMMN.Infrastructures.EvtStore;
using CaseManagement.Workflow.Infrastructure.Bus;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CasePlanInstance.CommandHandlers
{
    public class LaunchCaseInstanceCommandHandler : ILaunchCaseInstanceCommandHandler
    {
        private readonly IMessageBroker _messageBroker;
        private readonly IEventStoreRepository _eventStoreRepository;

        public LaunchCaseInstanceCommandHandler(IMessageBroker messageBroker, IEventStoreRepository eventStoreRepository)
        {
            _messageBroker = messageBroker;
            _eventStoreRepository = eventStoreRepository;
        }

        public async Task Handle(LaunchCaseInstanceCommand launchCaseInstanceCommand)
        {
            var caseInstance = await _eventStoreRepository.GetLastAggregate<Domains.CasePlanInstanceAggregate>(launchCaseInstanceCommand.CasePlanInstanceId, Domains.CasePlanInstanceAggregate.GetStreamName(launchCaseInstanceCommand.CasePlanInstanceId));
            if (caseInstance == null || string.IsNullOrWhiteSpace(caseInstance.Id))
            {
                throw new UnknownCaseInstanceException(launchCaseInstanceCommand.CasePlanInstanceId);
            }

            if (caseInstance.CaseOwner != launchCaseInstanceCommand.Performer)
            {
                throw new UnauthorizedCaseWorkerException(launchCaseInstanceCommand.Performer, caseInstance.Id, null);
            }
            
            await _messageBroker.QueueLaunchProcess(caseInstance.Id);
        }
    }
}
