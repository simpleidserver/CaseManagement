using CaseManagement.Common.Domains;
using System;
using System.Diagnostics;

namespace CaseManagement.BPMN.Domains
{
    [DebuggerDisplay("Add incoming token")]
    [Serializable]
    public class IncomingTokenAddedEvent : DomainEvent
    {
        public IncomingTokenAddedEvent(string id, string aggregateId, int version, string executionPathId, string executionPointerId, string serializedToken, DateTime creationDateTime) : base(id, aggregateId, version)
        {
            ExecutionPathId = executionPathId;
            ExecutionPointerId = executionPointerId;
            SerializedToken = serializedToken;
            CreateDateTime = creationDateTime;
        }

        public string ExecutionPathId { get; set; }
        public string ExecutionPointerId { get; set; }
        public string SerializedToken { get; set; }
        public DateTime CreateDateTime { get; set; }
    }
}
