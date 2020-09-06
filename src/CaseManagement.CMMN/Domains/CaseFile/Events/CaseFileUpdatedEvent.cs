using CaseManagement.CMMN.Infrastructures;
using System;
using System.Diagnostics;

namespace CaseManagement.CMMN.Domains.Events
{
    [DebuggerDisplay("Case file '{AggregateId}' is updated")]
    public class CaseFileUpdatedEvent : DomainEvent
    {
        public CaseFileUpdatedEvent(string id, string aggregateId, int version, DateTime updateDatetime, string name, string description, string payload) : base(id, aggregateId, version)
        {
            UpdateDatetime = updateDatetime;
            Name = name;
            Description = description;
            Payload = payload;
        }

        public DateTime UpdateDatetime { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Payload { get; set; }
    }
}