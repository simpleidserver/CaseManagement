using CaseManagement.CMMN.CaseFile.Exceptions;
using CaseManagement.CMMN.Persistence;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CaseFile.Commands.Handlers
{
    public class UpdateCaseFilePayloadCommandHandler : IRequestHandler<UpdateCaseFilePayloadCommand, bool>
    {
        private readonly ICaseFileCommandRepository _caseFileCommandRepository;

        public UpdateCaseFilePayloadCommandHandler(
            ICaseFileCommandRepository caseFileCommandRepository)
        {
            _caseFileCommandRepository = caseFileCommandRepository;
        }

        public async Task<bool> Handle(UpdateCaseFilePayloadCommand request, CancellationToken cancellationToken)
        {
            var caseFile = await _caseFileCommandRepository.Get(request.Id, cancellationToken);
            if (caseFile == null || string.IsNullOrWhiteSpace(caseFile.AggregateId))
            {
                throw new UnknownCaseFileException(request.Id);
            }

            caseFile.UpdatePayload(request.Payload);
            await _caseFileCommandRepository.Update(caseFile, cancellationToken);
            await _caseFileCommandRepository.SaveChanges(cancellationToken);
            return true;
        }
    }
}
