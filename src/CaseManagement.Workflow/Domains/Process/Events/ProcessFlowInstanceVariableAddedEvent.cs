using CaseManagement.Workflow.Infrastructure;
using System.Diagnostics;

namespace CaseManagement.Workflow.Domains.Events
{
    [DebuggerDisplay("{Key}={Value} added")]
    public class ProcessFlowInstanceVariableAddedEvent : DomainEvent
    {
        public ProcessFlowInstanceVariableAddedEvent(string id, string aggregateId, int version, string key, string value) : base(id, aggregateId, version)
        {
            Key = key;
            Value = value;
        }

        public string Key { get; set; }
        public string Value { get; set; }
    }
}
