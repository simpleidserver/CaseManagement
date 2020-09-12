using CaseManagement.CMMN.CaseFile.Commands;
using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Infrastructure;
using MediatR;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CaseFile.Command.Handlers
{
    public class AddCaseFileCommandHandler : IRequestHandler<AddCaseFileCommand, string>
    {
        private readonly ICommitAggregateHelper _commitAggregateHelper;
        private readonly CMMNServerOptions _options;

        public AddCaseFileCommandHandler(ICommitAggregateHelper commitAggregateHelper, IOptions<CMMNServerOptions> options)
        {
            _commitAggregateHelper = commitAggregateHelper;
            _options = options.Value;
        }

        public async Task<string> Handle(AddCaseFileCommand addCaseFileCommand, CancellationToken token)
        {
            var payload = addCaseFileCommand.Payload;
            if (string.IsNullOrWhiteSpace(addCaseFileCommand.Payload))
            {
                payload = _options.DefaultCMMNSchema;
                payload = payload.Replace("{id}", $"CasePlanModel_{Guid.NewGuid()}");
            }

            var caseFile = CaseFileAggregate.New(addCaseFileCommand.Name, addCaseFileCommand.Description, 0, addCaseFileCommand.Owner, payload);
            var streamName = CaseFileAggregate.GetStreamName(caseFile.AggregateId);
            await _commitAggregateHelper.Commit(caseFile, streamName, token);
            return caseFile.AggregateId;
        }
    }
}