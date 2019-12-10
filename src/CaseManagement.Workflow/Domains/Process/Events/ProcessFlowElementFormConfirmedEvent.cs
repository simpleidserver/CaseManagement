using CaseManagement.Workflow.Infrastructure;
using System.Collections.Generic;
using System.Diagnostics;

namespace CaseManagement.Workflow.Domains.Events
{
    [DebuggerDisplay("Confirm form instance {ElementId}")]
    public class ProcessFlowElementFormConfirmedEvent : DomainEvent
    {
        public ProcessFlowElementFormConfirmedEvent(string id, string aggregateId, int version, string elementId, string formInstanceId, string formId, Dictionary<string, string> formContent) : base(id, aggregateId, version)
        {
            ElementId = elementId;
            FormInstanceId = formInstanceId;
            FormId = formId;
            FormContent = formContent;
        }

        public string ElementId { get; set; }
        public string FormInstanceId { get; set; }
        public string FormId { get; set; }
        public Dictionary<string, string> FormContent { get; set; }
    }
}
