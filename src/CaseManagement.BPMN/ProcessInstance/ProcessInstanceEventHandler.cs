using CaseManagement.BPMN.Domains;
using CaseManagement.BPMN.Persistence;
using CaseManagement.BPMN.Persistence.InMemory;
using CaseManagement.Common.Domains;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.BPMN.ProcessInstance
{
    public class ProcessInstanceEventHandler : IDomainEvtConsumerGeneric<ProcessInstanceCreatedEvent>,
        IDomainEvtConsumerGeneric<FlowNodeCreatedEvent>,
        IDomainEvtConsumerGeneric<FlowNodeTransitionRaisedEvent>
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

        public async Task Handle(FlowNodeCreatedEvent message, CancellationToken token)
        {
            var record = await _processInstanceQueryRepository.Get(message.AggregateId, token);
            record.Handle(message);
            await _processInstanceCommandRepository.Update(record, token);
            await _processInstanceCommandRepository.SaveChanges(token);
        }

        public async Task Handle(FlowNodeTransitionRaisedEvent message, CancellationToken token)
        {
            var record = await _processInstanceQueryRepository.Get(message.AggregateId, token);
            record.Handle(message);
            await _processInstanceCommandRepository.Update(record, token);
            await _processInstanceCommandRepository.SaveChanges(token);
        }
    }
}
