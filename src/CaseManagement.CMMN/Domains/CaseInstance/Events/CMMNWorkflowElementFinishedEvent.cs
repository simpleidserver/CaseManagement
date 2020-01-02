using CaseManagement.Workflow.Infrastructure;
using System;
using System.Diagnostics;

namespace CaseManagement.CMMN.Domains.Events
{
    [DebuggerDisplay("Finish the element {ElementDefinitionId}")]
    public class CMMNWorkflowElementFinishedEvent : DomainEvent
    {
        public CMMNWorkflowElementFinishedEvent(string id, string aggregateId, int version, string elementDefinitionId, DateTime endDateTime) : base(id, aggregateId, version)
        {
            ElementDefinitionId = elementDefinitionId;
            EndDateTime = endDateTime;
        }

        public string ElementDefinitionId { get; set; }
        public DateTime EndDateTime { get; set; }
    }
}
