using CaseManagement.Gateway.Website.Statistic.DTOs;
using CaseManagement.Gateway.Website.Statistic.Queries;
using System.Threading.Tasks;

namespace CaseManagement.Gateway.Website.Statistic.QueryHandlers
{
    public interface IGetStatisticQueryHandler
    {
        Task<DailyStatisticResponse> Handle(GetStatisticQuery query);
    }
}
