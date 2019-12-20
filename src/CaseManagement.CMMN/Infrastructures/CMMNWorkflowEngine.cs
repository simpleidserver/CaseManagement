using CaseManagement.CMMN.CaseInstance.Processors;
using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Domains.Events;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Infrastructures
{
    public class CMMNWorkflowEngine : ICMMNWorkflowEngine
    {
        private readonly IEnumerable<ICMMNPlanItemProcessor> _cmmnPlanItemProcessors;

        public CMMNWorkflowEngine(IEnumerable<ICMMNPlanItemProcessor> cmmnPlanItemProcessors)
        {
            _cmmnPlanItemProcessors = cmmnPlanItemProcessors;
        }

        public Task Start(CMMNWorkflowDefinition workflowDefinition, CMMNWorkflowInstance workflowInstance, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var createListener = new CreateListener(workflowDefinition, workflowInstance, _cmmnPlanItemProcessors, cancellationToken);
            createListener.Listen();
            var repetitionListener = new RepetitionListener(workflowDefinition, workflowInstance);
            repetitionListener.Listen();
            foreach (var element in workflowDefinition.Elements)
            {
                workflowInstance.CreateWorkflowElementInstance(element);
            }

            return Task.CompletedTask;
        }

        private class CreateListener
        {
            private readonly CMMNWorkflowDefinition _workflowDefinition;
            private readonly CMMNWorkflowInstance _workflowInstance;
            private readonly IEnumerable<ICMMNPlanItemProcessor> _processors;
            private readonly CancellationToken _token;

            public CreateListener(CMMNWorkflowDefinition workflowDefinition, CMMNWorkflowInstance workflowInstance, IEnumerable<ICMMNPlanItemProcessor> processors, CancellationToken token)
            {
                _workflowDefinition = workflowDefinition;
                _workflowInstance = workflowInstance;
                _processors = processors;
                _token = token;
            }

            public void Listen()
            {
                _workflowInstance.EventRaised += HandleEventRaised;
            }

            private void HandleEventRaised(object sender, DomainEventArgs args)
            {
                var elementCreated = args.DomainEvt as CMMNWorkflowElementCreatedEvent;
                if (elementCreated == null)
                {
                    return;
                }

                var processor = _processors.First(p => p.Type == elementCreated.WorkflowElementDefinitionType);
                var parameter = new PlanItemProcessorParameter(_workflowDefinition, _workflowInstance, _workflowInstance.GetWorkflowElementInstance(elementCreated.ElementId));
                processor.Handle(parameter, _token).ContinueWith((obj) =>
                {
                    if (_workflowInstance.IsRepetitionRuleSatisfied(elementCreated.WorkflowElementDefinitionId, _workflowDefinition, false))
                    {
                        _workflowInstance.CreateWorkflowElementInstance(elementCreated.WorkflowElementDefinitionId, elementCreated.WorkflowElementDefinitionType);
                        return;
                    }
                });
            }
        }

        private class RepetitionListener
        {
            private readonly CMMNWorkflowDefinition _workflowDefinition;
            private readonly CMMNWorkflowInstance _workflowInstance;

            public RepetitionListener(CMMNWorkflowDefinition workflowDefinition, CMMNWorkflowInstance workflowInstance)
            {
                _workflowDefinition = workflowDefinition;
                _workflowInstance = workflowInstance;
            }

            public void Listen()
            {
                _workflowInstance.EventRaised += HandleEventRaised;
            }

            private void HandleEventRaised(object sender, DomainEventArgs e)
            {
                var raisedEvt = e.DomainEvt as CMMNWorkflowElementTransitionRaisedEvent;
                if (raisedEvt == null)
                {
                    return;
                }

                var sourcePlanItemInstance = _workflowInstance.GetWorkflowElementInstance(raisedEvt.ElementId);
                foreach (var planItem in _workflowDefinition.Elements)
                {
                    if(planItem.EntryCriterions.Any(ec => ec.SEntry.PlanItemOnParts.Any(pi => pi.StandardEvent == raisedEvt.Transition && pi.SourceRef == sourcePlanItemInstance.WorkflowElementDefinitionId)) && planItem.ActivationRule == CMMNActivationRuleTypes.Repetition)
                    {
                        if (_workflowInstance.IsRepetitionRuleSatisfied(planItem.Id, _workflowDefinition, true))
                        {
                            _workflowInstance.CreateWorkflowElementInstance(planItem.Id, planItem.Type);
                        }
                    }
                }
            }
        }
    }
}
