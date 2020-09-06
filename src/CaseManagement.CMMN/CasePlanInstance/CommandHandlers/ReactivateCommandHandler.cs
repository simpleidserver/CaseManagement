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
    public class ReactivateCommandHandler : IReactivateCommandHandler
    {
        private readonly IEventStoreRepository _eventStoreRepository;
        private readonly IMessageBroker _messageBroker;

        public ReactivateCommandHandler(IEventStoreRepository eventStoreRepository, IMessageBroker messageBroker)
        {
            _eventStoreRepository = eventStoreRepository;
            _messageBroker = messageBroker;
        }

        public async Task Handle(ReactivateCommand reactivateCommand)
        {
            /*
            var caseInstance = await _eventStoreRepository.GetLastAggregate<Domains.CasePlanInstanceAggregate>(reactivateCommand.CaseInstanceId, Domains.CasePlanInstanceAggregate.GetStreamName(reactivateCommand.CaseInstanceId));
            if (caseInstance == null || string.IsNullOrWhiteSpace(caseInstance.Id))
            {
                throw new UnknownCaseInstanceException(reactivateCommand.CaseInstanceId);
            }            

            if (!reactivateCommand.BypassUserValidation && caseInstance.CaseOwner != reactivateCommand.Performer)
            {
                throw new UnauthorizedCaseWorkerException(reactivateCommand.Performer, reactivateCommand.CaseInstanceId, string.Empty);
            }

            if (!string.IsNullOrWhiteSpace(reactivateCommand.CaseInstanceElementId))
            {
                var elt = caseInstance.WorkflowElementInstances.FirstOrDefault(w => w.Id == reactivateCommand.CaseInstanceElementId);
                if (elt == null)
                {
                    throw new UnknownCaseInstanceElementException(reactivateCommand.CaseInstanceId, reactivateCommand.CaseInstanceElementId);
                }

                caseInstance.MakeTransition(elt.Id, CMMNTransitions.Reactivate);
                await _messageBroker.QueueTransition(caseInstance.Id, elt.Id, CMMNTransitions.Reactivate);
                return;
            }

            caseInstance.MakeTransition(CMMNTransitions.Reactivate);
            await _messageBroker.QueueReactivateProcess(caseInstance.Id);
            */
        }
    }
}
