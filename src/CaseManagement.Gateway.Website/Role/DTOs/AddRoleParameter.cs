using System.Runtime.Serialization;

namespace CaseManagement.Gateway.Website.Role.DTOs
{
    [DataContract]
    public class AddRoleParameter
    {
        [DataMember(Name = "role")]
        public string Role { get; set; }
    }
}
