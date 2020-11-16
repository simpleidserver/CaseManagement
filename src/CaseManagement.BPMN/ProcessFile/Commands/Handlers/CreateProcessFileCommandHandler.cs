using CaseManagement.BPMN.Domains;
using CaseManagement.BPMN.ProcessFile.Results;
using CaseManagement.Common;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.BPMN.ProcessFile.Commands.Handlers
{
    public class CreateProcessFileCommandHandler : IRequestHandler<CreateProcessFileCommand, CreateProcessFileResult>
    {
        private readonly ILogger<CreateProcessFileCommandHandler> _logger;
        private readonly ICommitAggregateHelper _commitAggregateHelper;
        private readonly BPMNServerOptions _options;

        public CreateProcessFileCommandHandler(
            ILogger<CreateProcessFileCommandHandler> logger,
            ICommitAggregateHelper commitAggregateHelper,
            IOptions<BPMNServerOptions> options)
        {
            _logger = logger;
            _commitAggregateHelper = commitAggregateHelper;
            _options = options.Value;
        }

        public async Task<CreateProcessFileResult> Handle(CreateProcessFileCommand request, CancellationToken cancellationToken)
        {
            var payload = request.Payload;
            if (string.IsNullOrWhiteSpace(request.Payload))
            {
                payload = _options.DefaultBPMNFile;
            }

            var processFile = ProcessFileAggregate.New(Guid.NewGuid().ToString(), request.Name, request.Description, 0, payload);
            var streamName = ProcessFileAggregate.GetStreamName(processFile.AggregateId);
            await _commitAggregateHelper.Commit(processFile, streamName, cancellationToken);
            _logger.LogInformation("Process file has been added");
            return new CreateProcessFileResult
            {
                Id = processFile.AggregateId
            };
        }
    }
}
