using System;
using System.Threading;
using System.Threading.Tasks;
using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Domains.Events;
using CaseManagement.CMMN.Persistence;
using CaseManagement.Workflow.Infrastructure;

namespace CaseManagement.CMMN.CaseInstance.EventHandlers
{
    public class CaseActivationHandler : IDomainEventHandler<CaseElementTransitionRaisedEvent>
    {
        private readonly IActivationCommandRepository _cmmnActivationCommandRepository;
        private readonly IActivationQueryRepository _cmmnActivationQueryRepository;
        private readonly ICaseInstanceQueryRepository _cmmnWorkflowInstanceQueryRepository;
        private readonly ICaseDefinitionQueryRepository _cmmnWorkflowDefinitionQueryRepository;

        public CaseActivationHandler(ICaseInstanceQueryRepository cmmnWorkflowInstanceQueryRepository, IActivationQueryRepository cmmnActivationQueryRepository, IActivationCommandRepository cmmnActivationCommandRepository, ICaseDefinitionQueryRepository cmmnWorkflowDefinitionQueryRepository)
        {
            _cmmnActivationCommandRepository = cmmnActivationCommandRepository;
            _cmmnActivationQueryRepository = cmmnActivationQueryRepository;
            _cmmnWorkflowInstanceQueryRepository = cmmnWorkflowInstanceQueryRepository;
            _cmmnWorkflowDefinitionQueryRepository = cmmnWorkflowDefinitionQueryRepository;
        }

        public async Task Handle(CaseElementTransitionRaisedEvent @event, CancellationToken cancellationToken)
        {
            if (@event.Transition != CMMNTransitions.Enable && @event.Transition != CMMNTransitions.ManualStart)
            {
                return;
            }

            var activation = await _cmmnActivationQueryRepository.FindById(@event.CaseElementId);
            if (@event.Transition == CMMNTransitions.ManualStart && activation != null)
            {
                _cmmnActivationCommandRepository.Delete(activation);
                await _cmmnActivationCommandRepository.SaveChanges();
                return;
            }

            if (activation != null)
            {
                return;
            }

            var workflowInstance = await _cmmnWorkflowInstanceQueryRepository.FindFlowInstanceById(@event.AggregateId);
            var workflowDefinition = await _cmmnWorkflowDefinitionQueryRepository.FindById(workflowInstance.CaseDefinitionId);
            var eltDef = workflowDefinition.GetElement(@event.CaseElementDefinitionId);
            activation = new CaseActivationAggregate
            {
                CreateDateTime = DateTime.UtcNow,
                WorkflowId = workflowDefinition.Id,
                WorkflowElementId = @event.CaseElementDefinitionId,
                WorkflowElementInstanceId = @event.CaseElementId,
                WorkflowElementName = eltDef.Name,
                WorkflowInstanceId = workflowInstance.Id,
                WorkflowInstanceName = workflowDefinition.Name,
                Performer = string.Empty
            };
            _cmmnActivationCommandRepository.Add(activation);
            await _cmmnActivationCommandRepository.SaveChanges();
        }
    }
}