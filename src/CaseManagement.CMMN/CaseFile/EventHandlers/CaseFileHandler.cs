using CaseManagement.CMMN.Domains.Events;
using CaseManagement.CMMN.Infrastructures;
using CaseManagement.CMMN.Infrastructures.Bus;
using CaseManagement.CMMN.Infrastructures.Lock;
using CaseManagement.CMMN.Persistence;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CaseFile.EventHandlers
{
    public class CaseFileHandler : IMessageBrokerConsumerGeneric<CaseFileAddedEvent>, IMessageBrokerConsumerGeneric<CaseFileUpdatedEvent>, IMessageBrokerConsumerGeneric<CaseFilePublishedEvent>
    {
        private readonly ICaseFileCommandRepository _caseFileCommandRepository;
        private readonly ICaseFileQueryRepository _caseFileQueryRepository;
        private readonly IDistributedLock _distributedLock;

        public CaseFileHandler(ICaseFileCommandRepository caseFileCommandRepository, ICaseFileQueryRepository caseFileQueryRepository, IDistributedLock distributedLock)
        {
            _caseFileCommandRepository = caseFileCommandRepository;
            _caseFileQueryRepository = caseFileQueryRepository;
            _distributedLock = distributedLock;
        }

        public string QueueName => CMMNConstants.QueueNames.CaseFiles;

        public async Task Handle(CaseFileAddedEvent @event, CancellationToken cancellationToken)
        {
            var lockId = $"add-casefile-{@event.Id}";
            if (!await _distributedLock.AcquireLock(lockId))
            {
                return;
            }

            try
            {
                var caseFile = Domains.CaseFileAggregate.New(new List<DomainEvent>
                {
                    @event
                });
                _caseFileCommandRepository.Add(caseFile);
                await _caseFileCommandRepository.SaveChanges();
            }
            finally
            {
                await _distributedLock.ReleaseLock(lockId);
            }
        }

        public async Task Handle(CaseFileUpdatedEvent @event, CancellationToken cancellationToken)
        {
            var lockId = $"update-casefile-{@event.Id}";
            if (!await _distributedLock.AcquireLock(lockId))
            {
                return;
            }

            try
            {
                var caseFile = await _caseFileQueryRepository.FindById(@event.AggregateId);
                caseFile.Handle(@event);
                _caseFileCommandRepository.Update(caseFile);
                await _caseFileCommandRepository.SaveChanges();
            }
            finally
            {
                await _distributedLock.ReleaseLock(lockId);
            }
        }

        public async Task Handle(CaseFilePublishedEvent @event, CancellationToken cancellationToken)
        {
            var lockId = $"publish-casefile-{@event.Id}";
            if (!await _distributedLock.AcquireLock(lockId))
            {
                return;
            }

            try
            {
                var caseFile = await _caseFileQueryRepository.FindById(@event.AggregateId);
                caseFile.Handle(@event);
                _caseFileCommandRepository.Update(caseFile);
                await _caseFileCommandRepository.SaveChanges();
            }
            finally
            {
                await _distributedLock.ReleaseLock(lockId);
            }
        }
    }
}
