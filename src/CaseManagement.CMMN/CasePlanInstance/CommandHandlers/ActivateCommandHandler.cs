using CaseManagement.CMMN.CasePlanInstance.Commands;
using CaseManagement.CMMN.CaseWorkerTask;
using CaseManagement.CMMN.Infrastructures.Bus;
using CaseManagement.CMMN.Infrastructures.EvtStore;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CasePlanInstance.CommandHandlers
{
    public class ActivateCommandHandler : IActivateCommandHandler
    {
        private readonly IEventStoreRepository _eventStoreRepository;
        private readonly ICaseWorkerTaskService _caseWorkerTaskService;
        private readonly IMessageBroker _messageBroker;

        public ActivateCommandHandler(IEventStoreRepository eventStoreRepository, ICaseWorkerTaskService caseWorkerTaskService, IMessageBroker messageBroker)
        {
            _eventStoreRepository = eventStoreRepository;
            _caseWorkerTaskService = caseWorkerTaskService;
            _messageBroker = messageBroker;
        }

        public async Task Handle(ActivateCommand command)
        {
            /*
            var casePlanInstance = await _eventStoreRepository.GetLastAggregate<CasePlanInstanceAggregate>(command.CasePlanInstanceId, CasePlanInstanceAggregate.GetStreamName(command.CasePlanInstanceId));
            if (casePlanInstance == null)
            {
                throw new UnknownCaseInstanceException(command.CasePlanInstanceId);
            }

            var elt = casePlanInstance.WorkflowElementInstances.FirstOrDefault(w => w.Id == command.CasePlanElementInstanceId);
            if (elt == null)
            {
                throw new UnknownCaseInstanceElementException(command.CasePlanInstanceId, command.CasePlanElementInstanceId);
            }

            casePlanInstance.MakeTransition(command.CasePlanElementInstanceId, CMMNTransitions.ManualStart);
            await _caseWorkerTaskService.ConfirmCaseWorker(new ConfirmCaseWorkerTask
            {
                BypassUserValidation = command.BypassUserValidation,
                CasePlanElementInstanceId = command.CasePlanElementInstanceId,
                CasePlanInstanceId = command.CasePlanInstanceId,
                Performer = command.Performer
            });
            await _messageBroker.QueueTransition(command.CasePlanInstanceId, command.CasePlanElementInstanceId, CMMNTransitions.ManualStart);
            */
        }
    }
}