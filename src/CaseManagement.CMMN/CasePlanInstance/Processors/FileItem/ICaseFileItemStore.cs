using CaseManagement.CMMN.Domains;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CasePlanInstance.Processors.FileItem
{
    public interface ICaseFileItemStore
    {
        string CaseFileItemType { get; }
        Task<bool> TryAddCaseFileItem(CaseFileItemInstance caseFileItem, CasePlanInstanceAggregate casePlanInstance, CancellationToken token);
    }
}