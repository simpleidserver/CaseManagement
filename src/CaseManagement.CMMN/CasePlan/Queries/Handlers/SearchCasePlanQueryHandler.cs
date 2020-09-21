using CaseManagement.CMMN.CasePlan.Results;
using CaseManagement.CMMN.Persistence;
using CaseManagement.Common.Results;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CasePlan.Queries.Handlers
{
    public class SearchCasePlanQueryHandler : IRequestHandler<SearchCasePlanQuery, SearchResult<CasePlanResult>>
    {
        private readonly ICasePlanQueryRepository _casePlanQueryRepository;

        public SearchCasePlanQueryHandler(ICasePlanQueryRepository casePlanQueryRepository)
        {
            _casePlanQueryRepository = casePlanQueryRepository;
        }

        public async Task<SearchResult<CasePlanResult>> Handle(SearchCasePlanQuery request, CancellationToken cancellationToken)
        {
            var result = await _casePlanQueryRepository.Find(new Persistence.Parameters.FindCasePlansParameter
            {
                CaseOwner = request.Owner,
                Count = request.Count,
                OrderBy = request.OrderBy,
                Order = request.Order,
                CaseFileId = request.CaseFileId,
                StartIndex = request.StartIndex,
                CasePlanId = request.CasePlanId,
                TakeLatest = request.TakeLatest
            }, cancellationToken);
            return new SearchResult<CasePlanResult>
            {
                Content = result.Content.Select(_ => CasePlanResult.ToDto(_)),
                Count = result.Count,
                StartIndex = result.StartIndex,
                TotalLength = result.TotalLength
            };
        }
    }
}
