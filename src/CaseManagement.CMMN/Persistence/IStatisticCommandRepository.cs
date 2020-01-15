using CaseManagement.CMMN.Domains;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Persistence
{
    public interface IStatisticCommandRepository
    {
        void Update(DailyStatisticAggregate caseDailyStatistic);
        void Add(DailyStatisticAggregate caseDailyStatistic);
        void Add(PerformanceStatisticAggregate performanceStatistic);
        Task KeepLastRecords(int nbRecords, string machineName);
        Task<int> SaveChanges();
    }
}