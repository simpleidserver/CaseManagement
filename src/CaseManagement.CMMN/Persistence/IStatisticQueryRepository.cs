using CaseManagement.CMMN.Domains;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Persistence
{
    public interface IStatisticQueryRepository
    {
        Task<CMMNWorkflowDefinitionStatisticAggregate> FindById(string id);
    }
}
