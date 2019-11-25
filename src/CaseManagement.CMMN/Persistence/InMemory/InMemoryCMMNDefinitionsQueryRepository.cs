using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Persistence.InMemory
{
    public class InMemoryCMMNDefinitionsQueryRepository : ICMMNDefinitionsQueryRepository
    {
        private ICollection<tDefinitions> _tDefinitions;

        public InMemoryCMMNDefinitionsQueryRepository(ICollection<tDefinitions> definitions)
        {
            _tDefinitions = definitions;
        }

        public Task<tDefinitions> FindDefinitionById(string id)
        {
            return Task.FromResult(_tDefinitions.FirstOrDefault(d => d.id == id));
        }

        public Task<ICollection<tDefinitions>> GetAll()
        {
            return Task.FromResult(_tDefinitions);
        }
    }
}