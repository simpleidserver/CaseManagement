using System.Collections.Generic;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CaseInstance.Repositories
{
    public class InMemoryDirectoryCaseFileItemRepository : ICaseFileItemRepository
    {
        private Dictionary<string, CaseFileItem> _caseFileItems;

        public InMemoryDirectoryCaseFileItemRepository()
        {
            _caseFileItems = new Dictionary<string, CaseFileItem>();
        }

        public Task<CaseFileItem> GetCaseFileItemInstance(string instanceId)
        {
            if (!_caseFileItems.ContainsKey(instanceId))
            {
                return Task.FromResult((CaseFileItem)null);
            }

            return Task.FromResult(_caseFileItems[instanceId]);
        }

        public Task AddCaseFileItem(string instanceId, string id)
        {
            _caseFileItems.Add(instanceId, new DirectoryCaseFileItem(id));
            return Task.CompletedTask;
        }
    }
}