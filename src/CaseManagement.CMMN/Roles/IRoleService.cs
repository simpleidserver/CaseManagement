using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Roles.Commands;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Roles
{
    public interface IRoleService
    {
        Task<JObject> Get(string role);
        Task<JObject> Add(AddRoleCommand addRoleCommand);
        Task<JObject> Search(IEnumerable<KeyValuePair<string, string>> query);
        Task<IEnumerable<RoleAggregate>> FindRolesByUser(string user);
        Task Update(UpdateRoleCommand updateRoleCommand);
        Task Delete(DeleteRoleCommand deleteRoleCommand);
    }
}
