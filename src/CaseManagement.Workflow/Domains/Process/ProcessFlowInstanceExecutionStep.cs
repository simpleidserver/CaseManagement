using System;

namespace CaseManagement.Workflow.Domains
{
    public class ProcessFlowInstanceExecutionStep : ICloneable
    {
        public ProcessFlowInstanceExecutionStep(string elementId, string elementName, DateTime startDateTime)
        {
            ElementId = elementId;
            ElementName = elementName;
            StartDateTime = startDateTime;
        }

        public string ElementId { get; private set; }
        public string ElementName { get; private set; }
        public DateTime StartDateTime { get; private set; }
        public DateTime? EndDateTime { get; set; }
        public string ErrorMessage { get; set; }

        public object Clone()
        {
            return new ProcessFlowInstanceExecutionStep(ElementId, ElementName, StartDateTime)
            {
                EndDateTime = EndDateTime,
                ErrorMessage = ErrorMessage
            };
        }
    }
}
