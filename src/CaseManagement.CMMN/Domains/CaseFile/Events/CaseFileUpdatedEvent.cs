using CaseManagement.Common.Domains;
using System;
using System.Diagnostics;

namespace CaseManagement.CMMN.Domains
{
    [DebuggerDisplay("Case file '{AggregateId}' is updated")]
    [Serializable]
    public class CaseFileUpdatedEvent : DomainEvent
    {
        public CaseFileUpdatedEvent(string id, string aggregateId, int version, DateTime updateDatetime, string name, string description) : base(id, aggregateId, version)
        {
            UpdateDatetime = updateDatetime;
            Name = name;
            Description = description;
        }

        public DateTime UpdateDatetime { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
