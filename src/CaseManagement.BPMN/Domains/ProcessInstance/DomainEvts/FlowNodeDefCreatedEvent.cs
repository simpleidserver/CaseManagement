using CaseManagement.Common.Domains;
using System;
using System.Diagnostics;

namespace CaseManagement.BPMN.Domains
{
    [DebuggerDisplay("Add flow node definition '{NodeType}'")]
    public class FlowNodeDefCreatedEvent : DomainEvent
    {
        public FlowNodeDefCreatedEvent(string id, string aggregateId, int version, string serializedContent, DateTime updateDateTime) : base(id, aggregateId, version)
        {
            SerializedContent = serializedContent;
            UpdateDateTime = updateDateTime;
        }

        public string SerializedContent { get; set; }
        public DateTime UpdateDateTime { get; set; }
    }
}
