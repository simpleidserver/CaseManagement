using CaseManagement.Gateway.Website.Statistic.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CaseManagement.Gateway.Website.Statistic.Services
{
    public interface IStatisticService
    {
        Task<DailyStatisticResponse> Get();
        Task<FindDailyStatisticResponse> Search(IEnumerable<KeyValuePair<string, string>> queries);
    }
}
