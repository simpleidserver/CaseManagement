using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Infrastructures.DomainEvts;
using CaseManagement.CMMN.Persistence;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CaseFile.EventHandlers
{
    public class CaseFileHandler : IDomainEvtConsumerGeneric<CaseFileAddedEvent>, IDomainEvtConsumerGeneric<CaseFileUpdatedEvent>, IDomainEvtConsumerGeneric<CaseFilePublishedEvent>
    {
        private readonly ICaseFileCommandRepository _caseFileCommandRepository;
        private readonly ICaseFileQueryRepository _caseFileQueryRepository;

        public CaseFileHandler(ICaseFileCommandRepository caseFileCommandRepository, ICaseFileQueryRepository caseFileQueryRepository)
        {
            _caseFileCommandRepository = caseFileCommandRepository;
            _caseFileQueryRepository = caseFileQueryRepository;
        }

        public async Task Handle(CaseFileAddedEvent @event, CancellationToken cancellationToken)
        {
            var caseFile = Domains.CaseFileAggregate.New(new List<DomainEvent>
                {
                    @event
                });
            await _caseFileCommandRepository.Add(caseFile, cancellationToken);
            await _caseFileCommandRepository.SaveChanges(cancellationToken);
        }

        public async Task Handle(CaseFileUpdatedEvent @event, CancellationToken cancellationToken)
        {
            var caseFile = await _caseFileQueryRepository.Get(@event.AggregateId, cancellationToken);
            caseFile.Handle(@event);
            await _caseFileCommandRepository.Update(caseFile, cancellationToken);
            await _caseFileCommandRepository.SaveChanges(cancellationToken);
        }

        public async Task Handle(CaseFilePublishedEvent @event, CancellationToken cancellationToken)
        {
            var caseFile = await _caseFileQueryRepository.Get(@event.AggregateId, cancellationToken);
            caseFile.Handle(@event);
            await _caseFileCommandRepository.Update(caseFile, cancellationToken);
            await _caseFileCommandRepository.SaveChanges(cancellationToken);
        }
    }
}
