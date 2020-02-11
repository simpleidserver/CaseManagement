using CaseManagement.CMMN.Domains;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Persistence
{
    public interface IPerformanceCommandRepository
    {
        void Add(PerformanceAggregate performanceStatistic);
        Task KeepLastRecords(int nbRecords, string machineName);
        Task<int> SaveChanges();
    }
}