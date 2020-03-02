using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CaseManagement.Gateway.Website.Role.DTOs
{
    [DataContract]
    public class UpdateRoleParameter
    {
        [DataMember(Name = "users")]
        public ICollection<string> Users { get; set; }
    }
}
