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
            // var workflowInstance = CMMNWorkflowInstance.New(workflowDefinition);
            foreach (var planItem in workflowDefinition.PlanItems)
            {
                // var repetitionListener = new RepetitionListener(workflowInstance);
                // repetitionListener.Listen();
                var createListener = new CreateListener(workflowDefinition, workflowInstance, _cmmnPlanItemProcessors, cancellationToken);
                createListener.Listen();
                workflowInstance.CreatePlanItemInstance(planItem);
            }

            return Task.CompletedTask;
        }

        private class CreateListener
        {
            private CMMNWorkflowDefinition _workflowDefinition;
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
                var elementCreated = args.DomainEvt as CMMNPlanItemInstanceCreatedEvent;
                if (elementCreated == null)
                {
                    return;
                }

                var processor = _processors.First(p => p.Type == elementCreated.PlanItemDefinitionType);
                processor.Handle(new PlanItemProcessorParameter(_workflowDefinition, _workflowInstance, _workflowInstance.PlanItemInstances.Last()), _token).ContinueWith((obj) =>
                {

                });
                /*
                var processor = _processFlowElementProcessorFactory.Build(cmmnPlanItem);
                var context = new CMMNWorkflowInstanceContext(_processFlowIntrance, _planItem, _planItem.Version);
                processor.Handle(context, _token).ContinueWith((obj) =>
                {
                    if (_processFlowIntrance.IsRepetitionRuleValid(_planItem))
                    {
                        _processFlowIntrance.CreatePlanItem(_planItem);
                        return;
                    }

                    // _processFlowIntrance.CompleteElement(_planItem);
                    _planItem.StateChanged -= HandleStateChanged;
                });
                */
            }
        }

        private class RepetitionListener
        {
            private Dictionary<string, string> _mappingPlanItemEvent;
            private readonly CMMNWorkflowInstance _workflowInstance;
            private readonly CMMNPlanItemInstance _planItemInstance;

            public RepetitionListener(CMMNWorkflowInstance workflowInstance)
            {
                _workflowInstance = workflowInstance;
                // _planItemInstance = planItemInstance;
            }

            public bool Listen()
            {
                _mappingPlanItemEvent = new Dictionary<string, string>();
                /*
                if (_planItemInstance.PlanItemDefinition.ActivationRule == CMMNActivationRuleTypes.Repetition && _planItemInstance.PlanItemDefinition.EntryCriterions.Any())
                {
                    foreach (var entryCriterion in _planItemInstance.PlanItemDefinition.EntryCriterions)
                    {
                        var planItemOnParts = entryCriterion.SEntry.PlanItemOnParts.Where(p => !string.IsNullOrWhiteSpace(p.SourceRef));
                        foreach (var planItemOnPart in planItemOnParts)
                        {
                            var elt = _workflowInstance.GetPlanItemInstance(planItemOnPart.SourceRef);
                            _mappingPlanItemEvent.Add(planItemOnPart.SourceRef, Enum.GetName(typeof(CMMNPlanItemTransitions), planItemOnPart.StandardEvent));
                            elt.TransitionApplied += HandleTransitionApplied;
                        }
                    }

                    return true;
                }

                return false;
                */
                return true;
            }

            private void HandleTransitionApplied(object sender, string e)
            {
                var elt = sender as CMMNPlanItemInstance;
                if (elt.Version == 1 || elt.Version != _planItemInstance.Version)
                {
                    return;
                }

                var standarEvent = _mappingPlanItemEvent[elt.Id];
                if (e == standarEvent)
                {
                    /*
                    if (_processFlowIntrance.IsEntryCriterionValid(_planItem))
                    {
                        _processFlowIntrance.CreatePlanItem(_planItem);
                    }
                    */
                }
            }
        }
    }
}
