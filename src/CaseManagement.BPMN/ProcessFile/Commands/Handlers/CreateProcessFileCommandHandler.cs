using CaseManagement.BPMN.Domains;
using CaseManagement.BPMN.Persistence;
using CaseManagement.BPMN.ProcessFile.Results;
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
        private readonly IProcessFileCommandRepository _processFileCommandRepository;
        private readonly BPMNServerOptions _options;

        public CreateProcessFileCommandHandler(
            ILogger<CreateProcessFileCommandHandler> logger,
            IProcessFileCommandRepository processFileCommandRepository,
            IOptions<BPMNServerOptions> options)
        {
            _logger = logger;
            _processFileCommandRepository = processFileCommandRepository;
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
            await _processFileCommandRepository.Add(processFile, cancellationToken);
            await _processFileCommandRepository.SaveChanges(cancellationToken);
            _logger.LogInformation("Process file has been added");
            return new CreateProcessFileResult
            {
                Id = processFile.AggregateId
            };
        }
    }
}
