using CaseManagement.BPMN.Domains;
using CaseManagement.Common.Parameters;

namespace CaseManagement.BPMN.Persistence.Parameters
{
    public class FindProcessInstancesParameter : BaseSearchParameter
    {
        public string ProcessFileId { get; set; }
        public ProcessInstanceStatus? Status { get; set; }
    }
}
