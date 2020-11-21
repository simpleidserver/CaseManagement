using CaseManagement.BPMN.Domains;
using CaseManagement.BPMN.Exceptions;
using CaseManagement.BPMN.ProcessFile.Results;
using CaseManagement.Common;
using CaseManagement.Common.EvtStore;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.BPMN.ProcessFile.Commands.Handlers
{
    public class PublishProcessFileCommandHandler : IRequestHandler<PublishProcessFileCommand, PublishProcessFileResult>
    {
        private readonly IEventStoreRepository _eventStoreRepository;
        private readonly ICommitAggregateHelper _commitAggregateHelper;

        public PublishProcessFileCommandHandler(IEventStoreRepository eventStoreRepository, ICommitAggregateHelper commitAggregateHelper)
        {
            _eventStoreRepository = eventStoreRepository;
            _commitAggregateHelper = commitAggregateHelper;
        }

        public async Task<PublishProcessFileResult> Handle(PublishProcessFileCommand request, CancellationToken cancellationToken)
        {
            var processFile = await _eventStoreRepository.GetLastAggregate<ProcessFileAggregate>(request.Id, ProcessFileAggregate.GetStreamName(request.Id));
            if (request == null || string.IsNullOrWhiteSpace(processFile.AggregateId))
            {
                throw new UnknownProcessFileException(request.Id);
            }

            var newProcessFile = processFile.Publish();
            await _commitAggregateHelper.Commit(processFile, ProcessFileAggregate.GetStreamName(processFile.AggregateId), cancellationToken);
            await _commitAggregateHelper.Commit(newProcessFile, ProcessFileAggregate.GetStreamName(newProcessFile.AggregateId), cancellationToken);
            return new PublishProcessFileResult
            {
                Id = newProcessFile.AggregateId
            };
        }
    }
}
