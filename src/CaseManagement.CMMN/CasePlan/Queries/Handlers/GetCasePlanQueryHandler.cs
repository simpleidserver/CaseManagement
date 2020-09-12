using MediatR;
using CaseManagement.CMMN.CasePlan.Results;
using CaseManagement.CMMN.Common.Exceptions;
using CaseManagement.CMMN.Persistence;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CasePlan.Queries.Handlers
{
    public class GetCasePlanQueryHandler : IRequestHandler<GetCasePlanQuery, CasePlanResult>
    {
        private readonly ICasePlanQueryRepository _queryRepository;

        public GetCasePlanQueryHandler(ICasePlanQueryRepository queryRepository)
        {
            _queryRepository = queryRepository;
        }

        public async Task<CasePlanResult> Handle(GetCasePlanQuery request, CancellationToken cancellationToken)
        {
            var result = await _queryRepository.Get(request.Id, cancellationToken);
            if (result == null)
            {
                throw new UnknownCasePlanException(request.Id);
            }

            return CasePlanResult.ToDto(result);
        }
    }
}