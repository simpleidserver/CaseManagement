using CaseManagement.Gateway.Website.Role.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CaseManagement.Gateway.Website.Role.Services
{
    public interface IRoleService
    {
        Task<RoleResponse> Add(AddRoleParameter parameter);
        Task Update(string role, UpdateRoleParameter parameter);
        Task<RoleResponse> Get(string role);
        Task Delete(string role);
        Task<FindResponse<RoleResponse>> Search(IEnumerable<KeyValuePair<string, string>> queries);
    }
}
