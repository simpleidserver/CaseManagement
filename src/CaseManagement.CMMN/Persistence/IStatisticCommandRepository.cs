using CaseManagement.CMMN.Domains;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Persistence
{
    public interface IStatisticCommandRepository
    {
        void Update(CaseDefinitionStatisticAggregate cmmnWorkflowDefinitionStatisticAggregate);
        void Add(CaseDefinitionStatisticAggregate cmmnWorkflowDefinitionStatisticAggregate);
        void Update(DailyStatisticAggregate caseDailyStatistic);
        void Add(DailyStatisticAggregate caseDailyStatistic);
        Task<int> SaveChanges();
    }
}