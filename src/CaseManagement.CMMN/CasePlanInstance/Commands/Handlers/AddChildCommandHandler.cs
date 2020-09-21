using CaseManagement.CMMN.CasePlanInstance.Exceptions;
using CaseManagement.CMMN.Domains;
using CaseManagement.Common.Bus;
using CaseManagement.Common.EvtStore;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CasePlanInstance.Commands.Handlers
{
    public class AddChildCommandHandler : IRequestHandler<AddChildCommand, bool>
    {
        private readonly IEventStoreRepository _eventStoreRepository;
        private readonly IMessageBroker _messageBroker;

        public AddChildCommandHandler(IEventStoreRepository eventStoreRepository, IMessageBroker messageBroker)
        {
            _eventStoreRepository = eventStoreRepository;
            _messageBroker = messageBroker;
        }

        public async Task<bool> Handle(AddChildCommand addChildCommand, CancellationToken token)
        {
            var caseInstance = await _eventStoreRepository.GetLastAggregate<CasePlanInstanceAggregate>(addChildCommand.CasePlanInstanceId, CasePlanInstanceAggregate.GetStreamName(addChildCommand.CasePlanInstanceId));
            if (caseInstance == null || string.IsNullOrWhiteSpace(caseInstance.AggregateId))
            {
                throw new UnknownCasePlanInstanceException(addChildCommand.CasePlanInstanceId);
            }

            var elt = caseInstance.GetChild(addChildCommand.CasePlanInstanceElementId);
            if (elt == null)
            {
                throw new UnknownCasePlanElementInstanceException(addChildCommand.CasePlanInstanceId, addChildCommand.CasePlanInstanceElementId);
            }

            caseInstance.MakeTransition(elt, CMMNTransitions.AddChild);
            await _messageBroker.QueueExternalEvent(CMMNConstants.ExternalTransitionNames.AddChild, addChildCommand.CasePlanInstanceId, addChildCommand.CasePlanInstanceElementId, token);
            return true;
        }
    }
}
