namespace CaseManagement.Workflow.Domains
{
    public class ProcessFlowInstanceElement
    {
        public ProcessFlowInstanceElement(string id, string name)
        {
            Id = id;
            Name = name;
            Status = ProcessFlowInstanceElementStatus.None;
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public ProcessFlowInstanceElementStatus Status { get; set; }

        public void Start()
        {
            Status = ProcessFlowInstanceElementStatus.Started;
        }

        public void Finish()
        {
            Status = ProcessFlowInstanceElementStatus.Finished;
        }
    }
}
