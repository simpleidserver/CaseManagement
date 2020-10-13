using CaseManagement.Common.Domains;
using System;
using System.Diagnostics;

namespace CaseManagement.HumanTask.Domains
{
    [DebuggerDisplay("Create the human task def '{Name}'")]
    public class HumanTaskDefCreatedEvent : DomainEvent
    {
        public HumanTaskDefCreatedEvent(string id, string aggregateId, int version, string name, DateTime createDateTime) : base(id, aggregateId, version)
        {
            Name = name;
            CreateDateTime = createDateTime;
        }

        public string Name { get; set; }
        public DateTime CreateDateTime { get; set; }
    }
}
