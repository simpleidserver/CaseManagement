using CaseManagement.BPMN.ProcessInstance.Results;
using CaseManagement.Common.Results;
using MediatR;

namespace CaseManagement.BPMN.ProcessInstance.Commands
{
    public class CreateProcessInstanceCommand : IRequest<SearchResult<ProcessInstanceResult>>
    {
        public string ProcessFileId { get; set; }
    }
}
