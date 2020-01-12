using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Domains.Events;
using CaseManagement.CMMN.Persistence;
using CaseManagement.Workflow.Infrastructure;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CaseInstance.EventHandlers
{
    public class WorkflowInstanceHandler : IDomainEventHandler<CMMNWorkflowInstanceCreatedEvent>, IDomainEventHandler<CMMNWorkflowElementCreatedEvent>, IDomainEventHandler<CMMNWorkflowElementFinishedEvent>,
        IDomainEventHandler<CMMNWorkflowElementInstanceFormCreatedEvent>, IDomainEventHandler<CMMNWorkflowElementInstanceFormSubmittedEvent>, IDomainEventHandler<CMMNWorkflowElementStartedEvent>,
        IDomainEventHandler<CMMNWorkflowElementTransitionRaisedEvent>, IDomainEventHandler<CMMNWorkflowTransitionRaisedEvent>, IDomainEventHandler<CMMNWorkflowInstanceVariableAddedEvent>
    {
        private readonly ICMMNWorkflowInstanceCommandRepository _cmmnWorkflowInstanceCommandRepository;
        private readonly ICMMNWorkflowInstanceQueryRepository _cmmnWorkflowInstanceQueryRepository;

        public WorkflowInstanceHandler(ICMMNWorkflowInstanceCommandRepository cmmnWorkflowInstanceCommandRepository, ICMMNWorkflowInstanceQueryRepository cmmnWorkflowInstanceQueryRepository)
        {
            _cmmnWorkflowInstanceCommandRepository = cmmnWorkflowInstanceCommandRepository;
            _cmmnWorkflowInstanceQueryRepository = cmmnWorkflowInstanceQueryRepository;
        }

        public async Task Handle(CMMNWorkflowInstanceCreatedEvent @event, CancellationToken cancellationToken)
        {
            var processFlowInstance = CMMNWorkflowInstance.New(new List<DomainEvent>
            {
                @event
            });
            _cmmnWorkflowInstanceCommandRepository.Add(processFlowInstance);
            await _cmmnWorkflowInstanceCommandRepository.SaveChanges();
        }

        public async Task Handle(CMMNWorkflowElementCreatedEvent @event, CancellationToken cancellationToken)
        {
            var flowInstance = await _cmmnWorkflowInstanceQueryRepository.FindFlowInstanceById(@event.AggregateId);
            flowInstance.Handle(@event);
            _cmmnWorkflowInstanceCommandRepository.Update(flowInstance);
            await _cmmnWorkflowInstanceCommandRepository.SaveChanges();
        }

        public async Task Handle(CMMNWorkflowElementFinishedEvent @event, CancellationToken cancellationToken)
        {
            var flowInstance = await _cmmnWorkflowInstanceQueryRepository.FindFlowInstanceById(@event.AggregateId);
            flowInstance.Handle(@event);
            _cmmnWorkflowInstanceCommandRepository.Update(flowInstance);
            await _cmmnWorkflowInstanceCommandRepository.SaveChanges();
        }

        public async Task Handle(CMMNWorkflowElementInstanceFormCreatedEvent @event, CancellationToken cancellationToken)
        {
            var flowInstance = await _cmmnWorkflowInstanceQueryRepository.FindFlowInstanceById(@event.AggregateId);
            flowInstance.Handle(@event);
            _cmmnWorkflowInstanceCommandRepository.Update(flowInstance);
            await _cmmnWorkflowInstanceCommandRepository.SaveChanges();
        }

        public async Task Handle(CMMNWorkflowElementInstanceFormSubmittedEvent @event, CancellationToken cancellationToken)
        {
            var flowInstance = await _cmmnWorkflowInstanceQueryRepository.FindFlowInstanceById(@event.AggregateId);
            flowInstance.Handle(@event);
            _cmmnWorkflowInstanceCommandRepository.Update(flowInstance);
            await _cmmnWorkflowInstanceCommandRepository.SaveChanges();
        }

        public async Task Handle(CMMNWorkflowElementStartedEvent @event, CancellationToken cancellationToken)
        {
            var flowInstance = await _cmmnWorkflowInstanceQueryRepository.FindFlowInstanceById(@event.AggregateId);
            flowInstance.Handle(@event);
            _cmmnWorkflowInstanceCommandRepository.Update(flowInstance);
            await _cmmnWorkflowInstanceCommandRepository.SaveChanges();
        }

        public async Task Handle(CMMNWorkflowElementTransitionRaisedEvent @event, CancellationToken cancellationToken)
        {
            var flowInstance = await _cmmnWorkflowInstanceQueryRepository.FindFlowInstanceById(@event.AggregateId);
            flowInstance.Handle(@event);
            _cmmnWorkflowInstanceCommandRepository.Update(flowInstance);
            await _cmmnWorkflowInstanceCommandRepository.SaveChanges();
        }

        public async Task Handle(CMMNWorkflowTransitionRaisedEvent @event, CancellationToken cancellationToken)
        {
            var flowInstance = await _cmmnWorkflowInstanceQueryRepository.FindFlowInstanceById(@event.AggregateId);
            flowInstance.Handle(@event);
            _cmmnWorkflowInstanceCommandRepository.Update(flowInstance);
            await _cmmnWorkflowInstanceCommandRepository.SaveChanges();
        }

        public async Task Handle(CMMNWorkflowInstanceVariableAddedEvent @event, CancellationToken cancellationToken)
        {
            var flowInstance = await _cmmnWorkflowInstanceQueryRepository.FindFlowInstanceById(@event.AggregateId);
            flowInstance.Handle(@event);
            _cmmnWorkflowInstanceCommandRepository.Update(flowInstance);
            await _cmmnWorkflowInstanceCommandRepository.SaveChanges();
        }
    }
}
