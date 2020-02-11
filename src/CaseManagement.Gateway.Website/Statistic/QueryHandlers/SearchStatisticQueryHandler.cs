using CaseManagement.Gateway.Website.Statistic.DTOs;
using CaseManagement.Gateway.Website.Statistic.Queries;
using CaseManagement.Gateway.Website.Statistic.Services;
using System.Threading.Tasks;

namespace CaseManagement.Gateway.Website.Statistic.QueryHandlers
{
    public class SearchStatisticQueryHandler : ISearchStatisticQueryHandler
    {
        private readonly IStatisticService _statisticService;

        public SearchStatisticQueryHandler(IStatisticService statisticService)
        {
            _statisticService = statisticService;
        }

        public Task<FindDailyStatisticResponse> Handle(SearchStatisticQuery query)
        {
            return _statisticService.Search(query.Queries);
        }
    }
}
