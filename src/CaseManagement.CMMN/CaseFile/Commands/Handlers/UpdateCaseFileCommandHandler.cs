using CaseManagement.CMMN.CaseFile.Commands;
using CaseManagement.CMMN.CaseFile.Exceptions;
using CaseManagement.CMMN.Persistence;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CaseFile.Command.Handlers
{
    public class UpdateCaseFileCommandHandler : IRequestHandler<UpdateCaseFileCommand, bool>
    {
        private readonly ICaseFileCommandRepository _caseFileCommandRepository;

        public UpdateCaseFileCommandHandler(
            ICaseFileCommandRepository caseFileCommandRepository)
        {
            _caseFileCommandRepository = caseFileCommandRepository;
        }

        public async Task<bool> Handle(UpdateCaseFileCommand command, CancellationToken token)
        {
            var caseFile = await _caseFileCommandRepository.Get(command.Id, token);
            if (caseFile == null || string.IsNullOrWhiteSpace(caseFile.AggregateId))
            {
                throw new UnknownCaseFileException(command.Id);
            }

            caseFile.Update(command.Name, command.Description);
            await _caseFileCommandRepository.Update(caseFile, token);
            await _caseFileCommandRepository.SaveChanges(token);
            return true;
        }
    }
}
