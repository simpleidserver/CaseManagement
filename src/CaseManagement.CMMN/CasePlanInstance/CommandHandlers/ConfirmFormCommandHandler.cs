using CaseManagement.CMMN.CasePlanInstance.Commands;
using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.FormInstance.Exceptions;
using CaseManagement.CMMN.Infrastructures;
using CaseManagement.CMMN.Infrastructures.Bus;
using CaseManagement.CMMN.Infrastructures.EvtStore;
using CaseManagement.CMMN.Persistence;
using CaseManagement.Workflow.Infrastructure.Bus;
using System.Linq;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CasePlanInstance.CommandHandlers
{
    public class ConfirmFormCommandHandler : IConfirmFormCommandHandler
    {
        private readonly IMessageBroker _messageBroker;
        private readonly IFormQueryRepository _formQueryRepository;
        private readonly IEventStoreRepository _eventStoreRepository;
        private readonly IRoleQueryRepository _roleQueryRepository;
        private readonly ICommitAggregateHelper _commitAggregateHelper;

        public ConfirmFormCommandHandler(IMessageBroker messageBroker, IFormQueryRepository formQueryRepository, IEventStoreRepository eventStoreRepository, IRoleQueryRepository roleQueryRepository, ICommitAggregateHelper commitAggregateHelper)
        {
            _messageBroker = messageBroker;
            _formQueryRepository = formQueryRepository;
            _eventStoreRepository = eventStoreRepository;
            _roleQueryRepository = roleQueryRepository;
            _commitAggregateHelper = commitAggregateHelper;
        }
        
        public async Task Handle(ConfirmFormCommand confirmFormCommand)
        {
            var id = FormInstanceAggregate.BuildFormInstanceIdentifier(confirmFormCommand.CasePlanInstanceId, confirmFormCommand.CasePlanElementInstanceId);
            var formInstance = await _eventStoreRepository.GetLastAggregate<FormInstanceAggregate>(id, FormInstanceAggregate.GetStreamName(id));
            if (formInstance == null || string.IsNullOrWhiteSpace(formInstance.Id))
            {
                throw new UnknownFormInstanceException(id);
            }

            var form = await _formQueryRepository.FindFormById(formInstance.FormId);
            if (confirmFormCommand.BypassUserValidation)
            {
                formInstance.Submit(form, confirmFormCommand.Content);
            }
            else
            {
                var roles = await _roleQueryRepository.FindRolesByUser(confirmFormCommand.Performer);
                formInstance.Submit(roles.Select(r => r.Name), form, confirmFormCommand.Content);
            }

            await _commitAggregateHelper.Commit(formInstance, FormInstanceAggregate.GetStreamName(id), CMMNConstants.QueueNames.FormInstances);
            await _messageBroker.QueueTransition(confirmFormCommand.CasePlanInstanceId, confirmFormCommand.CasePlanElementInstanceId, CMMNTransitions.Complete);
        }
    }
}
