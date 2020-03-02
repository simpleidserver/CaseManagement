using CaseManagement.Gateway.Website.Role.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CaseManagement.Gateway.Website.Role.CommandHandlers
{
    public interface IUpdateRoleCommandHandler
    {
        Task Handle(UpdateRoleCommand command);
    }
}
