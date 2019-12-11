using CaseManagement.CMMN.Domains;
using System.Collections.Generic;
using System.Linq;

namespace CaseManagement.CMMN.CaseInstance.Repositories
{
    public class CaseFileItemRepositoryFactory : ICaseFileItemRepositoryFactory
    {
        private readonly IEnumerable<ICaseFileItemRepository> _caseFileItemRepositories;

        public CaseFileItemRepositoryFactory(IEnumerable<ICaseFileItemRepository> caseFileItemRepositories)
        {
            _caseFileItemRepositories = caseFileItemRepositories;
        }

        public ICaseFileItemRepository Get(CMMNCaseFileItem caseFileItem)
        {
            return _caseFileItemRepositories.First(c => c.CaseFileItemType == caseFileItem.Definition.DefinitionType);
        }
    }
}
