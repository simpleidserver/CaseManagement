using CaseManagement.Common.Domains;
using System;
using System.Diagnostics;

namespace CaseManagement.CMMN.Domains
{
    [DebuggerDisplay("Case file payload '{AggregateId}' is updated")]
    [Serializable]
    public class CaseFilePayloadUpdatedEvent : DomainEvent
    {
        public CaseFilePayloadUpdatedEvent(string id, string aggregateId, int version, DateTime updateDatetime, string payload) : base(id, aggregateId, version)
        {
            UpdateDatetime = updateDatetime;
            Payload = payload;
        }

        public DateTime UpdateDatetime { get; set; }
        public string Payload { get; set; }
    }
}
