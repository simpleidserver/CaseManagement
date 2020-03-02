using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CaseManagement.Gateway.Website.Role.DTOs
{
    [DataContract]
    public class RoleResponse
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }
        [DataMember(Name = "users")]
        public ICollection<string> Users { get; set; }
        [DataMember(Name = "is_deleted")]
        public bool IsDeleted { get; set; }
        [DataMember(Name = "create_datetime")]
        public DateTime CreateDateTime { get; set; }
        [DataMember(Name = "update_datetime")]
        public DateTime UpdateDateTime { get; set; }
    }
}
