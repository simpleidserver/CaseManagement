using CaseManagement.CMMN.CasePlanInstance.Exceptions;
using CaseManagement.CMMN.Infrastructure.Bus;
using CaseManagement.CMMN.Infrastructure.EvtStore;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CasePlanInstance.Commands.Handlers
{
    public class LaunchCaseInstanceCommandHandler : IRequestHandler<LaunchCaseInstanceCommand, bool>
    {
        private readonly IMessageBroker _messageBroker;
        private readonly IEventStoreRepository _eventStoreRepository;

        public LaunchCaseInstanceCommandHandler(IMessageBroker messageBroker, IEventStoreRepository eventStoreRepository)
        {
            _messageBroker = messageBroker;
            _eventStoreRepository = eventStoreRepository;
        }

        public async Task<bool> Handle(LaunchCaseInstanceCommand launchCaseInstanceCommand, CancellationToken token)
        {
            var caseInstance = await _eventStoreRepository.GetLastAggregate<Domains.CasePlanInstanceAggregate>(launchCaseInstanceCommand.CasePlanInstanceId, Domains.CasePlanInstanceAggregate.GetStreamName(launchCaseInstanceCommand.CasePlanInstanceId));
            if (caseInstance == null || string.IsNullOrWhiteSpace(caseInstance.AggregateId))
            {
                throw new UnknownCasePlanInstanceException(launchCaseInstanceCommand.CasePlanInstanceId);
            }
            
            await _messageBroker.QueueCasePlanInstance(caseInstance.AggregateId, token);
            return true;
        }
    }
}
