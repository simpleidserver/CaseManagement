using CaseManagement.CMMN.CasePlanInstance.Commands;
using CaseManagement.CMMN.CasePlanInstance.Exceptions;
using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Infrastructures.Bus;
using CaseManagement.CMMN.Infrastructures.EvtStore;
using CaseManagement.Workflow.Infrastructure.Bus;
using System.Threading;
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

        public async Task Handle(LaunchCaseInstanceCommand launchCaseInstanceCommand, CancellationToken token)
        {
            var caseInstance = await _eventStoreRepository.GetLastAggregate<CasePlanInstanceAggregate>(launchCaseInstanceCommand.CasePlanInstanceId, 
                CasePlanInstanceAggregate.GetStreamName(launchCaseInstanceCommand.CasePlanInstanceId), token);
            if (caseInstance == null || string.IsNullOrWhiteSpace(caseInstance.Id))
            {
                throw new UnknownCaseInstanceException(launchCaseInstanceCommand.CasePlanInstanceId);
            }
            
            await _messageBroker.QueueCasePlanInstance(caseInstance.Id);
        }
    }
}
