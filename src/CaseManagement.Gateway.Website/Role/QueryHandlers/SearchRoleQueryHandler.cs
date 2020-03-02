using CaseManagement.Gateway.Website.Role.DTOs;
using CaseManagement.Gateway.Website.Role.Queries;
using CaseManagement.Gateway.Website.Role.Services;
using System.Linq;
using System.Threading.Tasks;

namespace CaseManagement.Gateway.Website.Role.QueryHandlers
{
    public class SearchRoleQueryHandler : ISearchRoleQueryHandler
    {
        private readonly IRoleService _roleService;

        public SearchRoleQueryHandler(IRoleService roleService)
        {
            _roleService = roleService;
        }

        public Task<FindResponse<RoleResponse>> Handle(SearchRoleQuery query)
        {
            return _roleService.Search(query.Queries.ToList());
        }
    }
}
