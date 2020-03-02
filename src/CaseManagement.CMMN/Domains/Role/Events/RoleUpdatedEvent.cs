using CaseManagement.CMMN.Infrastructures;
using System;
using System.Collections.Generic;

namespace CaseManagement.CMMN.Domains.Role.Events
{
    public class RoleUpdatedEvent : DomainEvent
    {
        public RoleUpdatedEvent(string id, string aggregateId, int version, ICollection<string> users, DateTime updateDateTime) : base(id, aggregateId, version)
        {
            Users = users;
            UpdateDateTime = updateDateTime;
        }

        public ICollection<string> Users { get; set; }
        public DateTime UpdateDateTime { get; set; }
    }
}
