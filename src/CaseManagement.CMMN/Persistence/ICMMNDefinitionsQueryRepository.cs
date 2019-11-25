using System.Collections.Generic;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Persistence
{
    public interface ICMMNDefinitionsQueryRepository
    {
        Task<ICollection<tDefinitions>> GetAll();
        Task<tDefinitions> FindDefinitionById(string id); 
    }
}
