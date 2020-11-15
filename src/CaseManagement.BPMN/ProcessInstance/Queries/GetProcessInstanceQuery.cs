using CaseManagement.BPMN.ProcessInstance.Results;
using MediatR;

namespace CaseManagement.BPMN.ProcessInstance.Queries
{
    public class GetProcessInstanceQuery : IRequest<ProcessInstanceResult>
    {
        public string Id { get; set; }
    }
}
