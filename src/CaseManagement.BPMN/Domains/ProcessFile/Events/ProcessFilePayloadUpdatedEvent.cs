using CaseManagement.Common.Domains;
using System;
using System.Diagnostics;

namespace CaseManagement.BPMN.Domains
{
    [DebuggerDisplay("Update payload")]
    public class ProcessFilePayloadUpdatedEvent : DomainEvent
    {
        public ProcessFilePayloadUpdatedEvent(string id, string aggregateId, int version, string payload, DateTime updateDateTime) : base(id, aggregateId, version)
        {
            Payload = payload;
            UpdateDateTime = updateDateTime;
        }

        public string Payload { get; set; }
        public DateTime UpdateDateTime { get; set; }
    }
}
