using CaseManagement.CMMN.Domains.Events;
using CaseManagement.CMMN.Infrastructures;
using CaseManagement.CMMN.Infrastructures.Bus;
using CaseManagement.CMMN.Persistence;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CasePlanInstance.EventHandlers
{
    public class CasePlanInstanceHandler : IMessageBrokerConsumerGeneric<CaseInstanceCreatedEvent>, IMessageBrokerConsumerGeneric<CaseElementCreatedEvent>, IMessageBrokerConsumerGeneric<CaseElementFinishedEvent>,
        IMessageBrokerConsumerGeneric<CaseElementStartedEvent>, IMessageBrokerConsumerGeneric<CaseElementTransitionRaisedEvent>, IMessageBrokerConsumerGeneric<CaseTransitionRaisedEvent>, IMessageBrokerConsumerGeneric<CaseInstanceVariableAddedEvent>
    {
        private readonly ICasePlanInstanceCommandRepository _cmmnWorkflowInstanceCommandRepository;
        private readonly ICasePlanInstanceQueryRepository _cmmnWorkflowInstanceQueryRepository;

        public CasePlanInstanceHandler(ICasePlanInstanceCommandRepository cmmnWorkflowInstanceCommandRepository, ICasePlanInstanceQueryRepository cmmnWorkflowInstanceQueryRepository)
        {
            _cmmnWorkflowInstanceCommandRepository = cmmnWorkflowInstanceCommandRepository;
            _cmmnWorkflowInstanceQueryRepository = cmmnWorkflowInstanceQueryRepository;
        }

        public string QueueName => CMMNConstants.QueueNames.CasePlanInstances;

        public async Task Handle(CaseInstanceCreatedEvent @event, CancellationToken cancellationToken)
        {
            var processFlowInstance = Domains.CasePlanInstanceAggregate.New(new List<DomainEvent>
            {
                @event
            });
            _cmmnWorkflowInstanceCommandRepository.Add(processFlowInstance);
            await _cmmnWorkflowInstanceCommandRepository.SaveChanges();
        }

        public async Task Handle(CaseElementCreatedEvent @event, CancellationToken cancellationToken)
        {
            var flowInstance = await _cmmnWorkflowInstanceQueryRepository.FindFlowInstanceById(@event.AggregateId);
            flowInstance.Handle(@event);
            _cmmnWorkflowInstanceCommandRepository.Update(flowInstance);
            await _cmmnWorkflowInstanceCommandRepository.SaveChanges();
        }

        public async Task Handle(CaseElementFinishedEvent @event, CancellationToken cancellationToken)
        {
            var flowInstance = await _cmmnWorkflowInstanceQueryRepository.FindFlowInstanceById(@event.AggregateId);
            flowInstance.Handle(@event);
            _cmmnWorkflowInstanceCommandRepository.Update(flowInstance);
            await _cmmnWorkflowInstanceCommandRepository.SaveChanges();
        }

        public async Task Handle(CaseElementStartedEvent @event, CancellationToken cancellationToken)
        {
            var flowInstance = await _cmmnWorkflowInstanceQueryRepository.FindFlowInstanceById(@event.AggregateId);
            flowInstance.Handle(@event);
            _cmmnWorkflowInstanceCommandRepository.Update(flowInstance);
            await _cmmnWorkflowInstanceCommandRepository.SaveChanges();
        }

        public async Task Handle(CaseElementTransitionRaisedEvent @event, CancellationToken cancellationToken)
        {
            var flowInstance = await _cmmnWorkflowInstanceQueryRepository.FindFlowInstanceById(@event.AggregateId);
            flowInstance.Handle(@event);
            _cmmnWorkflowInstanceCommandRepository.Update(flowInstance);
            await _cmmnWorkflowInstanceCommandRepository.SaveChanges();
        }

        public async Task Handle(CaseTransitionRaisedEvent @event, CancellationToken cancellationToken)
        {
            var flowInstance = await _cmmnWorkflowInstanceQueryRepository.FindFlowInstanceById(@event.AggregateId);
            flowInstance.Handle(@event);
            _cmmnWorkflowInstanceCommandRepository.Update(flowInstance);
            await _cmmnWorkflowInstanceCommandRepository.SaveChanges();
        }

        public async Task Handle(CaseInstanceVariableAddedEvent @event, CancellationToken cancellationToken)
        {
            var flowInstance = await _cmmnWorkflowInstanceQueryRepository.FindFlowInstanceById(@event.AggregateId);
            flowInstance.Handle(@event);
            _cmmnWorkflowInstanceCommandRepository.Update(flowInstance);
            await _cmmnWorkflowInstanceCommandRepository.SaveChanges();
        }
    }
}
