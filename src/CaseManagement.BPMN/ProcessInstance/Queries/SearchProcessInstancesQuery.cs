using CaseManagement.BPMN.Domains;
using CaseManagement.BPMN.ProcessInstance.Results;
using CaseManagement.Common.Parameters;
using CaseManagement.Common.Results;
using MediatR;

namespace CaseManagement.BPMN.ProcessInstance.Queries
{
    public class SearchProcessInstancesQuery : BaseSearchParameter, IRequest<SearchResult<ProcessInstanceResult>>
    {
        public string ProcessFileId { get; set; }
        public ProcessInstanceStatus? Status { get; set; }
    }
}
