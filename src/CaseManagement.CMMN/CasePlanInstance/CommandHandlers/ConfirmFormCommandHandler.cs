using CaseManagement.CMMN.CasePlanInstance.Commands;
using CaseManagement.CMMN.CasePlanInstance.Exceptions;
using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.FormInstance;
using CaseManagement.CMMN.FormInstance.Commands;
using CaseManagement.CMMN.Infrastructures.Bus;
using CaseManagement.CMMN.Infrastructures.EvtStore;
using CaseManagement.Workflow.Infrastructure.Bus;
using System.Linq;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CasePlanInstance.CommandHandlers
{
    public class ConfirmFormCommandHandler : IConfirmFormCommandHandler
    {
        private readonly IMessageBroker _messageBroker;
        private readonly IEventStoreRepository _eventStoreRepository;
        private readonly IFormInstanceService _formInstanceService;

        public ConfirmFormCommandHandler(IMessageBroker messageBroker, IEventStoreRepository eventStoreRepository, IFormInstanceService formInstanceService)
        {
            _messageBroker = messageBroker;
            _eventStoreRepository = eventStoreRepository;
            _formInstanceService = formInstanceService;
        }
        
        public async Task Handle(ConfirmFormCommand command)
        {
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

            casePlanInstance.MakeTransition(command.CasePlanElementInstanceId, CMMNTransitions.Complete);
            await _formInstanceService.Confirm(new ConfirmFormInstanceCommand
            {
                BypassUserValidation = command.BypassUserValidation,
                CasePlanElementInstanceId = command.CasePlanElementInstanceId,
                CasePlanInstanceId = command.CasePlanInstanceId,
                Performer = command.Performer
            });
            await _messageBroker.QueueTransition(command.CasePlanInstanceId, command.CasePlanElementInstanceId, CMMNTransitions.Complete);
        }
    }
}
