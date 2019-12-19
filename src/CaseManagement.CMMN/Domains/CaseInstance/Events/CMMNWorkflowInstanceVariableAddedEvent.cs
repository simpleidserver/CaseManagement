using CaseManagement.Workflow.Infrastructure;

namespace CaseManagement.CMMN.Domains.Events
{
    public class CMMNWorkflowInstanceVariableAddedEvent : DomainEvent
    {
        public CMMNWorkflowInstanceVariableAddedEvent(string id, string aggregateId, int version, string key, string value) : base(id, aggregateId, version)
        {
            Key = key;
            Value = value;
        }

        public string Key { get; set; }
        public string Value { get; set; }
    }
}
