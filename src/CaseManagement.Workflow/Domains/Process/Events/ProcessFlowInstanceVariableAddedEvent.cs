using CaseManagement.Workflow.Infrastructure;

namespace CaseManagement.Workflow.Domains.Events
{
    public class ProcessFlowInstanceVariableAddedEvent : DomainEvent
    {
        public ProcessFlowInstanceVariableAddedEvent(string processFlowInstanceId, string key, string value)
        {
            ProcessFlowInstanceId = processFlowInstanceId;
            Key = key;
            Value = value;
        }

        public string ProcessFlowInstanceId { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
