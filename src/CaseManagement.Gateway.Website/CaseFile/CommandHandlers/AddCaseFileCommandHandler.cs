﻿using System.Threading.Tasks;
using CaseManagement.Gateway.Website.CaseFile.Commands;
using CaseManagement.Gateway.Website.CaseFile.Services;

namespace CaseManagement.Gateway.Website.CaseFile.CommandHandlers
{
    public class AddCaseFileCommandHandler : IAddCaseFileCommandHandler
    {
        private readonly ICaseFileService _caseFileService;

        public AddCaseFileCommandHandler(ICaseFileService caseFileService)
        {
            _caseFileService = caseFileService;
        }

        public Task<string> Handle(AddCaseFileCommand command)
        {
            return _caseFileService.Add(command);
        }
    }
}
