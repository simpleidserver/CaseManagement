using CaseManagement.CMMN.CaseFile.Commands;
using CaseManagement.CMMN.CaseFile.Exceptions;
using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Infrastructure;
using CaseManagement.CMMN.Infrastructure.EvtStore;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CaseFile.Command.Handlers
{
    public class PublishCaseFileCommandHandler : IRequestHandler<PublishCaseFileCommand, string>
    {
        private readonly IEventStoreRepository _eventStoreRepository;
        private readonly ICommitAggregateHelper _commitAggregateHelper;

        public PublishCaseFileCommandHandler(IEventStoreRepository eventStoreRepository, ICommitAggregateHelper commitAggregateHelper)
        {
            _eventStoreRepository = eventStoreRepository;
            _commitAggregateHelper = commitAggregateHelper;
        }

        public async Task<string> Handle(PublishCaseFileCommand publishCaseFileCommand, CancellationToken token)
        {
            var caseFile = await _eventStoreRepository.GetLastAggregate<CaseFileAggregate>(publishCaseFileCommand.Id, CaseFileAggregate.GetStreamName(publishCaseFileCommand.Id));
            if (caseFile == null || string.IsNullOrWhiteSpace(caseFile.AggregateId))
            {
                throw new UnknownCaseFileException(publishCaseFileCommand.Id);
            }

            var newCaseFile = caseFile.Publish();
            await _commitAggregateHelper.Commit(caseFile, CaseFileAggregate.GetStreamName(caseFile.AggregateId), token);
            await _commitAggregateHelper.Commit(newCaseFile, CaseFileAggregate.GetStreamName(newCaseFile.AggregateId), token);
            return newCaseFile.AggregateId;
        }
    }
}
