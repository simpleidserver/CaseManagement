using CaseManagement.CMMN.Common;
using CaseManagement.CMMN.Persistence;
using CaseManagement.Common.Results;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CaseFile.Queries.Handlers
{
    public class CountCaseFileQueryHandler : IRequestHandler<CountCaseFileQuery, CountResult>
    {
        private readonly ICasePlanQueryRepository _queryRepository;

        public CountCaseFileQueryHandler(ICasePlanQueryRepository queryRepository)
        {
            _queryRepository = queryRepository;
        }

        public async Task<CountResult> Handle(CountCaseFileQuery request, CancellationToken cancellationToken)
        {
            var result = await _queryRepository.Count(cancellationToken);
            return new CountResult(result);
        }
    }
}
