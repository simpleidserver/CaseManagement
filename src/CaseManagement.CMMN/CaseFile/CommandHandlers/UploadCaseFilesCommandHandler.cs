using CaseManagement.CMMN.CaseFile.Commands;
using CaseManagement.CMMN.Domains.CaseFile;
using CaseManagement.CMMN.Infrastructures;
using CaseManagement.CMMN.Parser;
using CaseManagement.CMMN.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CaseFile.CommandHandlers
{
    public class UploadCaseFilesCommandHandler : IUploadCaseFilesCommandHandler
    {
        private readonly ICaseDefinitionCommandRepository _caseDefinitionCommandRepository;
        private readonly ICaseFileCommandRepository _caseFileCommandRepository;

        public UploadCaseFilesCommandHandler(ICaseDefinitionCommandRepository caseDefinitionCommandRepository, ICaseFileCommandRepository caseFileCommandRepository)
        {
            _caseDefinitionCommandRepository = caseDefinitionCommandRepository;
            _caseFileCommandRepository = caseFileCommandRepository;
        }

        public async Task<IEnumerable<string>> Handle(UploadCaseFilesCommand uploadCaseFilesCommand)
        {
            var result = new List<string>();
            var tDefinitionsLst = Validate(uploadCaseFilesCommand);
            foreach(var kvp in tDefinitionsLst)
            {
                var caseFileId = Guid.NewGuid().ToString();
                var caseDefinitionLst = CMMNParser.ExtractWorkflowDefinition(kvp.Value, caseFileId);
                foreach(var cd in caseDefinitionLst)
                {
                    cd.CaseOwner = uploadCaseFilesCommand.NameIdentifier;
                    _caseDefinitionCommandRepository.Add(cd);
                }

                _caseFileCommandRepository.Add(new CaseFileDefinitionAggregate
                {
                    Id = caseFileId,
                    CreateDateTime = DateTime.UtcNow,
                    UpdateDateTime = DateTime.UtcNow,
                    Description = kvp.Key.Name,
                    Name = kvp.Key.Name,
                    Payload = kvp.Key.Content,
                    Owner = uploadCaseFilesCommand.NameIdentifier
                });
                result.Add(caseFileId);
            }

            await _caseFileCommandRepository.SaveChanges();
            await _caseDefinitionCommandRepository.SaveChanges();
            return result;
        }

        private Dictionary<UploadCaseFile, tDefinitions> Validate(UploadCaseFilesCommand uploadCaseFilesCommand)
        {
            if (uploadCaseFilesCommand.Files == null || !uploadCaseFilesCommand.Files.Any())
            {
                throw new AggregateValidationException(new Dictionary<string, string>
                {
                    { "validation", "at least one file must be passed" }
                });
            }

            var result = new Dictionary<UploadCaseFile, tDefinitions>();
            try
            {
                foreach (var file in uploadCaseFilesCommand.Files)
                {
                    result.Add(file, CMMNParser.ParseWSDL(file.Content));
                }
            }
            catch
            {
                throw new AggregateValidationException(new Dictionary<string, string>
                {
                    { "validation", "at least one file is not correct" }
                });
            }

            return result;
        }
    }
}
