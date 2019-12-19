using CaseManagement.CMMN.Persistence.Parameters;
using CaseManagement.CMMN.Persistence.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Persistence
{
    public interface ICMMNDefinitionsQueryRepository
    {
        Task<ICollection<tDefinitions>> GetAll();
        Task<FindResponse<tDefinitions>> Find(BaseFindParameter parameter);
        Task<tDefinitions> FindDefinitionById(string id); 
    }
}
