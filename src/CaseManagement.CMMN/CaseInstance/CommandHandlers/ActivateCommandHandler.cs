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
    public class ActivateCommandHandler : IActivateCommandHandler
    {
        private readonly IEventStoreRepository _eventStoreRepository;
        private readonly IQueueProvider _queueProvider;

        public ActivateCommandHandler(IEventStoreRepository eventStoreRepository, IQueueProvider queueProvider)
        {
            _eventStoreRepository = eventStoreRepository;
            _queueProvider = queueProvider;
        }

        public async Task Handle(ActivateCommand activateCommand)
        {
            var caseInstance = await _eventStoreRepository.GetLastAggregate<Domains.CaseInstance>(activateCommand.CaseInstanceId, Domains.CaseInstance.GetStreamName(activateCommand.CaseInstanceId));
            if (caseInstance == null || string.IsNullOrWhiteSpace(caseInstance.Id))
            {
                throw new UnknownCaseInstanceException(activateCommand.CaseInstanceId);
            }

            var caseInstanceElt = caseInstance.WorkflowElementInstances.FirstOrDefault(e => e.Id == activateCommand.CaseElementInstanceId);
            if (caseInstanceElt == null)
            {
                throw new UnknownCaseInstanceElementException(caseInstance.Id, activateCommand.CaseElementInstanceId);
            }

            caseInstance.MakeTransition(caseInstanceElt.Id, CMMNTransitions.ManualStart);
            await _queueProvider.QueueTransition(caseInstance.Id, caseInstanceElt.Id, CMMNTransitions.ManualStart);
        }
    }
}
