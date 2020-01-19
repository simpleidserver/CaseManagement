using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CaseInstance.Repositories
{
    public class InMemoryDirectoryCaseFileItemRepository : ICaseFileItemRepository
    {
        private ConcurrentBag<CaseFileItem> _caseFileItems;

        public InMemoryDirectoryCaseFileItemRepository()
        {
            _caseFileItems = new ConcurrentBag<CaseFileItem>();
        }

        public Task<CaseFileItem> FindByCaseElementInstance(string caseElementInstanceId)
        {
            return Task.FromResult(_caseFileItems.FirstOrDefault(c => c.CaseElementInstanceId == caseElementInstanceId));
        }

        public Task<IEnumerable<CaseFileItem>> FindByCaseInstance(string caseInstanceId)
        {
            return Task.FromResult(_caseFileItems.Where(c => c.CaseInstanceId == caseInstanceId));
        }

        public Task AddCaseFileItem(string caseInstanceId, string caseElementInstanceId, string caseElementDefinitionId, string value)
        {
            _caseFileItems.Add(new DirectoryCaseFileItem(Guid.NewGuid().ToString())
            {
                CaseInstanceId = caseInstanceId,
                CaseElementInstanceId = caseElementInstanceId,
                CaseElementDefinitionId = caseElementDefinitionId,
                Value = value
            });
            return Task.CompletedTask;
        }
    }
}