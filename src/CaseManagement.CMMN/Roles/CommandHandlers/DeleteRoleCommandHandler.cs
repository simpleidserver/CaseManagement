using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Infrastructures;
using CaseManagement.CMMN.Persistence;
using CaseManagement.CMMN.Roles.Commands;
using CaseManagement.CMMN.Roles.Exceptions;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Roles.CommandHandlers
{
    public class DeleteRoleCommandHandler : IDeleteRoleCommandHandler
    {
        private readonly IRoleQueryRepository _roleQueryRepository;
        private readonly ICommitAggregateHelper _commitAggregateHelper;

        public DeleteRoleCommandHandler(IRoleQueryRepository roleQueryRepository, ICommitAggregateHelper commitAggregateHelper)
        {
            _roleQueryRepository = roleQueryRepository;
            _commitAggregateHelper = commitAggregateHelper;
        }

        public async Task Handle(DeleteRoleCommand deleteRoleCommand)
        {
            var role = await _roleQueryRepository.FindById(deleteRoleCommand.Role);
            if (role == null)
            {
                throw new UnknownRoleException(deleteRoleCommand.Role);
            }

            role.Delete();
            await _commitAggregateHelper.Commit(role, RoleAggregate.GetStreamName(deleteRoleCommand.Role), CMMNConstants.QueueNames.Roles);
        }
    }
}
