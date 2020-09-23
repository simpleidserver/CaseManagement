using CaseManagement.Common.Domains;
using System;
using System.Diagnostics;

namespace CaseManagement.BPMN.Domains
{
    [DebuggerDisplay("Complete execution pointer")]
    public class ExecutionPointerCompletedEvent : DomainEvent
    {
        public ExecutionPointerCompletedEvent(string id, string aggregateId, int version, string executionPathId, string executionPointerId, DateTime completionDateTime) : base(id, aggregateId, version)
        {
            ExecutionPathId = executionPathId;
            ExecutionPointerId = executionPointerId;
            CompletionDateTime = completionDateTime;
        }

        public string ExecutionPathId { get; set; }
        public string ExecutionPointerId { get; set; }
        public DateTime CompletionDateTime { get; set; }
    }
}
