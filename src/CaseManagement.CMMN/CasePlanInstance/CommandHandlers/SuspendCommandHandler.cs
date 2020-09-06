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
    public class SuspendCommandHandler : ISuspendCommandHandler
    {
        private readonly IEventStoreRepository _eventStoreRepository;
        private readonly IMessageBroker _messageBroker;

        public SuspendCommandHandler(IEventStoreRepository eventStoreRepository, IMessageBroker messageBroker)
        {
            _eventStoreRepository = eventStoreRepository;
            _messageBroker = messageBroker;
        }

        public async Task Handle(SuspendCommand suspendCommand)
        {
            /*
            var caseInstance = await _eventStoreRepository.GetLastAggregate<Domains.CasePlanInstanceAggregate>(suspendCommand.CaseInstanceId, Domains.CasePlanInstanceAggregate.GetStreamName(suspendCommand.CaseInstanceId));
            if (caseInstance == null || string.IsNullOrWhiteSpace(caseInstance.Id))
            {
                throw new UnknownCaseInstanceException(suspendCommand.CaseInstanceId);
            }

            if (!suspendCommand.BypassUserValidation && caseInstance.CaseOwner != suspendCommand.Performer)
            {
                throw new UnauthorizedCaseWorkerException(suspendCommand.Performer, suspendCommand.CaseInstanceId, string.Empty);
            }

            if (!string.IsNullOrWhiteSpace(suspendCommand.CaseInstanceElementId))
            {
                var elt = caseInstance.WorkflowElementInstances.FirstOrDefault(w => w.Id == suspendCommand.CaseInstanceElementId);
                if (elt == null)
                {
                    throw new UnknownCaseInstanceElementException(suspendCommand.CaseInstanceId, suspendCommand.CaseInstanceElementId);
                }

                caseInstance.MakeTransition(elt.Id, CMMNTransitions.Suspend);
                await _messageBroker.QueueTransition(caseInstance.Id, elt.Id, CMMNTransitions.Suspend);
                return;
            }

            caseInstance.MakeTransition(CMMNTransitions.Suspend);
            await _messageBroker.QueueTransition(caseInstance.Id, null, CMMNTransitions.Suspend);
            */
        }
    }
}
