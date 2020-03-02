using System.Collections.Generic;
using System.Threading.Tasks;
using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Infrastructures;
using CaseManagement.CMMN.Persistence;
using CaseManagement.CMMN.Roles.Commands;

namespace CaseManagement.CMMN.Roles.CommandHandlers
{
    public class AddRoleCommandHandler : IAddRoleCommandHandler
    {
        private readonly IRoleQueryRepository _roleQueryRepository;
        private readonly ICommitAggregateHelper _commitAggregateHelper;

        public AddRoleCommandHandler(IRoleQueryRepository roleQueryRepository, ICommitAggregateHelper commitAggregateHelper)
        {
            _roleQueryRepository = roleQueryRepository;
            _commitAggregateHelper = commitAggregateHelper;
        }

        public async Task<RoleAggregate> Handle(AddRoleCommand addRoleCommand)
        {
            var role = await _roleQueryRepository.FindById(addRoleCommand.Role);
            if (role != null)
            {
                throw new AggregateValidationException(new Dictionary<string, string>
                {
                    { "validation", "role already exists" }
                });
            }

            var newRole = RoleAggregate.New(addRoleCommand.Role);
            await _commitAggregateHelper.Commit(newRole, RoleAggregate.GetStreamName(newRole.Id), CMMNConstants.QueueNames.Roles);
            return newRole;
        }
    }
}
