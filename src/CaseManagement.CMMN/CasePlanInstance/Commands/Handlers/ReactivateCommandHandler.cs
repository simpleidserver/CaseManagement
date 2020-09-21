using CaseManagement.CMMN.CasePlanInstance.Exceptions;
using CaseManagement.CMMN.Domains;
using CaseManagement.Common.Bus;
using CaseManagement.Common.EvtStore;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CasePlanInstance.Commands.Handlers
{
    public class ReactivateCommandHandler : IRequestHandler<ReactivateCommand, bool>
    {
        private readonly IEventStoreRepository _eventStoreRepository;
        private readonly IMessageBroker _messageBroker;

        public ReactivateCommandHandler(IEventStoreRepository eventStoreRepository, IMessageBroker messageBroker)
        {
            _eventStoreRepository = eventStoreRepository;
            _messageBroker = messageBroker;
        }

        public async Task<bool> Handle(ReactivateCommand reactivateCommand, CancellationToken token)
        {
            var caseInstance = await _eventStoreRepository.GetLastAggregate<CasePlanInstanceAggregate>(reactivateCommand.CaseInstanceId, CasePlanInstanceAggregate.GetStreamName(reactivateCommand.CaseInstanceId));
            if (caseInstance == null || string.IsNullOrWhiteSpace(caseInstance.AggregateId))
            {
                throw new UnknownCasePlanInstanceException(reactivateCommand.CaseInstanceId);
            }

            if (!string.IsNullOrWhiteSpace(reactivateCommand.CaseInstanceElementId))
            {
                var elt = caseInstance.GetChild(reactivateCommand.CaseInstanceElementId);
                if (elt == null)
                {
                    throw new UnknownCasePlanElementInstanceException(reactivateCommand.CaseInstanceId, reactivateCommand.CaseInstanceElementId);
                }

                caseInstance.MakeTransition(elt, CMMNTransitions.Reactivate);
                await _messageBroker.QueueExternalEvent(CMMNConstants.ExternalTransitionNames.Reactivate, reactivateCommand.CaseInstanceId, reactivateCommand.CaseInstanceElementId, token);
                return true;
            }

            caseInstance.MakeTransition(CMMNTransitions.Reactivate);
            await _messageBroker.QueueExternalEvent(CMMNConstants.ExternalTransitionNames.Reactivate, reactivateCommand.CaseInstanceId, null, token);
            return true;
        }
    }
}
