using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Infrastructures;
using CaseManagement.CMMN.Persistence;
using CaseManagement.CMMN.Roles.Commands;
using CaseManagement.CMMN.Roles.Exceptions;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Roles.CommandHandlers
{
    public class UpdateRoleCommandHandler : IUpdateRoleCommandHandler
    {
        private readonly IRoleQueryRepository _roleQueryRepository;
        private readonly ICommitAggregateHelper _commitAggregateHelper;

        public UpdateRoleCommandHandler(IRoleQueryRepository roleQueryRepository, ICommitAggregateHelper commitAggregateHelper)
        {
            _roleQueryRepository = roleQueryRepository;
            _commitAggregateHelper = commitAggregateHelper;
        }

        public async Task Handle(UpdateRoleCommand updateRoleCommand)
        {
            var role = await _roleQueryRepository.FindById(updateRoleCommand.Role);
            if (role == null)
            {
                throw new UnknownRoleException(updateRoleCommand.Role);
            }

            role.Update(updateRoleCommand.UserIds);
            await _commitAggregateHelper.Commit(role, RoleAggregate.GetStreamName(updateRoleCommand.Role), CMMNConstants.QueueNames.Roles);
        }
    }
}
