using CaseManagement.CMMN.CaseFile.Results;
using CaseManagement.Common.Parameters;
using CaseManagement.Common.Results;
using MediatR;

namespace CaseManagement.CMMN.CaseFile.Queries
{
    public class SearchCaseFileQuery : BaseSearchParameter, IRequest<SearchResult<CaseFileResult>>
    {
        public string Owner { get; set; }
        public string CaseFileId { get; set; }
        public bool TakeLatest { get; set; }
        public string Text { get; set; }
    }
}
