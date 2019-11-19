using CaseManagement.Workflow.Domains;

namespace CaseManagement.BPMN.Domains
{
    public class BPMNReceiveTask : ProcessFlowInstanceElement
    {
        public BPMNReceiveTask(string id, string name, string operationId) : base(id, name)
        {
            OperationId = operationId;
        }

        public string OperationId { get; set; }
    }
}
