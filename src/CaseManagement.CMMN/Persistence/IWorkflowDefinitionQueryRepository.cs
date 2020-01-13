using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Persistence.Parameters;
using CaseManagement.CMMN.Persistence.Responses;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Persistence
{
    public interface IWorkflowDefinitionQueryRepository
    {
        Task<CaseDefinition> FindById(string id);
        Task<FindResponse<CaseDefinition>> Find(FindWorkflowDefinitionsParameter parameter);
    }
}
