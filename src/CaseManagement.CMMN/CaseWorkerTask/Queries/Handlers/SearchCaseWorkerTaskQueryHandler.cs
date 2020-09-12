using CaseManagement.CMMN.CaseWorkerTask.Results;
using CaseManagement.CMMN.Common;
using CaseManagement.CMMN.Persistence;
using CaseManagement.CMMN.Persistence.Parameters;
using MediatR;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CaseWorkerTask.Queries.Handlers
{
    public class SearchCaseWorkerTaskQueryHandler : IRequestHandler<SearchCaseWorkerTaskQuery, SearchResult<CaseWorkerTaskResult>>
    {
        private readonly ICaseWorkerTaskQueryRepository _caseWorkerTaskQueryRepository;

        public SearchCaseWorkerTaskQueryHandler(ICaseWorkerTaskQueryRepository caseWorkerTaskQueryRepository)
        {
            _caseWorkerTaskQueryRepository = caseWorkerTaskQueryRepository;
        }

        public async Task<SearchResult<CaseWorkerTaskResult>> Handle(SearchCaseWorkerTaskQuery request, CancellationToken cancellationToken)
        {
            var result = await _caseWorkerTaskQueryRepository.Find(new FindCaseWorkerTasksParameter
            {
                Claims = request.Claims,
                Count = request.Count,
                Order=  request.Order,
                OrderBy = request.OrderBy,
                StartIndex = request.StartIndex
            }, cancellationToken);
            return new SearchResult<CaseWorkerTaskResult>
            {
                Count = result.Count,
                StartIndex = result.StartIndex,
                TotalLength = result.TotalLength,
                Content = result.Content.Select(_ => CaseWorkerTaskResult.ToDTO(_))
            };
        }
    }
}
