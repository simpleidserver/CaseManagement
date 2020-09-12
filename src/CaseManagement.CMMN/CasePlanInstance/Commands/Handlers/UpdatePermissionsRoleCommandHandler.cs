using CaseManagement.CMMN.CasePlanInstance.Commands;
using CaseManagement.CMMN.CasePlanInstance.Exceptions;
using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Infrastructure;
using CaseManagement.CMMN.Infrastructure.EvtStore;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CasePlanInstance.Command.Handlers
{
    public class UpdatePermissionsRoleCommandHandler : IRequestHandler<UpdatePermissionsRoleCommand, bool>
    {
        private readonly IEventStoreRepository _eventStoreRepository;
        private readonly ICommitAggregateHelper _commitAggregateHelper;

        public UpdatePermissionsRoleCommandHandler(IEventStoreRepository eventStoreRepository, ICommitAggregateHelper commitAggregateHelper)
        {
            _eventStoreRepository = eventStoreRepository;
            _commitAggregateHelper = commitAggregateHelper;
        }

        public async Task<bool> Handle(UpdatePermissionsRoleCommand updatePermissionsRoleCommand, CancellationToken token)
        {
            var caseInstance = await _eventStoreRepository.GetLastAggregate<CasePlanInstanceAggregate>(updatePermissionsRoleCommand.CasePlanInstanceId, CasePlanInstanceAggregate.GetStreamName(updatePermissionsRoleCommand.CasePlanInstanceId));
            if (caseInstance == null || string.IsNullOrWhiteSpace(caseInstance.AggregateId))
            {
                throw new UnknownCasePlanInstanceException(updatePermissionsRoleCommand.CasePlanInstanceId);
            }

            caseInstance.UpdatePermission(updatePermissionsRoleCommand.RoleId, updatePermissionsRoleCommand.Claims.ToList());
            await _commitAggregateHelper.Commit(caseInstance, caseInstance.GetStreamName(), token);
            return true;
        }
    }
}
