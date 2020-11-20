using CaseManagement.BPMN.Exceptions;
using CaseManagement.BPMN.Persistence;
using CaseManagement.BPMN.ProcessFile.Results;
using CaseManagement.BPMN.Resources;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.BPMN.ProcessFile.Queries.Handlers
{
    public class GetProcessFileQueryHandler : IRequestHandler<GetProcessFileQuery, ProcessFileResult>
    {
        private readonly IProcessFileQueryRepository _processFileQueryRepository;

        public GetProcessFileQueryHandler(IProcessFileQueryRepository processFileQueryRepository)
        {
            _processFileQueryRepository = processFileQueryRepository;
        }

        public async Task<ProcessFileResult> Handle(GetProcessFileQuery request, CancellationToken cancellationToken)
        {
            var result = await _processFileQueryRepository.Get(request.Id, cancellationToken);
            if (result == null)
            {
                throw new UnknownProcessFileException(string.Format(Global.UnknownProcessFile, request.Id));
            }

            return ProcessFileResult.ToDto(result);
        }
    }
}
