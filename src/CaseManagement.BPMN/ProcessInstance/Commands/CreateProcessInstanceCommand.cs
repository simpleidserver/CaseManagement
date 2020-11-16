using CaseManagement.BPMN.ProcessInstance.Results;
using MediatR;

namespace CaseManagement.BPMN.ProcessInstance.Commands
{
    public class CreateProcessInstanceCommand : IRequest<ProcessInstanceResult>
    {
        public string ProcessFileId { get; set; }
    }
}
