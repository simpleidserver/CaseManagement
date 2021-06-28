using MediatR;
using CaseManagement.CMMN.CaseFile.Exceptions;
using CaseManagement.CMMN.CaseFile.Results;
using CaseManagement.CMMN.Persistence;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CaseFile.Queries.Handlers
{
    public class GetCaseFileQueryHandler : IRequestHandler<GetCaseFileQuery, CaseFileResult>
    {
        private readonly ICaseFileQueryRepository _queryRepository;

        public GetCaseFileQueryHandler(ICaseFileQueryRepository queryRepository)
        {
            _queryRepository = queryRepository;
        }

        public async Task<CaseFileResult> Handle(GetCaseFileQuery request, CancellationToken cancellationToken)
        {
            var result = await _queryRepository.Get(request.Id, cancellationToken);
            if (result == null)
            {
                throw new UnknownCaseFileException(request.Id);
            }

            return result;
        }
    }
}
