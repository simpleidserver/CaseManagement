using CaseManagement.Common.Domains;
using System;
using System.Diagnostics;

namespace CaseManagement.BPMN.Domains
{
    [DebuggerDisplay("Update activity state")]
    public class ActivityStateUpdatedEvent : DomainEvent
    {
        public ActivityStateUpdatedEvent(string id, string aggregateId, int version, string flowNodeInstanceId, ActivityStates state, DateTime updateDateTime) : base(id, aggregateId, version)
        {
            FlowNodeInstanceId = flowNodeInstanceId;
            State = state;
            UpdateDateTime = updateDateTime;
        }

        public string FlowNodeInstanceId { get; set; }
        public ActivityStates State { get; set; }
        public DateTime UpdateDateTime { get; set; }
    }
}
