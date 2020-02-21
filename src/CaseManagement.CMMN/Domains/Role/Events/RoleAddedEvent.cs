using CaseManagement.CMMN.Infrastructures;
using System;
using System.Diagnostics;

namespace CaseManagement.CMMN.Domains.Role.Events
{
    [DebuggerDisplay("Role {AggregateId} is added")]
    public class RoleAddedEvent : DomainEvent
    {
        public RoleAddedEvent(string id, string aggregateId, int version, DateTime createDateTime) : base(id, aggregateId, version)
        {
            CreateDateTime = createDateTime;
        }

        public DateTime CreateDateTime { get; set; }
    }
}
