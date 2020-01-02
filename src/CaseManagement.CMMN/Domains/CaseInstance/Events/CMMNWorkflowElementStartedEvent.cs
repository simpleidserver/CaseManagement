using CaseManagement.Workflow.Infrastructure;
using System;
using System.Diagnostics;

namespace CaseManagement.CMMN.Domains.Events
{
    [DebuggerDisplay("Start the element {ElementDefinitionId}")]
    public class CMMNWorkflowElementStartedEvent : DomainEvent
    {
        public CMMNWorkflowElementStartedEvent(string id, string aggregateId, int version, string elementDefinitionId, DateTime startDateTime ) : base(id, aggregateId, version)
        {
            ElementDefinitionId = elementDefinitionId;
            StartDateTime = startDateTime;
        }

        public string ElementDefinitionId { get; set; }
        public DateTime StartDateTime { get; set; }
    }
}
