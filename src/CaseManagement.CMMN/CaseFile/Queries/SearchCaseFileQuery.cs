using MediatR;
using CaseManagement.CMMN.CaseFile.Results;
using CaseManagement.CMMN.Common;
using CaseManagement.CMMN.Common.Parameters;

namespace CaseManagement.CMMN.CaseFile.Queries
{
    public class SearchCaseFileQuery : BaseSearchParameter, IRequest<SearchResult<CaseFileResult>>
    {
        public string Owner { get; set; }
        public string CaseFileId { get; set; }
    }
}
