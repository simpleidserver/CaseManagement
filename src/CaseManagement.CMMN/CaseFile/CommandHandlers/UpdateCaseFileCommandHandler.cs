using CaseManagement.CMMN.CaseFile.Commands;
using CaseManagement.CMMN.CaseFile.Exceptions;
using CaseManagement.CMMN.Infrastructures;
using CaseManagement.CMMN.Parser;
using CaseManagement.CMMN.Persistence;
using CaseManagement.CMMN.Persistence.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CaseFile.CommandHandlers
{
    public class UpdateCaseFileCommandHandler : IUpdateCaseFileCommandHandler
    {
        private readonly ICaseFileQueryRepository _caseFileQueryRepository;
        private readonly ICaseDefinitionQueryRepository _caseDefinitionQueryRepository;
        private readonly ICaseDefinitionCommandRepository _caseDefinitionCommandRepository;
        private readonly ICaseFileCommandRepository _caseFileCommandRepository;

        public UpdateCaseFileCommandHandler(ICaseFileQueryRepository caseFileQueryRepository, ICaseDefinitionQueryRepository caseDefinitionQueryRepository, ICaseDefinitionCommandRepository caseDefinitionCommandRepository, ICaseFileCommandRepository caseFileCommandRepository)
        {
            _caseFileQueryRepository = caseFileQueryRepository;
            _caseDefinitionQueryRepository = caseDefinitionQueryRepository;
            _caseDefinitionCommandRepository = caseDefinitionCommandRepository;
            _caseFileCommandRepository = caseFileCommandRepository;
        }

        public async Task<bool> Handle(UpdateCaseFileCommand command)
        {
            tDefinitions tDefinitions;
            try
            {
                tDefinitions = CMMNParser.ParseWSDL(command.Payload);
            }
            catch
            {
                throw new AggregateValidationException(new Dictionary<string, string>
                {
                    { "validation", "xml file is not valid" }
                });
            }

            var caseFile = await _caseFileQueryRepository.FindById(command.Id);
            if (caseFile == null)
            {
                throw new UnknownCaseFileException(command.Id);
            }

            if (caseFile.Owner != command.NameIdentifier)
            {
                throw new UnauthorizedCaseFileException(command.NameIdentifier, command.Id);
            }

            var caseDefinitions = await _caseDefinitionQueryRepository.Find(new FindWorkflowDefinitionsParameter
            {
                CaseFileId = caseFile.Id
            });
            foreach(var caseDefinition in caseDefinitions.Content)
            {
                _caseDefinitionCommandRepository.Delete(caseDefinition);
            }

            caseFile.Name = command.Name;
            caseFile.Description = command.Description;
            caseFile.Payload = command.Payload;
            caseFile.UpdateDateTime = DateTime.UtcNow;
            foreach (var record in CMMNParser.ExtractWorkflowDefinition(tDefinitions, caseFile.Id))
            {
                record.CaseOwner = command.NameIdentifier;
                _caseDefinitionCommandRepository.Add(record);
            }

            _caseFileCommandRepository.Update(caseFile);
            await _caseFileCommandRepository.SaveChanges();
            await _caseDefinitionCommandRepository.SaveChanges();
            return true;
        }
    }
}
