using CaseManagement.Common.Domains;
using System;
using System.Diagnostics;

namespace CaseManagement.BPMN.Domains
{
    [DebuggerDisplay("Create execution path")]
    [Serializable]
    public class ExecutionPathCreatedEvent : DomainEvent
    {
        public ExecutionPathCreatedEvent(string id, string aggregateId, int version, string executionPathId, DateTime createDateTime) : base(id, aggregateId, version)
        {
            ExecutionPathId = executionPathId;
            CreateDateTime = createDateTime;
        }

        public string ExecutionPathId { get; set; }
        public DateTime CreateDateTime { get; set; }
    }
}
