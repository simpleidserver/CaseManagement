using CaseManagement.Workflow.Infrastructure;
using System;

namespace CaseManagement.Workflow.Domains.Events
{
    public class ProcessFlowElementCompletedEvent : DomainEvent
    {
        public ProcessFlowElementCompletedEvent(string processFlowInstanceId, string processFlowInstanceElementId, DateTime completedDateTime)
        {
            ProcessFlowInstanceId = processFlowInstanceId;
            ProcessFlowInstanceElementId = processFlowInstanceElementId;
            CompletedDateTime = completedDateTime;
        }

        public string ProcessFlowInstanceId { get; set; }
        public string ProcessFlowInstanceElementId { get; set; }
        public DateTime CompletedDateTime { get; set; }
    }
}
