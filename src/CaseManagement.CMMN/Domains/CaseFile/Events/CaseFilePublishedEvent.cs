using CaseManagement.CMMN.Infrastructures;
using System.Diagnostics;

namespace CaseManagement.CMMN.Domains.Events
{
    [DebuggerDisplay("Case file {AggregateId} is published by {Performer}")]
    public class CaseFilePublishedEvent : DomainEvent
    {
        public CaseFilePublishedEvent(string id, string aggregateId, int version, string performer) : base(id, aggregateId, version)
        {
            Performer = performer;
        }

        public string Performer { get; set; }
    }
}
