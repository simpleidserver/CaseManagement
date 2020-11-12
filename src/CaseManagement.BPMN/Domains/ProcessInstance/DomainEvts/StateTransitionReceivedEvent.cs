using CaseManagement.BPMN.Common;
using CaseManagement.Common.Domains;
using System;
using System.Diagnostics;

namespace CaseManagement.BPMN.Domains
{
    [DebuggerDisplay("Add state transition")]
    public class StateTransitionReceivedEvent : DomainEvent
    {
        public StateTransitionReceivedEvent(string id, string aggregateId, int version, StateTransitionToken stateTransitionToken, DateTime updateDateTime) : base(id, aggregateId, version)
        {
            StateTransitionToken = stateTransitionToken;
            UpdateDateTime = updateDateTime;
        }

        public StateTransitionToken StateTransitionToken { get; set; }
        public DateTime UpdateDateTime { get; set; }
    }
}
