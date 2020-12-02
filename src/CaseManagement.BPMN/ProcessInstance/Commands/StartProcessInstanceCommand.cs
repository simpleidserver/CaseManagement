using MediatR;

namespace CaseManagement.BPMN.ProcessInstance.Commands
{
    public class StartProcessInstanceCommand : IRequest<bool>
    {
        public string ProcessInstanceId { get; set; }
        public string NameIdentifier { get; set; }
    }
}
