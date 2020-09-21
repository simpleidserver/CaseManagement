using MediatR;
using CaseManagement.CMMN.Common;
using CaseManagement.CMMN.Persistence;
using System.Threading;
using System.Threading.Tasks;
using CaseManagement.Common.Results;

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