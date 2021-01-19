using CaseManagement.Common.Domains;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CaseManagement.CMMN.Domains
{
    [DebuggerDisplay("Consume ONPart event")]
    [Serializable]
    public class OnPartEvtConsumedEvent : DomainEvent
    {
        public OnPartEvtConsumedEvent(string id, string aggregateId, int version, string elementId, string sourceElementId, ICollection<IncomingTransition> transitions) : base(id, aggregateId, version)
        {
            ElementId = elementId;
            SourceElementId = sourceElementId;
            Transitions = transitions;
        }

        public string ElementId { get; set; }
        public string SourceElementId { get; set; }
        public ICollection<IncomingTransition> Transitions { get; set; }
    }


    [Serializable]
    public class IncomingTransition
    {
        public IncomingTransition(CMMNTransitions transition, Dictionary<string, string> incomingTokens)
        {
            Transition = transition;
            IncomingTokens = incomingTokens;
        }

        public CMMNTransitions Transition { get; set; }
        public Dictionary<string, string> IncomingTokens { get; set; }
    }
}
