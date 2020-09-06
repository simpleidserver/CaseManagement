using CaseManagement.CMMN.CaseFile.Commands;
using CaseManagement.CMMN.CaseFile.Exceptions;
using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Infrastructures;
using CaseManagement.CMMN.Infrastructures.EvtStore;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CaseFile.CommandHandlers
{
    public class PublishCaseFileCommandHandler : IPublishCaseFileCommandHandler
    {
        private readonly IEventStoreRepository _eventStoreRepository;
        private readonly ICommitAggregateHelper _commitAggregateHelper;

        public PublishCaseFileCommandHandler(IEventStoreRepository eventStoreRepository, ICommitAggregateHelper commitAggregateHelper)
        {
            _eventStoreRepository = eventStoreRepository;
            _commitAggregateHelper = commitAggregateHelper;
        }

        public async Task<string> Handle(PublishCaseFileCommand publishCaseFileCommand, CancellationToken cancellationToken)
        {
            var caseFile = await _eventStoreRepository.GetLastAggregate<CaseFileAggregate>(publishCaseFileCommand.Id, CaseFileAggregate.GetStreamName(publishCaseFileCommand.Id), cancellationToken);
            if (caseFile == null || string.IsNullOrWhiteSpace(caseFile.Id))
            {
                throw new UnknownCaseFileException(publishCaseFileCommand.Id);
            }

            var newCaseFile = caseFile.Publish();
            await _commitAggregateHelper.Commit(caseFile, CaseFileAggregate.GetStreamName(caseFile.Id), cancellationToken);
            await _commitAggregateHelper.Commit(newCaseFile, CaseFileAggregate.GetStreamName(newCaseFile.Id), cancellationToken);
            return newCaseFile.Id;
        }
    }
}
