using CaseManagement.Gateway.Website.Role.DTOs;
using CaseManagement.Gateway.Website.Role.Queries;
using System.Threading.Tasks;

namespace CaseManagement.Gateway.Website.Role.QueryHandlers
{
    public interface IGetRoleQueryHandler
    {
        Task<RoleResponse> Handle(GetRoleQuery query);
    }
}