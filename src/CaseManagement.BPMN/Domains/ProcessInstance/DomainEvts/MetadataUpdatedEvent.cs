using CaseManagement.Common.Domains;
using System;
using System.Diagnostics;

namespace CaseManagement.BPMN.Domains
{
    [DebuggerDisplay("Update metadata '{Key}'='{Value}'")]
    public class MetadataUpdatedEvent : DomainEvent
    {
        public MetadataUpdatedEvent(string id, string aggregateId, int version, string flowNodeInstanceId, string key, string value, DateTime updateDateTime) : base(id, aggregateId, version)
        {
            FlowNodeInstanceId = flowNodeInstanceId;
            Key = key;
            Value = value;
            UpdateDateTime = updateDateTime;
        }

        public string FlowNodeInstanceId { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public DateTime UpdateDateTime { get; set; }
    }
}
