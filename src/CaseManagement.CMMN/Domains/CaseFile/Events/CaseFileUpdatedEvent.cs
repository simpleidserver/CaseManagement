using CaseManagement.CMMN.Infrastructures;
using System;
using System.Diagnostics;

namespace CaseManagement.CMMN.Domains.Events
{
    [DebuggerDisplay("Case file {AggregateId} is updated by {Performer}")]
    public class CaseFileUpdatedEvent : DomainEvent
    {
        public CaseFileUpdatedEvent(string id, string aggregateId, int version, DateTime updateDatetime, string name, string description, string payload, string performer) : base(id, aggregateId, version)
        {
            UpdateDatetime = updateDatetime;
            Name = name;
            Description = description;
            Payload = payload;
            Performer = performer;
        }

        public DateTime UpdateDatetime { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Payload { get; set; }
        public string Performer { get; set; }
    }
}