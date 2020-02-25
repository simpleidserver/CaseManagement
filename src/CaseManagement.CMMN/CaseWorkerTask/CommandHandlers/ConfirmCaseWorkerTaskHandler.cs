using CaseManagement.CMMN.CaseWorkerTask.Commands;
using CaseManagement.CMMN.CaseWorkerTask.Exceptions;
using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Infrastructures;
using CaseManagement.CMMN.Infrastructures.EvtStore;
using CaseManagement.CMMN.Roles;
using System.Linq;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CaseWorkerTask.CommandHandlers
{
    public class ConfirmCaseWorkerTaskHandler  : IConfirmCaseWorkerTaskHandler
    {
        private readonly IEventStoreRepository _eventStoreRepository;
        private readonly IRoleService _roleService;
        private readonly ICommitAggregateHelper _commitAggregateHelper;

        public ConfirmCaseWorkerTaskHandler(IEventStoreRepository eventStoreRepository, IRoleService roleService, ICommitAggregateHelper commitAggregateHelper)
        {
            _eventStoreRepository = eventStoreRepository;
            _roleService = roleService;
            _commitAggregateHelper = commitAggregateHelper;
        }

        public async Task Handle(ConfirmCaseWorkerTask command)
        {
            var id = CaseWorkerTaskAggregate.BuildCaseWorkerTaskIdentifier(command.CasePlanInstanceId, command.CasePlanElementInstanceId);
            var caseWorkerTask = await _eventStoreRepository.GetLastAggregate<CaseWorkerTaskAggregate>(id, CaseWorkerTaskAggregate.GetStreamName(id));
            if (caseWorkerTask == null || string.IsNullOrWhiteSpace(caseWorkerTask.Id))
            {
                throw new UnknownCaseWorkerTaskException();
            }

            if (command.BypassUserValidation)
            {
                caseWorkerTask.Confirm();
            }
            else
            {
                var roles = await _roleService.FindRolesByUser(command.Performer);
                caseWorkerTask.Confirm(roles.Select(r => r.Id));
            }

            await _commitAggregateHelper.Commit(caseWorkerTask, CaseWorkerTaskAggregate.GetStreamName(id), CMMNConstants.QueueNames.CaseWorkerTasks);
        }
    }
}
