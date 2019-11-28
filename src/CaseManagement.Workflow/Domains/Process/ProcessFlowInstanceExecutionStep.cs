using System;

namespace CaseManagement.Workflow.Domains
{
    public class ProcessFlowInstanceExecutionStep : ICloneable
    {
        public ProcessFlowInstanceExecutionStep(ProcessFlowInstanceElement element, DateTime startDateTime)
        {
            Element = element;
            StartDateTime = startDateTime;
        }

        public ProcessFlowInstanceElement Element { get; private set; }
        public DateTime StartDateTime { get; private set; }
        public DateTime? EndDateTime { get; set; }

        public object Clone()
        {
            return new ProcessFlowInstanceExecutionStep((ProcessFlowInstanceElement)Element.Clone(), StartDateTime)
            {
                EndDateTime = EndDateTime
            };
        }
    }
}
