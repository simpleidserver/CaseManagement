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
    public class UpdateProcessFilePayloadCommandHandler : IRequestHandler<UpdateProcessFilePayloadCommand, bool>
    {
        private readonly IEventStoreRepository _eventStoreRepository;
        private readonly ICommitAggregateHelper _commitAggregateHelper;
        private readonly ILogger<UpdateProcessFilePayloadCommandHandler> _logger;

        public UpdateProcessFilePayloadCommandHandler(
            IEventStoreRepository eventStoreRepository,
            ICommitAggregateHelper commitAggregateHelper,
            ILogger<UpdateProcessFilePayloadCommandHandler> logger)
        {
            _eventStoreRepository = eventStoreRepository;
            _commitAggregateHelper = commitAggregateHelper;
            _logger = logger;
        }

        public async Task<bool> Handle(UpdateProcessFilePayloadCommand request, CancellationToken cancellationToken)
        {
            var processFile = await _eventStoreRepository.GetLastAggregate<ProcessFileAggregate>(request.Id, ProcessFileAggregate.GetStreamName(request.Id));
            if (processFile == null || string.IsNullOrWhiteSpace(processFile.AggregateId))
            {
                _logger.LogError($"Cannot update process file because it doesn't exist : '{request.Id}'");
                throw new UnknownProcessFileException(request.Id);
            }

            processFile.UpdatePayload(request.Payload);
            await _commitAggregateHelper.Commit(processFile, ProcessFileAggregate.GetStreamName(request.Id), cancellationToken);
            return true;
        }
    }
}
