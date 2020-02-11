using CaseManagement.CMMN.Domains;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Persistence
{
    public interface IDailyStatisticCommandRepository
    {
        void Update(DailyStatisticAggregate caseDailyStatistic);
        void Add(DailyStatisticAggregate caseDailyStatistic);
        Task<int> SaveChanges();
    }
}