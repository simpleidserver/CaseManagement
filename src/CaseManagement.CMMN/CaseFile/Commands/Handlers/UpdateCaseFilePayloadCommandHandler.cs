using CaseManagement.CMMN.CaseFile.Exceptions;
using CaseManagement.CMMN.Domains;
using CaseManagement.Common;
using CaseManagement.Common.EvtStore;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CaseFile.Commands.Handlers
{
    public class UpdateCaseFilePayloadCommandHandler : IRequestHandler<UpdateCaseFilePayloadCommand, bool>
    {
        private readonly IEventStoreRepository _eventStoreRepository;
        private readonly ICommitAggregateHelper _commitAggregateHelper;

        public UpdateCaseFilePayloadCommandHandler(IEventStoreRepository eventStoreRepository, ICommitAggregateHelper commitAggregateHelper)
        {
            _eventStoreRepository = eventStoreRepository;
            _commitAggregateHelper = commitAggregateHelper;
        }

        public async Task<bool> Handle(UpdateCaseFilePayloadCommand request, CancellationToken cancellationToken)
        {
            var caseFile = await _eventStoreRepository.GetLastAggregate<CaseFileAggregate>(request.Id, CaseFileAggregate.GetStreamName(request.Id));
            if (caseFile == null || string.IsNullOrWhiteSpace(caseFile.AggregateId))
            {
                throw new UnknownCaseFileException(request.Id);
            }

            caseFile.UpdatePayload(request.Payload);
            await _commitAggregateHelper.Commit(caseFile, CaseFileAggregate.GetStreamName(caseFile.AggregateId), cancellationToken);
            return true;
        }
    }
}
