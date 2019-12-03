using CaseManagement.Workflow.Infrastructure;
using System;

namespace CaseManagement.Workflow.Domains.Events
{
    public class ProcessFlowElementStartedEvent : DomainEvent
    {
        public ProcessFlowElementStartedEvent(string processFlowInstanceId, string processFlowInstanceElementId, DateTime startDateTime)
        {
            ProcessFlowInstanceId = processFlowInstanceId;
            ProcessFlowInstanceElementId = processFlowInstanceElementId;
            StartDateTime = startDateTime;
        }
        
        public string ProcessFlowInstanceId { get; set; }
        public string ProcessFlowInstanceElementId { get; set; }
        public DateTime StartDateTime { get; set; }
    }
}
