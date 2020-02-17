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
    public class TerminateCommandHandler : ITerminateCommandHandler
    {
        private readonly IEventStoreRepository _eventStoreRepository;
        private readonly IMessageBroker _messageBroker;

        public TerminateCommandHandler(IEventStoreRepository eventStoreRepository, IMessageBroker messageBroker)
        {
            _eventStoreRepository = eventStoreRepository;
            _messageBroker = messageBroker;
        }

        public async Task Handle(TerminateCommand terminateCommand)
        {
            var caseInstance = await _eventStoreRepository.GetLastAggregate<Domains.CasePlanInstanceAggregate>(terminateCommand.CaseInstanceId, Domains.CasePlanInstanceAggregate.GetStreamName(terminateCommand.CaseInstanceId));
            if (caseInstance == null || string.IsNullOrWhiteSpace(caseInstance.Id))
            {
                throw new UnknownCaseInstanceException(terminateCommand.CaseInstanceId);
            }

            if (!terminateCommand.BypassUserValidation && caseInstance.CaseOwner != terminateCommand.Performer)
            {
                throw new UnauthorizedCaseWorkerException(terminateCommand.Performer, terminateCommand.CaseInstanceId, string.Empty);
            }

            if (!string.IsNullOrWhiteSpace(terminateCommand.CaseInstanceElementId))
            {
                var elt = caseInstance.WorkflowElementInstances.FirstOrDefault(w => w.Id == terminateCommand.CaseInstanceElementId);
                if (elt == null)
                {
                    throw new UnknownCaseInstanceElementException(terminateCommand.CaseInstanceId, terminateCommand.CaseInstanceElementId);
                }

                caseInstance.MakeTransition(elt.Id, CMMNTransitions.Terminate);
                await _messageBroker.QueueTransition(caseInstance.Id, elt.Id, CMMNTransitions.Terminate);
                return;
            }

            caseInstance.MakeTransition(CMMNTransitions.Terminate);
            await _messageBroker.QueueTransition(caseInstance.Id, null, CMMNTransitions.Terminate);
        }
    }
}
