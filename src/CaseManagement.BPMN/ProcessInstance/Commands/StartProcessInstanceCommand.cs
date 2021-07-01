using MediatR;

namespace CaseManagement.BPMN.ProcessInstance.Commands
{
    public class StartProcessInstanceCommand : IRequest<bool>
    {
        public StartProcessInstanceCommand()
        {
            NewExecutionPath = true;
        }

        public string ProcessInstanceId { get; set; }
        public string NameIdentifier { get; set; }
        public bool NewExecutionPath { get; set; }
    }
}
