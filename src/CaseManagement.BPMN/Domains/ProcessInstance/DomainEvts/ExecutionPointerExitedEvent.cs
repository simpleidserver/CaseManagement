using CaseManagement.Common.Domains;
using System;
using System.Diagnostics;

namespace CaseManagement.BPMN.Domains
{
    [DebuggerDisplay("Complete execution pointer")]
    [Serializable]
    public class ExecutionPointerExitedEvent : DomainEvent
    {
        public ExecutionPointerExitedEvent(string id, string aggregateId, int version, string executionPathId, string executionPointerId, DateTime exitDateTime) : base(id, aggregateId, version)
        {
            ExecutionPathId = executionPathId;
            ExecutionPointerId = executionPointerId;
            ExitDateTime = exitDateTime;
        }

        public string ExecutionPathId { get; set; }
        public string ExecutionPointerId { get; set; }
        public DateTime ExitDateTime { get; set; }
    }
}
