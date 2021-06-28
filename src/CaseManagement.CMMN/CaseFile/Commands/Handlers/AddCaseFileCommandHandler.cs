using CaseManagement.CMMN.CaseFile.Commands;
using CaseManagement.CMMN.CaseFile.Results;
using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Persistence;
using MediatR;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CaseFile.Command.Handlers
{
    public class AddCaseFileCommandHandler : IRequestHandler<AddCaseFileCommand, CreateCaseFileResult>
    {
        private readonly ICaseFileCommandRepository _caseFileCommandRepository;
        private readonly CMMNServerOptions _options;

        public AddCaseFileCommandHandler(
            ICaseFileCommandRepository caseFileCommandRepository,
            IOptions<CMMNServerOptions> options)
        {
            _caseFileCommandRepository = caseFileCommandRepository;
            _options = options.Value;
        }

        public async Task<CreateCaseFileResult> Handle(AddCaseFileCommand addCaseFileCommand, CancellationToken token)
        {
            var payload = addCaseFileCommand.Payload;
            if (string.IsNullOrWhiteSpace(addCaseFileCommand.Payload))
            {
                payload = _options.DefaultCMMNSchema;
                payload = payload.Replace("{id}", $"CasePlanModel_{Guid.NewGuid()}");
            }

            var caseFile = CaseFileAggregate.New(addCaseFileCommand.Name, addCaseFileCommand.Description, 0, payload);
            await _caseFileCommandRepository.Add(caseFile, token);
            await _caseFileCommandRepository.SaveChanges(token);
            return new CreateCaseFileResult
            {
                Id = caseFile.AggregateId
            };
        }
    }
}