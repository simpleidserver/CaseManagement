using CaseManagement.Gateway.Website.CaseFile.DTOs;
using CaseManagement.Gateway.Website.CaseFile.Queries;
using CaseManagement.Gateway.Website.CaseFile.Services;
using CaseManagement.Gateway.Website.Extensions;
using System.Linq;
using System.Threading.Tasks;

namespace CaseManagement.Gateway.Website.CaseFile.QueryHandlers
{
    public class SearchMyLatestCaseFileQueryHandler : ISearchMyLatestCaseFileQueryHandler
    {
        private readonly ICaseFileService _caseFileService;

        public SearchMyLatestCaseFileQueryHandler(ICaseFileService caseFileService)
        {
            _caseFileService = caseFileService;
        }

        public Task<FindResponse<CaseFileResponse>> Handle(SearchMyLatestPublishedCaseFileQuery query)
        {
            var queries = query.Queries.ToList();
            queries.TryReplace("owner", query.NameIdentifier);
            queries.TryReplace("take_latest", "true");
            return _caseFileService.Search(queries);
        }
    }
}