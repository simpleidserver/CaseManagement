using CaseManagement.Common.Domains;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CaseManagement.CMMN.Domains
{
    [DebuggerDisplay("Update role '{RoleId}'")]
    [Serializable]
    public class CaseInstanceRoleUpdatedEvent : DomainEvent
    {
        public CaseInstanceRoleUpdatedEvent(string id, string aggregateId, int version, string roleId, ICollection<KeyValuePair<string, string>> claims, DateTime updateDateTime) : base(id, aggregateId, version)
        {
            RoleId = roleId;
            Claims = claims;
            UpdateDateTime = updateDateTime;
        }

        public string RoleId { get; set; }
        public ICollection<KeyValuePair<string, string>> Claims { get; set; }
        public DateTime UpdateDateTime { get; set; }
    }
}
