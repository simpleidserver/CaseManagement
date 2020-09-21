using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Persistence;
using CaseManagement.Common.Domains;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CasePlanInstance
{
    public class CasePlanInstanceEventHandler : IDomainEvtConsumerGeneric<CasePlanInstanceCreatedEvent>,
        IDomainEvtConsumerGeneric<CaseElementTransitionRaisedEvent>,
        IDomainEvtConsumerGeneric<CaseInstanceWorkerTaskAddedEvent>,
        IDomainEvtConsumerGeneric<CaseInstanceRoleUpdatedEvent>,
        IDomainEvtConsumerGeneric<CaseTransitionRaisedEvent>,
        IDomainEvtConsumerGeneric<CaseFileItemAddedEvent>,
        IDomainEvtConsumerGeneric<CaseInstanceWorkerTaskRemovedEvent>,
        IDomainEvtConsumerGeneric<VariableUpdatedEvent>,
        IDomainEvtConsumerGeneric<CasePlanItemInstanceCreatedEvent>
    {
        private readonly ICasePlanInstanceCommandRepository _casePlanInstanceCommandRepository;
        private readonly ICasePlanInstanceQueryRepository _casePlanInstanceQueryRepository;

        public CasePlanInstanceEventHandler(ICasePlanInstanceCommandRepository casePlanInstanceCommandRepository, ICasePlanInstanceQueryRepository casePlanInstanceQueryRepository)
        {
            _casePlanInstanceCommandRepository = casePlanInstanceCommandRepository;
            _casePlanInstanceQueryRepository = casePlanInstanceQueryRepository;
        }

        public async Task Handle(CasePlanInstanceCreatedEvent message, CancellationToken token)
        {
            var record = CasePlanInstanceAggregate.New(new List<DomainEvent> { message });
            await _casePlanInstanceCommandRepository.Add(record, token);
            await _casePlanInstanceCommandRepository.SaveChanges(token);
        }

        public async Task Handle(CaseElementTransitionRaisedEvent message, CancellationToken token)
        {
            var record = await _casePlanInstanceQueryRepository.Get(message.AggregateId, token);
            record.Handle(message);
            await _casePlanInstanceCommandRepository.Update(record, token);
            await _casePlanInstanceCommandRepository.SaveChanges(token);
        }

        public async Task Handle(CaseInstanceWorkerTaskAddedEvent message, CancellationToken token)
        {
            var record = await _casePlanInstanceQueryRepository.Get(message.AggregateId, token);
            record.Handle(message);
            await _casePlanInstanceCommandRepository.Update(record, token);
            await _casePlanInstanceCommandRepository.SaveChanges(token);
        }

        public async Task Handle(CaseInstanceRoleUpdatedEvent message, CancellationToken token)
        {
            var record = await _casePlanInstanceQueryRepository.Get(message.AggregateId, token);
            record.Handle(message);
            await _casePlanInstanceCommandRepository.Update(record, token);
            await _casePlanInstanceCommandRepository.SaveChanges(token);
        }

        public async Task Handle(CaseTransitionRaisedEvent message, CancellationToken token)
        {
            var record = await _casePlanInstanceQueryRepository.Get(message.AggregateId, token);
            record.Handle(message);
            await _casePlanInstanceCommandRepository.Update(record, token);
            await _casePlanInstanceCommandRepository.SaveChanges(token);
        }

        public async Task Handle(CaseFileItemAddedEvent message, CancellationToken token)
        {
            var record = await _casePlanInstanceQueryRepository.Get(message.AggregateId, token);
            record.Handle(message);
            await _casePlanInstanceCommandRepository.Update(record, token);
            await _casePlanInstanceCommandRepository.SaveChanges(token);
        }

        public async Task Handle(CaseInstanceWorkerTaskRemovedEvent message, CancellationToken token)
        {
            var record = await _casePlanInstanceQueryRepository.Get(message.AggregateId, token);
            record.Handle(message);
            await _casePlanInstanceCommandRepository.Update(record, token);
            await _casePlanInstanceCommandRepository.SaveChanges(token);
        }

        public async Task Handle(VariableUpdatedEvent message, CancellationToken token)
        {
            var record = await _casePlanInstanceQueryRepository.Get(message.AggregateId, token);
            record.Handle(message);
            await _casePlanInstanceCommandRepository.Update(record, token);
            await _casePlanInstanceCommandRepository.SaveChanges(token);
        }

        public async Task Handle(CasePlanItemInstanceCreatedEvent message, CancellationToken token)
        {
            var record = await _casePlanInstanceQueryRepository.Get(message.AggregateId, token);
            record.Handle(message);
            await _casePlanInstanceCommandRepository.Update(record, token);
            await _casePlanInstanceCommandRepository.SaveChanges(token);
        }
    }
}
