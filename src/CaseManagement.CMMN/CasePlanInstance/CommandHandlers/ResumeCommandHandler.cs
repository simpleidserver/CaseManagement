using CaseManagement.CMMN.CasePlanInstance.Commands;
using CaseManagement.CMMN.CasePlanInstance.Exceptions;
using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Infrastructures.Bus;
using CaseManagement.CMMN.Infrastructures.EvtStore;
using CaseManagement.Workflow.Infrastructure.Bus;
using System.Linq;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CasePlanInstance.CommandHandlers
{
    public class ResumeCommandHandler : IResumeCommandHandler
    {
        private readonly IEventStoreRepository _eventStoreRepository;
        private readonly IMessageBroker _messageBroker;

        public ResumeCommandHandler(IEventStoreRepository eventStoreRepository, IMessageBroker messageBroker)
        {
            _eventStoreRepository = eventStoreRepository;
            _messageBroker = messageBroker;
        }

        public async Task Handle(ResumeCommand resumeCommand)
        {
            /*
            var caseInstance = await _eventStoreRepository.GetLastAggregate<Domains.CasePlanInstanceAggregate>(resumeCommand.CaseInstanceId, Domains.CasePlanInstanceAggregate.GetStreamName(resumeCommand.CaseInstanceId));
            if (caseInstance == null || string.IsNullOrWhiteSpace(caseInstance.Id))
            {
                throw new UnknownCaseInstanceException(resumeCommand.CaseInstanceId);
            }

            if (!resumeCommand.BypassUserValidation && caseInstance.CaseOwner != resumeCommand.Performer)
            {
                throw new UnauthorizedCaseWorkerException(resumeCommand.Performer, resumeCommand.CaseInstanceId, string.Empty);
            }

            if (!string.IsNullOrWhiteSpace(resumeCommand.CaseInstanceElementId))
            {
                var elt = caseInstance.WorkflowElementInstances.FirstOrDefault(w => w.Id == resumeCommand.CaseInstanceElementId);
                if (elt == null)
                {
                    throw new UnknownCaseInstanceElementException(resumeCommand.CaseInstanceId, resumeCommand.CaseInstanceElementId);
                }

                caseInstance.MakeTransition(elt.Id, CMMNTransitions.Resume);
                await _messageBroker.QueueTransition(caseInstance.Id, elt.Id, CMMNTransitions.Resume);
            }

            caseInstance.MakeTransition(CMMNTransitions.Resume);
            await _messageBroker.QueueTransition(caseInstance.Id, null, CMMNTransitions.Resume);
            */
        }
    }
}
