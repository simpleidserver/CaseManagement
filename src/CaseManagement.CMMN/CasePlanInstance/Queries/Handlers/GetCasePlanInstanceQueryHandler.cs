using CaseManagement.CMMN.CasePlanInstance.Exceptions;
using CaseManagement.CMMN.CasePlanInstance.Results;
using CaseManagement.CMMN.Persistence;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CasePlanInstance.Queries.Handlers
{
    public class GetCasePlanInstanceQueryHandler : IRequestHandler<GetCasePlanInstanceQuery, CasePlanInstanceResult>
    {
        private readonly ICasePlanInstanceQueryRepository _casePlanInstanceQueryRepository;

        public GetCasePlanInstanceQueryHandler(ICasePlanInstanceQueryRepository casePlanInstanceQueryRepository)
        {
            _casePlanInstanceQueryRepository = casePlanInstanceQueryRepository;
        }


        public async Task<CasePlanInstanceResult> Handle(GetCasePlanInstanceQuery request, CancellationToken cancellationToken)
        {
            var result = await _casePlanInstanceQueryRepository.Get(request.CasePlanInstanceId, cancellationToken);
            if (result == null)
            {
                throw new UnknownCasePlanInstanceException(request.CasePlanInstanceId);
            }

            return result;
        }
    }
}
