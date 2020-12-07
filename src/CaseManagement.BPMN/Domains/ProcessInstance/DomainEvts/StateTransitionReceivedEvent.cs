using CaseManagement.BPMN.Infrastructure.Jobs.Notifications;
using CaseManagement.Common.Domains;
using System;
using System.Diagnostics;

namespace CaseManagement.BPMN.Domains
{
    [DebuggerDisplay("Add state transition")]
    [Serializable]
    public class StateTransitionReceivedEvent : DomainEvent
    {
        public StateTransitionReceivedEvent(string id, string aggregateId, int version, StateTransitionNotification stateTransitionToken, DateTime updateDateTime) : base(id, aggregateId, version)
        {
            StateTransitionToken = stateTransitionToken;
            UpdateDateTime = updateDateTime;
        }

        public StateTransitionNotification StateTransitionToken { get; set; }
        public DateTime UpdateDateTime { get; set; }
    }
}
