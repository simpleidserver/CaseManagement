using CaseManagement.Common.Domains;
using System;
using System.Diagnostics;

namespace CaseManagement.BPMN.Domains
{
    [DebuggerDisplay("Update activity state")]
    [Serializable]
    public class ActivityStateUpdatedEvent : DomainEvent
    {
        public ActivityStateUpdatedEvent(string id, string aggregateId, int version, string flowNodeInstanceId, ActivityStates state, string message, DateTime updateDateTime) : base(id, aggregateId, version)
        {
            FlowNodeInstanceId = flowNodeInstanceId;
            State = state;
            Message = message;
            UpdateDateTime = updateDateTime;
        }

        public string FlowNodeInstanceId { get; set; }
        public ActivityStates State { get; set; }
        public string Message { get; set; }
        public DateTime UpdateDateTime { get; set; }
    }
}
