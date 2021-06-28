using CaseManagement.CMMN.Persistence;
using CaseManagement.Common.Results;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CasePlan.Queries.Handlers
{
    public class CountCasePlanQueryHandler : IRequestHandler<CountCasePlanQuery, CountResult>
    {
        private readonly ICasePlanQueryRepository _queryRepository;

        public CountCasePlanQueryHandler(ICasePlanQueryRepository queryRepository)
        {
            _queryRepository = queryRepository;
        }

        public async Task<CountResult> Handle(CountCasePlanQuery request, CancellationToken cancellationToken)
        {
            var result = await _queryRepository.Count(cancellationToken);
            return new CountResult(result);
        }
    }
}