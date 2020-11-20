using CaseManagement.BPMN.ProcessFile.Results;
using CaseManagement.Common.Parameters;
using CaseManagement.Common.Results;
using MediatR;

namespace CaseManagement.BPMN.ProcessFile.Queries
{
    public class SearchProcessFilesQuery : BaseSearchParameter, IRequest<SearchResult<ProcessFileResult>>
    {
        public bool TakeLatest { get; set; }
    }
}
