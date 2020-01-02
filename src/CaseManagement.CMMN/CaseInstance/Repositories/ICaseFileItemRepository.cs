using System.Threading.Tasks;

namespace CaseManagement.CMMN.CaseInstance.Repositories
{
    public interface ICaseFileItemRepository
    {
        Task<CaseFileItem> GetCaseFileItemInstance(string id);
        Task AddCaseFileItem(string instanceId, string id);
    }
}