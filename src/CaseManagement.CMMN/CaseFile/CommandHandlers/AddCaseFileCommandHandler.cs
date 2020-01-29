using CaseManagement.CMMN.CaseFile.Commands;
using CaseManagement.CMMN.Domains.CaseFile;
using CaseManagement.CMMN.Infrastructures;
using CaseManagement.CMMN.Parser;
using CaseManagement.CMMN.Persistence;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CaseFile.CommandHandlers
{
    public class AddCaseFileCommandHandler : IAddCaseFileCommandHandler
    {
        private readonly CMMNServerOptions _options;
        private readonly ICaseDefinitionCommandRepository _caseDefinitionCommandRepository;
        private readonly ICaseFileCommandRepository _caseFileCommandRepository;

        public AddCaseFileCommandHandler(IOptions<CMMNServerOptions> options, ICaseDefinitionCommandRepository caseDefinitionCommandRepository, ICaseFileCommandRepository caseFileCommandRepository)
        {
            _options = options.Value;
            _caseDefinitionCommandRepository = caseDefinitionCommandRepository;
            _caseFileCommandRepository = caseFileCommandRepository;
        }

        public async Task<string> Handle(AddCaseFileCommand uploadCaseFileCommand)
        {
            Validate(uploadCaseFileCommand);
            var caseFileId = Guid.NewGuid().ToString();
            var tDefinitions = CMMNParser.ParseWSDL(_options.DefaultCMMNSchema);
            foreach (var record in CMMNParser.ExtractWorkflowDefinition(tDefinitions, caseFileId))
            {
                record.CaseOwner = uploadCaseFileCommand.NameIdentifier;
                _caseDefinitionCommandRepository.Add(record);
            }

            _caseFileCommandRepository.Add(new CaseFileDefinitionAggregate
            {
                Id = caseFileId,
                CreateDateTime = DateTime.UtcNow,
                UpdateDateTime = DateTime.UtcNow,
                Description = uploadCaseFileCommand.Description,
                Name = uploadCaseFileCommand.Name,
                Payload = _options.DefaultCMMNSchema,
                Owner = uploadCaseFileCommand.NameIdentifier
            });

            await _caseDefinitionCommandRepository.SaveChanges();
            await _caseFileCommandRepository.SaveChanges();
            return caseFileId;
        }

        private void Validate(AddCaseFileCommand addCaseFileCommand)
        {
            var errors = new Dictionary<string, string>();
            if (string.IsNullOrWhiteSpace(addCaseFileCommand.Name))
            {
                throw new AggregateValidationException(new Dictionary<string, string>
                {
                    { "validation", "name must be specified" }
                });
            }

            if (string.IsNullOrWhiteSpace(addCaseFileCommand.Description))
            {
                throw new AggregateValidationException(new Dictionary<string, string>
                {
                    { "validation", "description must be specified" }
                });
            }
        }
    }
}
