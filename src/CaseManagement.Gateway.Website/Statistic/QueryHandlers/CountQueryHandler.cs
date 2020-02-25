using CaseManagement.Gateway.Website.CaseFile.Services;
using CaseManagement.Gateway.Website.CasePlans.Services;
using CaseManagement.Gateway.Website.Statistic.Queries;
using CaseManagement.Gateway.Website.Statistic.Results;
using System.Threading.Tasks;

namespace CaseManagement.Gateway.Website.Statistic.QueryHandlers
{
    public class CountQueryHandler : ICountQueryHandler
    {
        private readonly ICaseFileService _caseFileService;
        private readonly ICasePlanService _casePlanService;

        public CountQueryHandler(ICaseFileService caseFileService, ICasePlanService casePlanService)
        {
            _caseFileService = caseFileService;
            _casePlanService = casePlanService;
        }

        public async Task<CountQueryResult> Handle(CountQuery query)
        {
            var caseFileCount = await _caseFileService.Count();
            var casePlanCount = await _casePlanService.Count();
            return new CountQueryResult
            {
                NbCaseFiles = caseFileCount,
                NbCasePlans = casePlanCount
            };
        }
    }
}
