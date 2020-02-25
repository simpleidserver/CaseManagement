using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Form;
using CaseManagement.CMMN.FormInstance.Commands;
using CaseManagement.CMMN.FormInstance.Exceptions;
using CaseManagement.CMMN.Infrastructures;
using CaseManagement.CMMN.Infrastructures.EvtStore;
using CaseManagement.CMMN.Roles;
using System.Linq;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.FormInstance.CommandHandlers
{
    public class ConfirmFormInstanceCommandHandler : IConfirmFormInstanceCommandHandler
    {
        private readonly IRoleService _roleService;
        private readonly IFormService _formService;
        private readonly IEventStoreRepository _eventStoreRepository;
        private readonly ICommitAggregateHelper _commitAggregateHelper;

        public ConfirmFormInstanceCommandHandler(IRoleService roleService, IFormService formService, IEventStoreRepository eventStoreRepository, ICommitAggregateHelper commitAggregateHelper)
        {
            _roleService = roleService;
            _formService = formService;
            _eventStoreRepository = eventStoreRepository;
            _commitAggregateHelper = commitAggregateHelper;
        }

        public async Task Handle(ConfirmFormInstanceCommand cmd)
        {
            var id = FormInstanceAggregate.BuildFormInstanceIdentifier(cmd.CasePlanInstanceId, cmd.CasePlanElementInstanceId);
            var formInstance = await _eventStoreRepository.GetLastAggregate<FormInstanceAggregate>(id, FormInstanceAggregate.GetStreamName(id));
            if (formInstance == null || string.IsNullOrWhiteSpace(formInstance.Id))
            {
                throw new UnknownFormInstanceException(id);
            }

            var form = await _formService.GetForm(formInstance.FormId);
            if (cmd.BypassUserValidation)
            {
                formInstance.Submit(form, cmd.Content);
            }
            else
            {
                var roles = await _roleService.FindRolesByUser(cmd.Performer);
                formInstance.Submit(roles.Select(r => r.Id), form, cmd.Content);
            }

            await _commitAggregateHelper.Commit(formInstance, FormInstanceAggregate.GetStreamName(id), CMMNConstants.QueueNames.FormInstances);
        }
    }
}
