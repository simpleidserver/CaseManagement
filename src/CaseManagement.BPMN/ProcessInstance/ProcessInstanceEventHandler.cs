using CaseManagement.BPMN.Domains;
using CaseManagement.BPMN.Persistence;
using CaseManagement.Common.Domains;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.BPMN.ProcessInstance
{
    public class ProcessInstanceEventHandler : IDomainEvtConsumerGeneric<ProcessInstanceCreatedEvent>,
        IDomainEvtConsumerGeneric<FlowNodeDefCreatedEvent>,
        IDomainEvtConsumerGeneric<ActivityStateUpdatedEvent>,
        IDomainEvtConsumerGeneric<ExecutionPathCreatedEvent>,
        IDomainEvtConsumerGeneric<ExecutionPointerAddedEvent>,
        IDomainEvtConsumerGeneric<ExecutionPointerCompletedEvent>,
        IDomainEvtConsumerGeneric<FlowNodeInstanceAddedEvent>,
        IDomainEvtConsumerGeneric<IncomingTokenAddedEvent>,
        IDomainEvtConsumerGeneric<FlowNodeInstanceCompletedEvent>
    {
        private readonly IProcessInstanceCommandRepository _processInstanceCommandRepository;
        private readonly IProcessInstanceQueryRepository _processInstanceQueryRepository;

        public ProcessInstanceEventHandler(IProcessInstanceCommandRepository processInstanceCommandRepository, IProcessInstanceQueryRepository processInstanceQueryRepository)
        {
            _processInstanceCommandRepository = processInstanceCommandRepository;
            _processInstanceQueryRepository = processInstanceQueryRepository;
        }

        public async Task Handle(ProcessInstanceCreatedEvent message, CancellationToken token)
        {
            var record = ProcessInstanceAggregate.New(new List<DomainEvent>
            {
                message
            });
            await _processInstanceCommandRepository.Add(record, token);
            await _processInstanceCommandRepository.SaveChanges(token);
        }

        public async Task Handle(FlowNodeDefCreatedEvent message, CancellationToken token)
        {
            var record = await _processInstanceQueryRepository.Get(message.AggregateId, token);
            record.Handle(message);
            await _processInstanceCommandRepository.Update(record, token);
            await _processInstanceCommandRepository.SaveChanges(token);
        }

        public async Task Handle(ActivityStateUpdatedEvent message, CancellationToken token)
        {
            var record = await _processInstanceQueryRepository.Get(message.AggregateId, token);
            record.Handle(message);
            await _processInstanceCommandRepository.Update(record, token);
            await _processInstanceCommandRepository.SaveChanges(token);
        }

        public async Task Handle(ExecutionPathCreatedEvent message, CancellationToken token)
        {
            var record = await _processInstanceQueryRepository.Get(message.AggregateId, token);
            record.Handle(message);
            await _processInstanceCommandRepository.Update(record, token);
            await _processInstanceCommandRepository.SaveChanges(token);
        }

        public async Task Handle(ExecutionPointerAddedEvent message, CancellationToken token)
        {
            var record = await _processInstanceQueryRepository.Get(message.AggregateId, token);
            record.Handle(message);
            await _processInstanceCommandRepository.Update(record, token);
            await _processInstanceCommandRepository.SaveChanges(token);
        }

        public async Task Handle(ExecutionPointerCompletedEvent message, CancellationToken token)
        {
            var record = await _processInstanceQueryRepository.Get(message.AggregateId, token);
            record.Handle(message);
            await _processInstanceCommandRepository.Update(record, token);
            await _processInstanceCommandRepository.SaveChanges(token);
        }

        public async Task Handle(FlowNodeInstanceAddedEvent message, CancellationToken token)
        {
            var record = await _processInstanceQueryRepository.Get(message.AggregateId, token);
            record.Handle(message);
            await _processInstanceCommandRepository.Update(record, token);
            await _processInstanceCommandRepository.SaveChanges(token);
        }

        public async Task Handle(IncomingTokenAddedEvent message, CancellationToken token)
        {
            var record = await _processInstanceQueryRepository.Get(message.AggregateId, token);
            record.Handle(message);
            await _processInstanceCommandRepository.Update(record, token);
            await _processInstanceCommandRepository.SaveChanges(token);
        }

        public async Task Handle(FlowNodeInstanceCompletedEvent message, CancellationToken token)
        {
            var record = await _processInstanceQueryRepository.Get(message.AggregateId, token);
            record.Handle(message);
            await _processInstanceCommandRepository.Update(record, token);
            await _processInstanceCommandRepository.SaveChanges(token);
        }
    }
}
