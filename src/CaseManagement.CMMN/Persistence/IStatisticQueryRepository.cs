using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Persistence.Parameters;
using CaseManagement.CMMN.Persistence.Responses;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Persistence
{
    public interface IStatisticQueryRepository
    {
        Task<CaseDefinitionStatisticAggregate> FindById(string id);
        Task<FindResponse<DailyStatisticAggregate>> FindDailyStatistics(FindDailyStatisticsParameter parameter);
    }
}
