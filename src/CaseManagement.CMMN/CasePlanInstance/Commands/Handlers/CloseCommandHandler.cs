using CaseManagement.CMMN.CasePlanInstance.Commands;
using CaseManagement.CMMN.CasePlanInstance.Exceptions;
using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Infrastructure.Bus;
using CaseManagement.CMMN.Infrastructure.EvtStore;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CasePlanInstance.Command.Handlers
{
    public class CloseCommandHandler : IRequestHandler<CloseCommand, bool>
    {
        private readonly IEventStoreRepository _eventStoreRepository;
        private readonly IMessageBroker _messageBroker;

        public CloseCommandHandler(IEventStoreRepository eventStoreRepository, IMessageBroker messageBroker)
        {
            _eventStoreRepository = eventStoreRepository;
            _messageBroker = messageBroker;
        }

        public async Task<bool> Handle(CloseCommand closeCommand, CancellationToken token)
        {
            var caseInstance = await _eventStoreRepository.GetLastAggregate<CasePlanInstanceAggregate>(closeCommand.CasePlanInstanceId, CasePlanInstanceAggregate.GetStreamName(closeCommand.CasePlanInstanceId));
            if (caseInstance == null || string.IsNullOrWhiteSpace(caseInstance.AggregateId))
            {
                throw new UnknownCasePlanInstanceException(closeCommand.CasePlanInstanceId);
            }

            caseInstance.MakeTransition(CMMNTransitions.Close);
            await _messageBroker.QueueExternalEvent(CMMNConstants.ExternalTransitionNames.Close, closeCommand.CasePlanInstanceId, null,token);
            return true;
        }
    }
}
