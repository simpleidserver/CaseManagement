using CaseManagement.CMMN.Domains;
using CaseManagement.Workflow.Engine;

namespace CaseManagement.CMMN.CaseInstance.Repositories
{
    public interface ICaseFileItemRepository : IWorkflowSubProcess
    {
        string CaseFileItemType { get; }
        CaseFileItem Get(CMMNCaseFileItem caseFileItem);
    }
}