using CaseManagement.Workflow.Infrastructure;
using System.Diagnostics;

namespace CaseManagement.CMMN.Domains.Events
{
    [DebuggerDisplay("Submit form {FormInstanceId}")]
    public class CMMNWorkflowElementInstanceFormSubmittedEvent : DomainEvent
    {
        public CMMNWorkflowElementInstanceFormSubmittedEvent(string id, string aggregateId, int version, string elementId, string formInstanceId) : base(id, aggregateId, version)
        {
            ElementId = elementId;
            FormInstanceId = formInstanceId;
        }

        public string ElementId { get; set; }
        public string FormInstanceId { get; set; }
    }
}
