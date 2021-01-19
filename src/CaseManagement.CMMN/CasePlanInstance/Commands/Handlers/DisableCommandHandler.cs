using CaseManagement.CMMN.CasePlanInstance.Exceptions;
using CaseManagement.CMMN.Domains;
using CaseManagement.Common.Bus;
using CaseManagement.Common.EvtStore;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CasePlanInstance.Commands.Handlers
{
    public class DisableCommandHandler : IRequestHandler<DisableCommand, bool>
    {
        private readonly IEventStoreRepository _eventStoreRepository;
        private readonly IMessageBroker _messageBroker;

        public DisableCommandHandler(IEventStoreRepository eventStoreRepository, IMessageBroker messageBroker)
        {
            _eventStoreRepository = eventStoreRepository;
            _messageBroker = messageBroker;
        }

        public async Task<bool> Handle(DisableCommand command, CancellationToken token)
        {
            var casePlanInstance = await _eventStoreRepository.GetLastAggregate<CasePlanInstanceAggregate>(command.CasePlanInstanceId, CasePlanInstanceAggregate.GetStreamName(command.CasePlanInstanceId));
            if (casePlanInstance == null)
            {
                throw new UnknownCasePlanInstanceException(command.CasePlanInstanceId);
            }

            var elt = casePlanInstance.GetChild(command.CasePlanElementInstanceId);
            if (elt == null)
            {
                throw new UnknownCasePlanElementInstanceException(command.CasePlanInstanceId, command.CasePlanElementInstanceId);
            }

            casePlanInstance.MakeTransition(elt, CMMNTransitions.Disable);
            await _messageBroker.QueueExternalEvent(CMMNConstants.ExternalTransitionNames.Disable, command.CasePlanInstanceId, command.CasePlanElementInstanceId, command.Parameters, token);
            return true;
        }
    }
}