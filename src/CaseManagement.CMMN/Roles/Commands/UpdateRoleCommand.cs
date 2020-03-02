using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CaseManagement.CMMN.Roles.Commands
{
    [DataContract]
    public class UpdateRoleCommand
    {
        public string Role { get; set; }
        [DataMember(Name = "users")]
        public ICollection<string> UserIds { get; set; }
    }
}
