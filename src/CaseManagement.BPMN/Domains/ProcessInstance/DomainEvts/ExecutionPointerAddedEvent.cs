using CaseManagement.Common.Domains;
using System;
using System.Diagnostics;

namespace CaseManagement.BPMN.Domains
{
    [DebuggerDisplay("Add execution pointer")]
    [Serializable]
    public class ExecutionPointerAddedEvent : DomainEvent
    {
        public ExecutionPointerAddedEvent(string id, string aggregateId, int version, string executionPointerId, string executionPathId, string flowNodeInstanceId, string flowNodeId, string serializedTokens, DateTime createDateTime) : base(id, aggregateId, version)
        {
            ExecutionPointerId = executionPointerId;
            ExecutionPathId = executionPathId;
            FlowNodeInstanceId = flowNodeInstanceId;
            FlowNodeId = flowNodeId;
            SerializedTokens = serializedTokens;
            CreateDateTime = createDateTime;
        }

        public string ExecutionPointerId { get; set; }
        public string ExecutionPathId { get; set; }
        public string FlowNodeInstanceId { get; set; }
        public string FlowNodeId { get; set; }
        public string SerializedTokens { get; set; }
        public DateTime CreateDateTime { get; set; }
    }
}
