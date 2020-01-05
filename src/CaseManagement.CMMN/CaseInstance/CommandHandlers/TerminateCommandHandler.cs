using CaseManagement.CMMN.CaseInstance.Commands;
using CaseManagement.CMMN.CaseInstance.Exceptions;
using CaseManagement.CMMN.Domains;
using CaseManagement.Workflow.Infrastructure.Bus;
using CaseManagement.Workflow.Infrastructure.EvtStore;
using System.Linq;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CaseInstance.CommandHandlers
{
    public class TerminateCommandHandler : ITerminateCommandHandler
    {
        private readonly IEventStoreRepository _eventStoreRepository;
        private readonly IQueueProvider _queueProvider;

        public TerminateCommandHandler(IEventStoreRepository eventStoreRepository, IQueueProvider queueProvider)
        {
            _eventStoreRepository = eventStoreRepository;
            _queueProvider = queueProvider;
        }

        public async Task Handle(TerminateCommand terminateCommand)
        {
            var caseInstance = await _eventStoreRepository.GetLastAggregate<CMMNWorkflowInstance>(terminateCommand.CaseInstanceId, CMMNWorkflowInstance.GetStreamName(terminateCommand.CaseInstanceId));
            if (caseInstance == null || string.IsNullOrWhiteSpace(caseInstance.Id))
            {
                throw new UnknownCaseInstanceException(terminateCommand.CaseInstanceId);
            }

            if (!string.IsNullOrWhiteSpace(terminateCommand.CaseInstanceElementId))
            {
                var elt = caseInstance.WorkflowElementInstances.FirstOrDefault(w => w.Id == terminateCommand.CaseInstanceElementId);
                if (elt == null)
                {
                    throw new UnknownCaseInstanceElementException(terminateCommand.CaseInstanceId, terminateCommand.CaseInstanceElementId);
                }

                caseInstance.MakeTransition(elt.Id, CMMNTransitions.Terminate);
                await _queueProvider.QueueTransition(caseInstance.Id, elt.Id, CMMNTransitions.Terminate);
                return;
            }

            caseInstance.MakeTransition(CMMNTransitions.Terminate);
            await _queueProvider.QueueTransition(caseInstance.Id, null, CMMNTransitions.Terminate);
        }
    }
}
