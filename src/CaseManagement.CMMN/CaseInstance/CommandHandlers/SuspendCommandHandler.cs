using CaseManagement.CMMN.CaseInstance.Commands;
using CaseManagement.CMMN.CaseInstance.Exceptions;
using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Infrastructures.Bus;
using CaseManagement.CMMN.Infrastructures.EvtStore;
using CaseManagement.Workflow.Infrastructure.Bus;
using System.Linq;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CaseInstance.CommandHandlers
{
    public class SuspendCommandHandler : ISuspendCommandHandler
    {
        private readonly IEventStoreRepository _eventStoreRepository;
        private readonly IQueueProvider _queueProvider;

        public SuspendCommandHandler(IEventStoreRepository eventStoreRepository, IQueueProvider queueProvider)
        {
            _eventStoreRepository = eventStoreRepository;
            _queueProvider = queueProvider;
        }

        public async Task Handle(SuspendCommand suspendCommand)
        {
            var caseInstance = await _eventStoreRepository.GetLastAggregate<Domains.CaseInstance>(suspendCommand.CaseInstanceId, Domains.CaseInstance.GetStreamName(suspendCommand.CaseInstanceId));
            if (caseInstance == null || string.IsNullOrWhiteSpace(caseInstance.Id))
            {
                throw new UnknownCaseInstanceException(suspendCommand.CaseInstanceId);
            }

            if (!string.IsNullOrWhiteSpace(suspendCommand.CaseInstanceElementId))
            {
                var elt = caseInstance.WorkflowElementInstances.FirstOrDefault(w => w.Id == suspendCommand.CaseInstanceElementId);
                if (elt == null)
                {
                    throw new UnknownCaseInstanceElementException(suspendCommand.CaseInstanceId, suspendCommand.CaseInstanceElementId);
                }

                caseInstance.MakeTransition(elt.Id, CMMNTransitions.Suspend);
                await _queueProvider.QueueTransition(caseInstance.Id, elt.Id, CMMNTransitions.Suspend);
                return;
            }

            caseInstance.MakeTransition(CMMNTransitions.Suspend);
            await _queueProvider.QueueTransition(caseInstance.Id, null, CMMNTransitions.Suspend);
        }
    }
}
