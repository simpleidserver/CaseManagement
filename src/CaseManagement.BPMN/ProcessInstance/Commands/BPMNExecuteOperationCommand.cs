using CaseManagement.BPMN.Infrastructure;

namespace CaseManagement.BPMN.ProcessInstance.Commands
{
    public class BPMNExecuteOperationCommand : ICommand
    {
        public string OperationId { get; set; }
    }
}
