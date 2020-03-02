using System.Collections.Generic;

namespace CaseManagement.Gateway.Website.Role.Commands
{
    public class UpdateRoleCommand
    {
        public string Role { get; set; }
        public ICollection<string> Users { get; set; }
    }
}
