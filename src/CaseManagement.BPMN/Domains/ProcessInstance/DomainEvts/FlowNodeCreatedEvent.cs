using CaseManagement.Common.Domains;
using System;
using System.Diagnostics;

namespace CaseManagement.BPMN.Domains
{
    [DebuggerDisplay("Add node '{NodeType}'")]
    public class FlowNodeCreatedEvent : DomainEvent
    {
        public FlowNodeCreatedEvent(string id, string aggregateId, int version, string nodeType, string serializedContent, DateTime updateDateTime) : base(id, aggregateId, version)
        {
            NodeType = nodeType;
            SerializedContent = serializedContent;
            UpdateDateTime = updateDateTime;
        }

        public string NodeType { get; set; }
        public string SerializedContent { get; set; }
        public DateTime UpdateDateTime { get; set; }
    }
}
