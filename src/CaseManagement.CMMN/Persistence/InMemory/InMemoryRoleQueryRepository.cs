using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Extensions;
using CaseManagement.CMMN.Persistence.Parameters;
using CaseManagement.CMMN.Persistence.Responses;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Persistence.InMemory
{
    public class InMemoryRoleQueryRepository : IRoleQueryRepository
    {
        private static Dictionary<string, string> MAPPING_ROLE_TO_PROPERTYNAME = new Dictionary<string, string>
        {
            { "id", "Id" },
            { "create_datetime", "CreateDateTime" },
            { "update_datetime", "UpdateDateTime" }
        };
        private readonly ICollection<RoleAggregate> _roles;

        public InMemoryRoleQueryRepository(ICollection<RoleAggregate> roles)
        {
            _roles = roles;
        }

        public Task<RoleAggregate> FindById(string id)
        {
            return Task.FromResult(_roles.FirstOrDefault(r => r.Id == id));
        }

        public Task<IEnumerable<RoleAggregate>> FindRolesByUser(string userId)
        {
            return Task.FromResult(_roles.Where(r => r.UserIds.Contains(userId)));
        }

        public Task<IEnumerable<RoleAggregate>> FindRoles(ICollection<string> roles)
        {
            return Task.FromResult(_roles.Where(r => roles.Contains(r.Id)));
        }

        public Task<FindResponse<RoleAggregate>> Search(FindRoleParameter findRoleParameter)
        {
            var result = _roles.AsQueryable();
            if (findRoleParameter.IsDeleted != null)
            {
                result = result.Where(r => r.IsDeleted == findRoleParameter.IsDeleted.Value);
            }

            if (MAPPING_ROLE_TO_PROPERTYNAME.ContainsKey(findRoleParameter.OrderBy))
            {
                result = result.InvokeOrderBy(MAPPING_ROLE_TO_PROPERTYNAME[findRoleParameter.OrderBy], findRoleParameter.Order);
            }

            int totalLength = result.Count();
            result = result.Skip(findRoleParameter.StartIndex).Take(findRoleParameter.Count);
            return Task.FromResult(new FindResponse<RoleAggregate>
            {
                StartIndex = findRoleParameter.StartIndex,
                Count = findRoleParameter.Count,
                TotalLength = totalLength,
                Content = result.ToList()
            });
        }
    }
}
