using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Persistence.Parameters;
using CaseManagement.CMMN.Persistence.Responses;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Persistence
{
    public interface ICaseDefinitionQueryRepository
    {
        Task<CaseDefinition> FindById(string id);
        Task<FindResponse<CaseDefinition>> Find(FindWorkflowDefinitionsParameter parameter);
        Task<CaseDefinitionHistoryAggregate> FindHistoryById(string id);
        Task<int> Count();
    }
}
