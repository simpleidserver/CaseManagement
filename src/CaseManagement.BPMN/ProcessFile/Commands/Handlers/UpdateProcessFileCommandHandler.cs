using CaseManagement.BPMN.Domains;
using CaseManagement.BPMN.Exceptions;
using CaseManagement.Common;
using CaseManagement.Common.EvtStore;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.BPMN.ProcessFile.Commands.Handlers
{
    public class UpdateProcessFileCommandHandler : IRequestHandler<UpdateProcessFileCommand, bool>
    {
        private readonly IEventStoreRepository _eventStoreRepository;
        private readonly ICommitAggregateHelper _commitAggregateHelper;
        private readonly ILogger<UpdateProcessFileCommandHandler> _logger;
        private readonly BPMNServerOptions _bpmnServerOpts;

        public UpdateProcessFileCommandHandler(
            IEventStoreRepository eventStoreRepository, 
            ICommitAggregateHelper commitAggregateHelper, 
            ILogger<UpdateProcessFileCommandHandler> logger,
            IOptions<BPMNServerOptions> bpmnServerOpts)
        {
            _eventStoreRepository = eventStoreRepository;
            _commitAggregateHelper = commitAggregateHelper;
            _logger = logger;
            _bpmnServerOpts = bpmnServerOpts.Value;
        }

        public async Task<bool> Handle(UpdateProcessFileCommand request, CancellationToken cancellationToken)
        {
            var processFile = await _eventStoreRepository.GetLastAggregate<ProcessFileAggregate>(request.Id, ProcessFileAggregate.GetStreamName(request.Id));
            if (processFile == null || string.IsNullOrWhiteSpace(processFile.AggregateId))
            {
                _logger.LogError($"Cannot update process file because it doesn't exist : '{request.Id}'");
                throw new UnknownProcessFileException(request.Id);
            }

            var payload = request.Payload;
            if (string.IsNullOrWhiteSpace(request.Payload))
            {
                payload = _bpmnServerOpts.DefaultBPMNFile;
            }

            processFile.Update(request.Name, request.Description, payload);
            await _commitAggregateHelper.Commit(processFile, ProcessFileAggregate.GetStreamName(request.Id), cancellationToken);
            return true;
        }
    }
}
