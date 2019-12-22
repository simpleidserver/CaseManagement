using CaseManagement.CMMN.CaseInstance.Processors;
using CaseManagement.CMMN.CaseInstance.Processors.Listeners;
using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Domains.Events;
using System;
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
            var cancellationTokenSource = new CancellationTokenSource();
            var task = new Task(() => HandleTask(workflowDefinition, workflowInstance, cancellationTokenSource));
            task.Start();
            return task;
        }

        private void HandleTask(CMMNWorkflowDefinition workflowDefinition, CMMNWorkflowInstance workflowInstance, CancellationTokenSource cancellationTokenSource)
        {
            var createListener = new CreateListener(workflowDefinition, workflowInstance, _cmmnPlanItemProcessors, cancellationTokenSource.Token);
            createListener.Listen();
            var repetitionListener = new RepetitionListener(workflowDefinition, workflowInstance);
            repetitionListener.Listen();
            foreach (var element in workflowDefinition.Elements)
            {
                workflowInstance.CreateWorkflowElementInstance(element);
            }

            var children = workflowDefinition.Elements.Select(e => e.Id);
            bool continueExecution = true;
            bool isSuspend = false;
            var reactivateListener = CMMNCaseTransitionListener.Listen(workflowInstance, CMMNTransitions.Reactivate, () =>
            {
                if (isSuspend)
                {
                    isSuspend = false;
                    var workflowElementInstances = workflowInstance.WorkflowElementInstances;
                    foreach (var workflowElementInstance in workflowElementInstances)
                    {
                        workflowInstance.MakeTransition(workflowElementInstance.Id, CMMNTransitions.ParentResume);
                    }
                }
            });
            var suspendListener = CMMNCaseTransitionListener.Listen(workflowInstance, CMMNTransitions.Suspend, () =>
            {
                var workflowElementInstances = workflowInstance.WorkflowElementInstances;
                foreach (var workflowElementInstance in workflowElementInstances)
                {
                    workflowInstance.MakeTransition(workflowElementInstance.Id, CMMNTransitions.ParentSuspend);
                }

                isSuspend = true;
            });
            var terminateListener = CMMNCaseTransitionListener.Listen(workflowInstance, CMMNTransitions.Terminate, () =>
            {
                var workflowElementInstances = workflowInstance.WorkflowElementInstances;
                foreach (var workflowElementInstance in workflowElementInstances)
                {
                    workflowInstance.MakeTransition(workflowElementInstance.Id, CMMNTransitions.ParentTerminate);
                }

                continueExecution = false;
            });
            while (continueExecution)
            {
                Thread.Sleep(100);
                if (isSuspend)
                {
                    continue;
                }

                try
                {
                    cancellationTokenSource.Token.ThrowIfCancellationRequested();
                    if (children.All(c => workflowInstance.IsWorkflowElementDefinitionFinished(c)))
                    {
                        continueExecution = false;
                        workflowInstance.MakeTransition(CMMNTransitions.Complete);
                    }
                }
                catch (OperationCanceledException)
                {
                    continueExecution = false;
                }
            }

            reactivateListener.Unsubscribe();
            suspendListener.Unsubscribe();
            terminateListener.Unsubscribe();
            createListener.Unsubscribe();
            repetitionListener.Unsubscribe();
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

            public void Unsubscribe()
            {
                _workflowInstance.EventRaised -= HandleEventRaised;
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
                var workflowElementInstance = parameter.WorkflowInstance.GetLastWorkflowElementInstance(elementCreated.WorkflowElementDefinitionId);
                if (workflowElementInstance == null || workflowElementInstance.Version == 0)
                {
                    _workflowInstance.StartElement(elementCreated.WorkflowElementDefinitionId);
                }

                processor.Handle(parameter, _token).ContinueWith((obj) =>
                {
                    var result = obj.Result;
                    if (result.WorkflowInstance.IsRepetitionRuleSatisfied(result.WorkflowElementInstance.WorkflowElementDefinitionId, result.WorkflowDefinition, false))
                    {
                        result.WorkflowInstance.CreateWorkflowElementInstance(result.WorkflowElementInstance.WorkflowElementDefinitionId, result.WorkflowElementInstance.WorkflowElementDefinitionType);
                        return;
                    }

                    result.WorkflowInstance.FinishElement(result.WorkflowElementInstance.WorkflowElementDefinitionId);
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

            public void Unsubscribe()
            {
                _workflowInstance.EventRaised -= HandleEventRaised;
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
                        else
                        {
                            _workflowInstance.FinishElement(planItem.Id);
                        }
                    }
                }
            }
        }
    }
}
