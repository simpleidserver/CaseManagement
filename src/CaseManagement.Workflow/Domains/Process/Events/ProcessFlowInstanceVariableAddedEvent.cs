using CaseManagement.Workflow.Infrastructure;

namespace CaseManagement.Workflow.Domains.Events
{
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
