namespace CaseManagement.Workflow.Domains
{
    public class ProcessFlowConnector
    {
        public ProcessFlowConnector(ProcessFlowInstanceElement source, ProcessFlowInstanceElement target)
        {
            Source = source;
            Target = target;
        }

        public ProcessFlowInstanceElement Source { get; set; }
        public ProcessFlowInstanceElement Target { get; set; }
    }
}
