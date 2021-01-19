using CaseManagement.CMMN.CasePlanInstance.Exceptions;
using CaseManagement.CMMN.Domains;
using CaseManagement.Common.Bus;
using CaseManagement.Common.EvtStore;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CasePlanInstance.Commands.Handlers
{
    public class TerminateCommandHandler : IRequestHandler<TerminateCommand, bool>
    {
        private readonly IEventStoreRepository _eventStoreRepository;
        private readonly IMessageBroker _messageBroker;

        public TerminateCommandHandler(IEventStoreRepository eventStoreRepository, IMessageBroker messageBroker)
        {
            _eventStoreRepository = eventStoreRepository;
            _messageBroker = messageBroker;
        }

        public async Task<bool> Handle(TerminateCommand terminateCommand, CancellationToken token)
        {
            var caseInstance = await _eventStoreRepository.GetLastAggregate<CasePlanInstanceAggregate>(terminateCommand.CaseInstanceId, Domains.CasePlanInstanceAggregate.GetStreamName(terminateCommand.CaseInstanceId));
            if (caseInstance == null || string.IsNullOrWhiteSpace(caseInstance.AggregateId))
            {
                throw new UnknownCasePlanInstanceException(terminateCommand.CaseInstanceId);
            }

            if (!string.IsNullOrWhiteSpace(terminateCommand.CaseInstanceElementId))
            {
                var elt = caseInstance.GetChild(terminateCommand.CaseInstanceElementId);
                if (elt == null)
                {
                    throw new UnknownCasePlanElementInstanceException(terminateCommand.CaseInstanceId, terminateCommand.CaseInstanceElementId);
                }

                caseInstance.MakeTransition(elt, CMMNTransitions.Terminate);
                await _messageBroker.QueueExternalEvent(CMMNConstants.ExternalTransitionNames.Terminate, caseInstance.AggregateId, elt.Id, terminateCommand.Parameters, token);
                return true;
            }

            caseInstance.MakeTransition(CMMNTransitions.Terminate);
            await _messageBroker.QueueExternalEvent(CMMNConstants.ExternalTransitionNames.Terminate, caseInstance.AggregateId, null, terminateCommand.Parameters, token);
            return true;
        }
    }
}
