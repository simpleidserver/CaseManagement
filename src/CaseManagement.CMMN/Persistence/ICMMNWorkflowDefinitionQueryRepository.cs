using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Persistence.Parameters;
using CaseManagement.CMMN.Persistence.Responses;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Persistence
{
    public interface ICMMNWorkflowDefinitionQueryRepository
    {
        Task<CMMNWorkflowDefinition> FindById(string id);
        Task<FindResponse<CMMNWorkflowDefinition>> Find(FindWorkflowDefinitionsParameter parameter);
    }
}
