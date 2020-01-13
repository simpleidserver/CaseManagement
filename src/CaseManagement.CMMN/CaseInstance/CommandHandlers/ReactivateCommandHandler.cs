using CaseManagement.CMMN.CaseInstance.Commands;
using CaseManagement.CMMN.CaseInstance.Exceptions;
using CaseManagement.CMMN.Domains;
using CaseManagement.Workflow.Infrastructure.Bus;
using CaseManagement.Workflow.Infrastructure.EvtStore;
using System.Linq;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CaseInstance.CommandHandlers
{
    public class ReactivateCommandHandler : IReactivateCommandHandler
    {
        private readonly IEventStoreRepository _eventStoreRepository;
        private readonly IQueueProvider _queueProvider;

        public ReactivateCommandHandler(IEventStoreRepository eventStoreRepository, IQueueProvider queueProvider)
        {
            _eventStoreRepository = eventStoreRepository;
            _queueProvider = queueProvider;
        }

        public async Task Handle(ReactivateCommand reactivateCommand)
        {
            var caseInstance = await _eventStoreRepository.GetLastAggregate<Domains.CaseInstance>(reactivateCommand.CaseInstanceId, Domains.CaseInstance.GetStreamName(reactivateCommand.CaseInstanceId));
            if (caseInstance == null || string.IsNullOrWhiteSpace(caseInstance.Id))
            {
                throw new UnknownCaseInstanceException(reactivateCommand.CaseInstanceId);
            }

            if (!string.IsNullOrWhiteSpace(reactivateCommand.CaseInstanceElementId))
            {
                var elt = caseInstance.WorkflowElementInstances.FirstOrDefault(w => w.Id == reactivateCommand.CaseInstanceElementId);
                if (elt == null)
                {
                    throw new UnknownCaseInstanceElementException(reactivateCommand.CaseInstanceId, reactivateCommand.CaseInstanceElementId);
                }

                caseInstance.MakeTransition(elt.Id, CMMNTransitions.Reactivate);
                await _queueProvider.QueueTransition(caseInstance.Id, elt.Id, CMMNTransitions.Reactivate);
                return;
            }

            caseInstance.MakeTransition(CMMNTransitions.Reactivate);
            await _queueProvider.QueueReactivateProcess(caseInstance.Id);
        }
    }
}
