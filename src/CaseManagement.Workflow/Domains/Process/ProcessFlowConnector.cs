using System;

namespace CaseManagement.Workflow.Domains
{
    public class ProcessFlowConnector : ICloneable
    {
        public ProcessFlowConnector(ProcessFlowInstanceElement source, ProcessFlowInstanceElement target)
        {
            Source = source;
            Target = target;
        }

        public ProcessFlowInstanceElement Source { get; set; }
        public ProcessFlowInstanceElement Target { get; set; }

        public object Clone()
        {
            return new ProcessFlowConnector((ProcessFlowInstanceElement)Source.Clone(), (ProcessFlowInstanceElement)Target.Clone());
        }
    }
}
