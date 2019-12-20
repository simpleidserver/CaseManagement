using CaseManagement.Workflow.Infrastructure;
using System;
using System.Diagnostics;

namespace CaseManagement.CMMN.Domains.Events
{
    [DebuggerDisplay("Create element {WorkflowElementDefinitionType} instance {ElementId}")]
    public class CMMNWorkflowElementCreatedEvent : DomainEvent
    {
        public CMMNWorkflowElementCreatedEvent(string id, string aggregateId, int version, string elementId, string workflowElementDefinitionId, CMMNWorkflowElementTypes workflowElementDefinitionType, DateTime createDateTime) : base(id, aggregateId, version)
        {
            ElementId = elementId;
            WorkflowElementDefinitionId = workflowElementDefinitionId;
            WorkflowElementDefinitionType = workflowElementDefinitionType;
            CreateDateTime = createDateTime;
        }

        public string ElementId { get; set; }
        public string WorkflowElementDefinitionId { get; set; }
        public CMMNWorkflowElementTypes WorkflowElementDefinitionType { get; set; }
        public DateTime CreateDateTime { get; set; }
    }
}
