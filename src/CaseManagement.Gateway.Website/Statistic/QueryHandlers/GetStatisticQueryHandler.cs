using CaseManagement.Gateway.Website.Statistic.DTOs;
using CaseManagement.Gateway.Website.Statistic.Queries;
using CaseManagement.Gateway.Website.Statistic.Services;
using System.Threading.Tasks;

namespace CaseManagement.Gateway.Website.Statistic.QueryHandlers
{
    public class GetStatisticQueryHandler : IGetStatisticQueryHandler
    {
        private readonly IStatisticService _statisticService;

        public GetStatisticQueryHandler(IStatisticService statisticService)
        {
            _statisticService = statisticService;
        }

        public Task<DailyStatisticResponse> Handle(GetStatisticQuery query)
        {
            return _statisticService.Get();
        }
    }
}
