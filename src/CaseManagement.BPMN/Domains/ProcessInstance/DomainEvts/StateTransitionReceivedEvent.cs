using CaseManagement.Common.Domains;
using System;
using System.Diagnostics;

namespace CaseManagement.BPMN.Domains
{
    [DebuggerDisplay("Add state transition")]
    [Serializable]
    public class StateTransitionReceivedEvent : DomainEvent
    {
        public StateTransitionReceivedEvent(
            string id, 
            string aggregateId, 
            int version,
            string flowNodeInstanceId,
            string state,
            string content,
            DateTime updateDateTime) : base(id, aggregateId, version)
        {
            FlowNodeInstanceId = flowNodeInstanceId;
            State = state;
            Content = content;
            UpdateDateTime = updateDateTime;
        }

        public string FlowNodeInstanceId { get; set; }
        public string State { get; set; }
        public string Content { get; set; }
        public DateTime UpdateDateTime { get; set; }
    }
}
