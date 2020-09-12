using CaseManagement.CMMN.Domains;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CasePlanInstance.Processors.FileItem
{
    public class CMISCaseFileItemStore : ICaseFileItemStore
    {
        public CMISCaseFileItemStore()
        {
        }

        public string CaseFileItemType => CMMNConstants.ContentManagementTypes.FAKE_CMIS_DIRECTORY;

        public Task<bool> TryAddCaseFileItem(CaseFileItemInstance caseFileItem, CasePlanInstanceAggregate casePlanInstance, CancellationToken token)
        {
            if (casePlanInstance.IsFileExists(caseFileItem.Id))
            {
                return Task.FromResult(false);
            }

            casePlanInstance.TryAddCaseFileItem(caseFileItem.Id, CaseFileItemType, "value");
            return Task.FromResult(true);
        }
    }
}
