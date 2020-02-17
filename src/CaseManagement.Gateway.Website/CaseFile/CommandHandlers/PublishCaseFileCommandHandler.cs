using CaseManagement.Gateway.Website.CaseFile.Commands;
using CaseManagement.Gateway.Website.CaseFile.Services;
using System.Threading.Tasks;

namespace CaseManagement.Gateway.Website.CaseFile.CommandHandlers
{
    public class PublishCaseFileCommandHandler : IPublishCaseFileCommandHandler
    {
        private readonly ICaseFileService _caseFileService;

        public PublishCaseFileCommandHandler(ICaseFileService caseFileService)
        {
            _caseFileService = caseFileService;
        }

        public Task<string> Handle(PublishCaseFileCommand publishCommand)
        {
            return _caseFileService.PublishMe(publishCommand.CaseFileId, publishCommand.IdentityToken);
        }
    }
}
