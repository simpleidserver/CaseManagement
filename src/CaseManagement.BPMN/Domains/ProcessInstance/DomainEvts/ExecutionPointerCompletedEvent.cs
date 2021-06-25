using CaseManagement.Common.Domains;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CaseManagement.BPMN.Domains
{
    [DebuggerDisplay("Complete execution pointer")]
    [Serializable]
    public class ExecutionPointerCompletedEvent : DomainEvent
    {
        public ExecutionPointerCompletedEvent(string id, string aggregateId, int version, string executionPathId, string executionPointerId, ICollection<MessageToken> outcomeValues, DateTime completionDateTime) : base(id, aggregateId, version)
        {
            ExecutionPathId = executionPathId;
            ExecutionPointerId = executionPointerId;
            OutcomeValues = outcomeValues;
            CompletionDateTime = completionDateTime;
        }

        public string ExecutionPathId { get; set; }
        public string ExecutionPointerId { get; set; }
        public ICollection<MessageToken> OutcomeValues { get; set; }
        public DateTime CompletionDateTime { get; set; }
    }
}
