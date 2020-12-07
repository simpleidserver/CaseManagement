using CaseManagement.Common.Domains;
using System;
using System.Diagnostics;

namespace CaseManagement.BPMN.Domains
{
    [DebuggerDisplay("Add flow node instance")]
    [Serializable]
    public class FlowNodeInstanceAddedEvent : DomainEvent
    {
        public FlowNodeInstanceAddedEvent(string id, string aggregateId, int version, string flowNodeInstanceId, string flowNodeId, DateTime createDateTime) : base(id, aggregateId, version)
        {
            FlowNodeInstanceId = flowNodeInstanceId;
            FlowNodeId = flowNodeId;
            CreateDateTime = createDateTime;
        }

        public string FlowNodeInstanceId { get; set; }
        public string FlowNodeId { get; set; }
        public DateTime CreateDateTime { get; set; }
    }
}
