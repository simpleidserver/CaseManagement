using CaseManagement.CMMN.CaseFile.Results;
using CaseManagement.CMMN.Persistence;
using CaseManagement.Common.Results;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CaseFile.Queries.Handlers
{
    public class SearchCaseFilesQueryHandler : IRequestHandler<SearchCaseFileQuery, SearchResult<CaseFileResult>>
    {
        private readonly ICaseFileQueryRepository _queryRepository;

        public SearchCaseFilesQueryHandler(ICaseFileQueryRepository queryRepository)
        {
            _queryRepository = queryRepository;
        }

        public Task<SearchResult<CaseFileResult>> Handle(SearchCaseFileQuery request, CancellationToken cancellationToken)
        {
            return _queryRepository.Find(new Persistence.Parameters.FindCaseFilesParameter
            {
                Count = request.Count,
                Order = request.Order,
                OrderBy = request.OrderBy,
                CaseFileId = request.CaseFileId,
                StartIndex = request.StartIndex,
                TakeLatest = request.TakeLatest,
                Text = request.Text
            }, cancellationToken);
        }
    }
}
