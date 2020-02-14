using CaseManagement.Gateway.Website.CaseFile.DTOs;
using CaseManagement.Gateway.Website.CaseFile.Queries;
using CaseManagement.Gateway.Website.CaseFile.Services;
using CaseManagement.Gateway.Website.Extensions;
using System.Linq;
using System.Threading.Tasks;

namespace CaseManagement.Gateway.Website.CaseFile.QueryHandlers
{
    public class SearchCaseFileHistoryQueryHandler : ISearchCaseFileHistoryQueryHandler
    {
        private readonly ICaseFileService _caseFileService;

        public SearchCaseFileHistoryQueryHandler(ICaseFileService caseFileService)
        {
            _caseFileService = caseFileService;
        }

        public Task<FindResponse<CaseFileResponse>> Handle(SearchCaseFileHistoryQuery searchCaseFileHistoryQuery)
        {
            var queries = searchCaseFileHistoryQuery.Queries.ToList();
            queries.TryReplace("owner", searchCaseFileHistoryQuery.NameIdentifier);
            queries.TryReplace("case_file_id", searchCaseFileHistoryQuery.CaseFileId);
            return _caseFileService.Search(queries);
        }
    }
}