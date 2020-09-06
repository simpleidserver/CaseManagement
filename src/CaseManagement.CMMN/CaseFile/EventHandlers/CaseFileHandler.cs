using CaseManagement.CMMN.Domains.Events;
using CaseManagement.CMMN.Infrastructures;
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
            _caseFileCommandRepository.Add(caseFile);
            await _caseFileCommandRepository.SaveChanges();
        }

        public async Task Handle(CaseFileUpdatedEvent @event, CancellationToken cancellationToken)
        {
            var caseFile = await _caseFileQueryRepository.Get(@event.AggregateId, cancellationToken);
            caseFile.Handle(@event);
            _caseFileCommandRepository.Update(caseFile);
            await _caseFileCommandRepository.SaveChanges();

        }

        public async Task Handle(CaseFilePublishedEvent @event, CancellationToken cancellationToken)
        {
            var caseFile = await _caseFileQueryRepository.Get(@event.AggregateId, cancellationToken);
            caseFile.Handle(@event);
            _caseFileCommandRepository.Update(caseFile);
            await _caseFileCommandRepository.SaveChanges();
        }
    }
}
