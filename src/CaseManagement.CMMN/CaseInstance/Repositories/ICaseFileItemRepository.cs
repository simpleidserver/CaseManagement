using CaseManagement.CMMN.Domains;

namespace CaseManagement.CMMN.CaseInstance.Repositories
{
    public interface ICaseFileItemRepository
    {
        string CaseFileItemType { get; }
        // CaseFileItem Get(CMMNCaseFileItem caseFileItem);
    }
}