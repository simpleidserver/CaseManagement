using System;

namespace CaseManagement.Workflow.Domains
{
    public class ProcessFlowInstanceElementFormElement : ICloneable
    {
        public string FormElementId { get; set; }
        public string Value { get; set; }

        public object Clone()
        {
            return new ProcessFlowInstanceElementFormElement
            {
                FormElementId = FormElementId,
                Value = Value
            };
        }
    }
}
