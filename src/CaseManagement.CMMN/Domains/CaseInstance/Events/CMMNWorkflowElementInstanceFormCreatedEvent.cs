using CaseManagement.Workflow.Infrastructure;
using System.Diagnostics;

namespace CaseManagement.CMMN.Domains.Events
{
    [DebuggerDisplay("Create form {FormId} for element {ElementId} (Role = {PerformerRef})")]
    public class CMMNWorkflowElementInstanceFormCreatedEvent : DomainEvent
    {
        public CMMNWorkflowElementInstanceFormCreatedEvent(string id, string aggregateId, int version, string elementId, string formInstanceId, string formId, string perfomerRef) : base(id, aggregateId, version)
        {
            ElementId = elementId;
            FormInstanceId = formInstanceId;
            FormId = formId;
            PerformerRef = perfomerRef;
        }

        public string ElementId { get; set; }
        public string FormInstanceId { get; set; }
        public string FormId { get; set; }
        public string PerformerRef { get; set; }
    }
}
