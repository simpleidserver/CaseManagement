using System.Runtime.Serialization;

namespace CaseManagement.CMMN.Roles.Commands
{
    [DataContract]
    public class AddRoleCommand
    {
        [DataMember(Name = "role")]
        public string Role { get; set; }
    }
}
