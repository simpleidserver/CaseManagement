using CaseManagement.Common.Domains;
using System;
using System.Diagnostics;

namespace CaseManagement.HumanTask.Domains
{
    [DebuggerDisplay("Notification definition is added")]
    public class NotificationDefinitionAddedEvent : DomainEvent
    {
        public NotificationDefinitionAddedEvent(string id, string aggregateId, int version, string name, DateTime createDateTime) : base(id, aggregateId, version)
        {
            Name = name;
            CreateDateTime = createDateTime;
        }

        public string Name { get; set; }
        public DateTime CreateDateTime { get; set; }
    }
}
