using CaseManagement.CMMN.CaseFile.Commands;
using CaseManagement.CMMN.CaseFile.Exceptions;
using CaseManagement.CMMN.Domains;
using CaseManagement.Common;
using CaseManagement.Common.EvtStore;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CaseFile.Command.Handlers
{
    public class UpdateCaseFileCommandHandler : IRequestHandler<UpdateCaseFileCommand, bool>
    {
        private readonly IEventStoreRepository _eventStoreRepository;
        private readonly ICommitAggregateHelper _commitAggregateHelper;

        public UpdateCaseFileCommandHandler(IEventStoreRepository eventStoreRepository, ICommitAggregateHelper commitAggregateHelper)
        {
            _eventStoreRepository = eventStoreRepository;
            _commitAggregateHelper = commitAggregateHelper;
        }

        public async Task<bool> Handle(UpdateCaseFileCommand command, CancellationToken token)
        {
            var caseFile = await _eventStoreRepository.GetLastAggregate<CaseFileAggregate>(command.Id, CaseFileAggregate.GetStreamName(command.Id));
            if (caseFile == null || string.IsNullOrWhiteSpace(caseFile.AggregateId))
            {
                throw new UnknownCaseFileException(command.Id);
            }

            caseFile.Update(command.Name, command.Description);
            await _commitAggregateHelper.Commit(caseFile, CaseFileAggregate.GetStreamName(caseFile.AggregateId), token);
            return true;
        }
    }
}
