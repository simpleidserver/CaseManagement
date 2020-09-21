using CaseManagement.CMMN.CasePlanInstance.Exceptions;
using CaseManagement.CMMN.Domains;
using CaseManagement.Common.Bus;
using CaseManagement.Common.EvtStore;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CasePlanInstance.Commands.Handlers
{
    public class CompleteCommandHandler : IRequestHandler<CompleteCommand, bool>
    {
        private readonly IEventStoreRepository _eventStoreRepository;
        private readonly IMessageBroker _messageBroker;

        public CompleteCommandHandler(IEventStoreRepository eventStoreRepository, IMessageBroker messageBroker)
        {
            _eventStoreRepository = eventStoreRepository;
            _messageBroker = messageBroker;
        }

        public async Task<bool> Handle(CompleteCommand completeCommand, CancellationToken token)
        {
            var caseInstance = await _eventStoreRepository.GetLastAggregate<CasePlanInstanceAggregate>(completeCommand.CaseInstanceId, CasePlanInstanceAggregate.GetStreamName(completeCommand.CaseInstanceId));
            if (caseInstance == null || string.IsNullOrWhiteSpace(caseInstance.AggregateId))
            {
                throw new UnknownCasePlanInstanceException(completeCommand.CaseInstanceId);
            }


            var elt = caseInstance.GetChild(completeCommand.CaseInstanceElementId);
            if (elt == null)
            {
                throw new UnknownCasePlanElementInstanceException(completeCommand.CaseInstanceId, completeCommand.CaseInstanceElementId);
            }

            caseInstance.MakeTransition(elt, CMMNTransitions.Complete);
            await _messageBroker.QueueExternalEvent(CMMNConstants.ExternalTransitionNames.Complete, completeCommand.CaseInstanceId, completeCommand.CaseInstanceElementId, token);
            return true;
        }
    }
}
