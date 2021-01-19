using CaseManagement.CMMN.CasePlanInstance.Commands;
using CaseManagement.CMMN.CasePlanInstance.Exceptions;
using CaseManagement.CMMN.Domains;
using CaseManagement.Common.Bus;
using CaseManagement.Common.EvtStore;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CasePlanInstance.CommandHandlers
{
    public class SuspendCommandHandler : IRequestHandler<SuspendCommand, bool>
    {
        private readonly IEventStoreRepository _eventStoreRepository;
        private readonly IMessageBroker _messageBroker;

        public SuspendCommandHandler(IEventStoreRepository eventStoreRepository, IMessageBroker messageBroker)
        {
            _eventStoreRepository = eventStoreRepository;
            _messageBroker = messageBroker;
        }

        public async Task<bool> Handle(SuspendCommand suspendCommand, CancellationToken token)
        {
            var caseInstance = await _eventStoreRepository.GetLastAggregate<CasePlanInstanceAggregate>(suspendCommand.CasePlanInstanceId, CasePlanInstanceAggregate.GetStreamName(suspendCommand.CasePlanInstanceId));
            if (caseInstance == null || string.IsNullOrWhiteSpace(caseInstance.AggregateId))
            {
                throw new UnknownCasePlanInstanceException(suspendCommand.CasePlanInstanceId);
            }

            if (!string.IsNullOrWhiteSpace(suspendCommand.CasePlanInstanceElementId))
            {
                var elt = caseInstance.GetChild(suspendCommand.CasePlanInstanceElementId);
                if (elt == null)
                {
                    throw new UnknownCasePlanElementInstanceException(suspendCommand.CasePlanInstanceId, suspendCommand.CasePlanInstanceElementId);
                }

                caseInstance.MakeTransition(elt, CMMNTransitions.Suspend);
                await _messageBroker.QueueExternalEvent(CMMNConstants.ExternalTransitionNames.Suspend, suspendCommand.CasePlanInstanceId, null, suspendCommand.Parameters, token);
                return true;
            }

            caseInstance.MakeTransition(CMMNTransitions.Suspend);
            await _messageBroker.QueueExternalEvent(CMMNConstants.ExternalTransitionNames.Suspend, suspendCommand.CasePlanInstanceId, suspendCommand.CasePlanInstanceElementId, suspendCommand.Parameters, token);
            return true;
        }
    }
}
