using CaseManagement.Gateway.Website.CaseFile.Commands;
using CaseManagement.Gateway.Website.CaseFile.Services;
using System.Threading.Tasks;

namespace CaseManagement.Gateway.Website.CaseFile.CommandHandlers
{
    public class UpdateCaseFileCommandHandler : IUpdateCaseFileCommandHandler
    {
        private readonly ICaseFileService _caseFileService;

        public UpdateCaseFileCommandHandler(ICaseFileService caseFileService)
        {
            _caseFileService = caseFileService;
        }

        public Task Handle(UpdateCaseFileCommand command)
        {
            return _caseFileService.UpdateMe(command.CaseFileId, new DTOs.UpdateCaseFileParameter
            {
                Description = command.Description,
                Name = command.Name,
                Payload = command.Payload
            }, command.IdentityToken);
        }
    }
}
