using CaseManagement.CMMN.Domains.Events;
using CaseManagement.CMMN.Persistence;
using CaseManagement.Workflow.Infrastructure;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CaseInstance.EventHandlers
{
    public class WorkflowInstanceHandler : IDomainEventHandler<CaseInstanceCreatedEvent>, IDomainEventHandler<CaseElementCreatedEvent>, IDomainEventHandler<CaseElementFinishedEvent>,
        IDomainEventHandler<CaseElementInstanceFormCreatedEvent>, IDomainEventHandler<CaseElementInstanceFormSubmittedEvent>, IDomainEventHandler<CaseElementStartedEvent>,
        IDomainEventHandler<CaseElementTransitionRaisedEvent>, IDomainEventHandler<CaseTransitionRaisedEvent>, IDomainEventHandler<CaseInstanceVariableAddedEvent>
    {
        private readonly IWorkflowInstanceCommandRepository _cmmnWorkflowInstanceCommandRepository;
        private readonly IWorkflowInstanceQueryRepository _cmmnWorkflowInstanceQueryRepository;

        public WorkflowInstanceHandler(IWorkflowInstanceCommandRepository cmmnWorkflowInstanceCommandRepository, IWorkflowInstanceQueryRepository cmmnWorkflowInstanceQueryRepository)
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
    }
}
