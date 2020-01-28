using CaseManagement.CMMN.Domains.Events;
using CaseManagement.CMMN.Infrastructures;
using CaseManagement.CMMN.Persistence;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CaseInstance.EventHandlers
{
    public class WorkflowInstanceHandler : IDomainEventHandler<CaseInstanceCreatedEvent>, IDomainEventHandler<CaseElementCreatedEvent>, IDomainEventHandler<CaseElementFinishedEvent>,
        IDomainEventHandler<CaseElementInstanceFormCreatedEvent>, IDomainEventHandler<CaseElementInstanceFormSubmittedEvent>, IDomainEventHandler<CaseElementStartedEvent>,
        IDomainEventHandler<CaseElementTransitionRaisedEvent>, IDomainEventHandler<CaseTransitionRaisedEvent>, IDomainEventHandler<CaseInstanceVariableAddedEvent>,
        IDomainEventHandler<CaseElementPlanificationConfirmedEvent>, IDomainEventHandler<CaseElementPlannedEvent>
    {
        private readonly ICaseInstanceCommandRepository _cmmnWorkflowInstanceCommandRepository;
        private readonly ICaseInstanceQueryRepository _cmmnWorkflowInstanceQueryRepository;

        public WorkflowInstanceHandler(ICaseInstanceCommandRepository cmmnWorkflowInstanceCommandRepository, ICaseInstanceQueryRepository cmmnWorkflowInstanceQueryRepository)
        {
            _cmmnWorkflowInstanceCommandRepository = cmmnWorkflowInstanceCommandRepository;
            _cmmnWorkflowInstanceQueryRepository = cmmnWorkflowInstanceQueryRepository;
        }

        public async Task Handle(CaseInstanceCreatedEvent @event, CancellationToken cancellationToken)
        {
            var processFlowInstance = Domains.CaseInstance.New(new List<DomainEvent>
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

        public async Task Handle(CaseElementInstanceFormCreatedEvent @event, CancellationToken cancellationToken)
        {
            var flowInstance = await _cmmnWorkflowInstanceQueryRepository.FindFlowInstanceById(@event.AggregateId);
            flowInstance.Handle(@event);
            _cmmnWorkflowInstanceCommandRepository.Update(flowInstance);
            await _cmmnWorkflowInstanceCommandRepository.SaveChanges();
        }

        public async Task Handle(CaseElementInstanceFormSubmittedEvent @event, CancellationToken cancellationToken)
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

        public async Task Handle(CaseElementPlanificationConfirmedEvent @event, CancellationToken cancellationToken)
        {
            var flowInstance = await _cmmnWorkflowInstanceQueryRepository.FindFlowInstanceById(@event.AggregateId);
            flowInstance.Handle(@event);
            _cmmnWorkflowInstanceCommandRepository.Update(flowInstance);
            await _cmmnWorkflowInstanceCommandRepository.SaveChanges();
        }

        public async Task Handle(CaseElementPlannedEvent @event, CancellationToken cancellationToken)
        {
            var flowInstance = await _cmmnWorkflowInstanceQueryRepository.FindFlowInstanceById(@event.AggregateId);
            flowInstance.Handle(@event);
            _cmmnWorkflowInstanceCommandRepository.Update(flowInstance);
            await _cmmnWorkflowInstanceCommandRepository.SaveChanges();
        }
    }
}
