using CaseManagement.CMMN.CasePlanInstance.Commands;
using CaseManagement.CMMN.CasePlanInstance.Exceptions;
using CaseManagement.CMMN.Domains;
using CaseManagement.Common.Bus;
using CaseManagement.Common.EvtStore;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CasePlanInstance.Command.Handlers
{
    public class ResumeCommandHandler : IRequestHandler<ResumeCommand, bool>
    {
        private readonly IEventStoreRepository _eventStoreRepository;
        private readonly IMessageBroker _messageBroker;

        public ResumeCommandHandler(IEventStoreRepository eventStoreRepository, IMessageBroker messageBroker)
        {
            _eventStoreRepository = eventStoreRepository;
            _messageBroker = messageBroker;
        }

        public async Task<bool> Handle(ResumeCommand resumeCommand, CancellationToken token)
        {
            var caseInstance = await _eventStoreRepository.GetLastAggregate<CasePlanInstanceAggregate>(resumeCommand.CasePlanInstanceId, CasePlanInstanceAggregate.GetStreamName(resumeCommand.CasePlanInstanceId));
            if (caseInstance == null || string.IsNullOrWhiteSpace(caseInstance.AggregateId))
            {
                throw new UnknownCasePlanInstanceException(resumeCommand.CasePlanInstanceId);
            }

            if (!string.IsNullOrWhiteSpace(resumeCommand.CasePlanInstanceElementId))
            {
                var elt = caseInstance.GetChild(resumeCommand.CasePlanInstanceElementId);
                if (elt == null)
                {
                    throw new UnknownCasePlanElementInstanceException(resumeCommand.CasePlanInstanceId, resumeCommand.CasePlanInstanceElementId);
                }

                caseInstance.MakeTransition(elt, CMMNTransitions.Resume);
                await _messageBroker.QueueExternalEvent(CMMNConstants.ExternalTransitionNames.Resume, resumeCommand.CasePlanInstanceId, resumeCommand.CasePlanInstanceElementId, resumeCommand.Parameters, token);
                return true;

            }

            caseInstance.MakeTransition(CMMNTransitions.Resume);
            await _messageBroker.QueueExternalEvent(CMMNConstants.ExternalTransitionNames.Resume, resumeCommand.CasePlanInstanceId, resumeCommand.CasePlanInstanceElementId, resumeCommand.Parameters, token);
            return true;
        }
    }
}
