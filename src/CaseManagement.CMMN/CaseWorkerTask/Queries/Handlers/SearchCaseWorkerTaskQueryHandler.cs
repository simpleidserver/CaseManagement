using CaseManagement.CMMN.CaseWorkerTask.Results;
using CaseManagement.CMMN.Persistence;
using CaseManagement.CMMN.Persistence.Parameters;
using CaseManagement.Common.Results;
using MediatR;
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

        public Task<SearchResult<CaseWorkerTaskResult>> Handle(SearchCaseWorkerTaskQuery request, CancellationToken cancellationToken)
        {
            return _caseWorkerTaskQueryRepository.Find(new FindCaseWorkerTasksParameter
            {
                Claims = request.Claims,
                Count = request.Count,
                Order=  request.Order,
                OrderBy = request.OrderBy,
                StartIndex = request.StartIndex
            }, cancellationToken);
        }
    }
}
