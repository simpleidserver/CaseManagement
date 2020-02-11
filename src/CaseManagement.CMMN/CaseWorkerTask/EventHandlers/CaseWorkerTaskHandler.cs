using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Domains.Events;
using CaseManagement.CMMN.Infrastructures;
using CaseManagement.CMMN.Infrastructures.Bus;
using CaseManagement.CMMN.Persistence;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CaseWorkerTask.EventHandlers
{
    public class CaseWorkerTaskHandler : IMessageBrokerConsumerGeneric<CaseWorkerTaskAddedEvent>, IMessageBrokerConsumerGeneric<CaseWorkerTaskConfirmedEvent>
    {
        private readonly ICaseWorkerTaskCommandRepository _caseWorkerTaskCommandRepository;
        private readonly ICaseWorkerTaskQueryRepository _caseWorkerTaskQueryRepository;

        public CaseWorkerTaskHandler(ICaseWorkerTaskCommandRepository caseWorkerTaskCommandRepository, ICaseWorkerTaskQueryRepository caseWorkerTaskQueryRepository)
        {
            _caseWorkerTaskCommandRepository = caseWorkerTaskCommandRepository;
            _caseWorkerTaskQueryRepository = caseWorkerTaskQueryRepository;
        }

        public string QueueName => CMMNConstants.QueueNames.CaseWorkerTasks;

        public async Task Handle(CaseWorkerTaskAddedEvent @event, CancellationToken cancellationToken)
        {
            var caseWorkerTask = CaseWorkerTaskAggregate.New(new List<DomainEvent> { @event });
            _caseWorkerTaskCommandRepository.Add(caseWorkerTask);
            await _caseWorkerTaskCommandRepository.SaveChanges();
        }

        public async Task Handle(CaseWorkerTaskConfirmedEvent @event, CancellationToken cancellationToken)
        {
            var caseWorkerTask = await _caseWorkerTaskQueryRepository.FindById(@event.AggregateId);
            caseWorkerTask.Handle(@event);
            _caseWorkerTaskCommandRepository.Update(caseWorkerTask);
            await _caseWorkerTaskCommandRepository.SaveChanges();
        }
    }
}
