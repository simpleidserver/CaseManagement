using MediatR;
using CaseManagement.CMMN.CaseFile.Results;
using CaseManagement.CMMN.Common;
using CaseManagement.CMMN.Persistence;
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

        public async Task<SearchResult<CaseFileResult>> Handle(SearchCaseFileQuery request, CancellationToken cancellationToken)
        {
            var result = await _queryRepository.Find(new Persistence.Parameters.FindCaseFilesParameter
            {
                Count = request.Count,
                Order = request.Order,
                OrderBy = request.OrderBy,
                CaseFileId = request.CaseFileId,
                Owner = request.Owner,
                StartIndex = request.StartIndex
            }, cancellationToken);
            return new SearchResult<CaseFileResult>
            {
                Content = result.Content.Select(_ => CaseFileResult.ToDto(_)),
                Count = result.Count,
                StartIndex = result.StartIndex,
                TotalLength = result.TotalLength
            };
        }
    }
}
