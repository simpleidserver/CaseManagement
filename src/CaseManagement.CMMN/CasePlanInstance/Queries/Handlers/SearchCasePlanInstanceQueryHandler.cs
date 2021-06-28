using CaseManagement.CMMN.CasePlanInstance.Results;
using CaseManagement.CMMN.Persistence;
using CaseManagement.CMMN.Persistence.Parameters;
using CaseManagement.Common.Results;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CasePlanInstance.Queries.Handlers
{
    public class SearchCasePlanInstanceQueryHandler : IRequestHandler<SearchCasePlanInstanceQuery, SearchResult<CasePlanInstanceResult>>
    {
        private readonly ICasePlanInstanceQueryRepository _casePlanInstanceQueryRepository;

        public SearchCasePlanInstanceQueryHandler(ICasePlanInstanceQueryRepository casePlanInstanceQueryRepository)
        {
            _casePlanInstanceQueryRepository = casePlanInstanceQueryRepository;
        }

        public Task<SearchResult<CasePlanInstanceResult>> Handle(SearchCasePlanInstanceQuery request, CancellationToken cancellationToken)
        {
           return _casePlanInstanceQueryRepository.Find(new FindCasePlanInstancesParameter
            {
                CasePlanId = request.CasePlanId,
                CaseFileId = request.CaseFileId,
                Count = request.Count,
                Order = request.Order,
                OrderBy = request.OrderBy,
                StartIndex = request.StartIndex
            }, cancellationToken);
        }
    }
}
