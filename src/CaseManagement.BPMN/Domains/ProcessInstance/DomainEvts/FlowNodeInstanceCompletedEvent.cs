using CaseManagement.Common.Domains;
using System;
using System.Diagnostics;

namespace CaseManagement.BPMN.Domains
{
    [DebuggerDisplay("Flow node instance is complete")]
    public class FlowNodeInstanceCompletedEvent : DomainEvent
    {
        public FlowNodeInstanceCompletedEvent(string id, string aggregateId, int version, string flowNodeInstanceId, DateTime executionDateTime) : base(id, aggregateId, version)
        {
            FlowNodeInstanceId = flowNodeInstanceId;
            ExecutionDateTime = executionDateTime;
        }

        public string FlowNodeInstanceId { get; set; }
        public DateTime ExecutionDateTime { get; set; }
    }
}
