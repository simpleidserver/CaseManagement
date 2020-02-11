using System.Collections.Generic;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CasePlanInstance.Repositories
{
    public interface ICaseFileItemRepository
    {
        Task<CaseFileItem> FindByCaseElementInstance(string caseElementInstanceId);
        Task<IEnumerable<CaseFileItem>> FindByCaseInstance(string caseInstanceId);

        Task AddCaseFileItem(string caseInstanceId, string caseElementInstanceId, string caseElementDefinitionId, string id);
    }
}