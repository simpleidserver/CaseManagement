using CaseManagement.Workflow.Domains;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CaseManagement.Workflow.Persistence.InMemory
{
    public class InMemoryRoleCommandRepository : IRoleCommandRepository
    {
        private ICollection<RoleAggregate> _roles;

        public InMemoryRoleCommandRepository(ICollection<RoleAggregate> roles)
        {
            _roles = roles;
        }

        public void Add(RoleAggregate role)
        {
            lock(_roles)
            {
                _roles.Add((RoleAggregate)role.Clone());
            }
        }

        public void Update(RoleAggregate role)
        {
            lock(_roles)
            {
                var record = _roles.First(r => r.Id == role.Id);
                _roles.Remove(record);
                _roles.Add((RoleAggregate)role.Clone());
            }
        }

        public Task<int> SaveChanges()
        {
            return Task.FromResult(1);
        }
    }
}