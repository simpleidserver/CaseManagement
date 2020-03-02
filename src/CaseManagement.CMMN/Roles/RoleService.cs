using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Extensions;
using CaseManagement.CMMN.Persistence;
using CaseManagement.CMMN.Persistence.Parameters;
using CaseManagement.CMMN.Persistence.Responses;
using CaseManagement.CMMN.Roles.CommandHandlers;
using CaseManagement.CMMN.Roles.Commands;
using CaseManagement.CMMN.Roles.Exceptions;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Roles
{
    public class RoleService : IRoleService
    {
        private readonly IRoleQueryRepository _roleQueryRepository;
        private readonly IUpdateRoleCommandHandler _updateRoleCommandHandler;
        private readonly IDeleteRoleCommandHandler _deleteRoleCommandHandler;
        private readonly IAddRoleCommandHandler _addRoleCommandHandler;

        public RoleService(IRoleQueryRepository roleQueryRepository, IUpdateRoleCommandHandler updateRoleCommandHandler, IDeleteRoleCommandHandler deleteRoleCommandHandler, IAddRoleCommandHandler addRoleCommandHandler)
        {
            _roleQueryRepository = roleQueryRepository;
            _updateRoleCommandHandler = updateRoleCommandHandler;
            _deleteRoleCommandHandler = deleteRoleCommandHandler;
            _addRoleCommandHandler = addRoleCommandHandler;
        }

        public async Task<JObject> Get(string id)
        {
            var result = await _roleQueryRepository.FindById(id);
            if (result == null)
            {
                throw new UnknownRoleException(id);
            }

            return ToDto(result);
        }

        public async Task<JObject> Add(AddRoleCommand addRoleCommand)
        {
            var result = await _addRoleCommandHandler.Handle(addRoleCommand);
            return ToDto(result);
        }

        public async Task<JObject> Search(IEnumerable<KeyValuePair<string, string>> query)
        {
            var result = await _roleQueryRepository.Search(ExtractFindParameter(query));
            return ToDto(result);
        }

        public Task<IEnumerable<RoleAggregate>> FindRolesByUser(string user)
        {
            return _roleQueryRepository.FindRolesByUser(user);
        }

        public Task Update(UpdateRoleCommand updateRoleCommand)
        {
            return _updateRoleCommandHandler.Handle(updateRoleCommand);
        }

        public Task Delete(DeleteRoleCommand deleteRoleCommand)
        {
            return _deleteRoleCommandHandler.Handle(deleteRoleCommand);
        }

        private static FindRoleParameter ExtractFindParameter(IEnumerable<KeyValuePair<string, string>> query)
        {
            bool isDeleted = false;
            var parameter = new FindRoleParameter();
            parameter.ExtractFindParameter(query);
            if (query.TryGet("is_deleted", out isDeleted))
            {
                parameter.IsDeleted = isDeleted;
            }

            return parameter;
        }
        
        private static JObject ToDto(FindResponse<RoleAggregate> resp)
        {
            return new JObject
            {
                { "start_index", resp.StartIndex },
                { "total_length", resp.TotalLength },
                { "count", resp.Count },
                { "content", new JArray(resp.Content.Select(r => ToDto(r))) }
            };
        }

        private static JObject ToDto(RoleAggregate role)
        {
            var result = new JObject
            {
                { "id", role.Id },
                { "is_deleted", role.IsDeleted },
                { "users", new JArray(role.UserIds) },
                { "update_datetime", role.UpdateDateTime },
                { "create_datetime", role.CreateDateTime }
            };

            return result;
        }
    }
}
