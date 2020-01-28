using CaseManagement.CMMN.CaseInstance.Exceptions;
using CaseManagement.CMMN.CaseInstance.Processors;
using CaseManagement.CMMN.CaseInstance.Processors.Listeners;
using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Domains.Events;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static CaseManagement.CMMN.CaseInstance.Processors.Listeners.CMMNCriterionListener;

namespace CaseManagement.CMMN.Infrastructures
{
    public class CaseEngine : ICaseEngine
    {
        private readonly ILogger _logger;
        private readonly IEnumerable<IProcessor> _cmmnPlanItemProcessors;

        public CaseEngine(ILogger<CaseEngine> logger, IEnumerable<IProcessor> cmmnPlanItemProcessors)
        {
            _logger = logger;
            _cmmnPlanItemProcessors = cmmnPlanItemProcessors;
        }

        public Task Start(CaseDefinition workflowDefinition, Domains.CaseInstance workflowInstance, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var cancellationTokenSource = new CancellationTokenSource();
            var task = new Task(() => HandleTask(workflowDefinition, workflowInstance, cancellationTokenSource));
            task.Start();
            return task;
        }

        public Task Reactivate(CaseDefinition workflowDefinition, Domains.CaseInstance workflowInstance, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var cancellationTokenSource = new CancellationTokenSource();
            var task = new Task(() => HandleTask(workflowDefinition, workflowInstance, cancellationTokenSource, true));
            task.Start();
            return task;
        }

        private void HandleTask(CaseDefinition workflowDefinition, Domains.CaseInstance workflowInstance, CancellationTokenSource cancellationTokenSource, bool reactivate = false)
        {
            bool continueExecution = true;
            bool isSuspend = false;
            var evtHandler = new EventHandler<DomainEventArgs>((sender, e) =>
            {
                var evt = e.DomainEvt as CaseElementPlanificationConfirmedEvent;
                if (evt == null)
                {
                    return;
                }

                var elt = workflowDefinition.GetElement(evt.CaseElementDefinitionId);
                if (elt == null)
                {
                    return;
                }

                var thread = new Thread(() =>
                {
                    while (isSuspend)
                    {
                        Thread.Sleep(CMMNConstants.WAIT_INTERVAL_MS);
                        if (!continueExecution)
                        {
                            return;
                        }
                    }

                    workflowInstance.CreateWorkflowElementInstance(elt);
                });
                thread.Start();
            });
            workflowInstance.EventRaised += evtHandler;
            var suspendListener = CMMNCaseTransitionListener.Listen(workflowInstance, CMMNTransitions.Suspend, () =>
            {
                lock (workflowInstance.WorkflowElementInstances)
                {
                    var workflowElementInstances = workflowInstance.WorkflowElementInstances;
                    foreach (var workflowElementInstance in workflowElementInstances)
                    {
                        workflowInstance.MakeTransitionParentSuspend(workflowElementInstance.Id);
                    }

                    isSuspend = true;
                }
            });
            var resumeListener = CMMNCaseTransitionListener.Listen(workflowInstance, CMMNTransitions.Resume, () =>
            {
                lock(workflowInstance.WorkflowElementInstances)
                {
                    var workflowElementInstances = workflowInstance.WorkflowElementInstances;
                    foreach (var workflowElementInstance in workflowElementInstances)
                    {
                        workflowInstance.MakeTransitionParentResume(workflowElementInstance.Id);
                    }

                    isSuspend = false;
                }
            });
            var terminateListener = CMMNCaseTransitionListener.Listen(workflowInstance, CMMNTransitions.Terminate, () =>
            {
                lock(workflowInstance.WorkflowElementInstances)
                {
                    var workflowElementInstances = workflowInstance.WorkflowElementInstances;
                    foreach (var workflowElementInstance in workflowElementInstances)
                    {
                        workflowInstance.MakeTransitionParentTerminate(workflowElementInstance.Id);
                    }

                    continueExecution = false;
                }
            });
            var closeListener = CMMNCaseTransitionListener.Listen(workflowInstance, CMMNTransitions.Close, () =>
            {
                lock (workflowInstance.WorkflowElementInstances)
                {
                    var workflowElementInstances = workflowInstance.WorkflowElementInstances;
                    foreach (var workflowElementInstance in workflowElementInstances)
                    {
                        workflowInstance.MakeTransitionParentTerminate(workflowElementInstance.Id);
                    }

                    continueExecution = false;
                }
            });
            KeyValuePair<Task, CriterionListener>? kvp = null;
            try
            {
                kvp = CMMNCriterionListener.ListenExitCriterias(new ProcessorParameter(null, workflowInstance, new CaseElementInstance(null, DateTime.UtcNow, null, CaseElementTypes.Stage, 0, null)), workflowDefinition.ExitCriterias.ToList(), cancellationTokenSource.Token);
                if (kvp != null)
                {
                    kvp.Value.Key.ContinueWith((r) =>
                    {
                        r.Wait();
                        workflowInstance.MakeTransition(CMMNTransitions.Terminate);
                    });
                }
            }
            catch (TerminateCaseInstanceElementException)
            {
                workflowInstance.MakeTransition(CMMNTransitions.Terminate);
            }

            var createListener = new CreateListener(workflowDefinition, workflowInstance, _cmmnPlanItemProcessors, cancellationTokenSource.Token);
            createListener.Listen();
            var repetitionListener = new RepetitionListener(workflowDefinition, workflowInstance);
            repetitionListener.Listen();
            if (reactivate)
            {
                lock (workflowInstance.WorkflowElementInstances)
                {
                    workflowInstance.MakeTransition(CMMNTransitions.Reactivate);
                    foreach (var elt in workflowInstance.WorkflowElementInstances.Where(w => w.IsActive()))
                    {
                        var parameter = new ProcessorParameter(workflowDefinition, workflowInstance, workflowInstance.GetWorkflowElementInstance(elt.Id));
                        var processor = _cmmnPlanItemProcessors.First(p => p.Type == elt.CaseElementDefinitionType);
                        processor.Handle(parameter, cancellationTokenSource.Token).ContinueWith((obj) =>
                        {
                            var result = obj.Result;
                            if (result.CaseInstance.IsRepetitionRuleSatisfied(result.CaseElementInstance.CaseElementDefinitionId, result.CaseDefinition, false))
                            {
                                result.CaseInstance.CreateWorkflowElementInstance(result.CaseElementInstance.CaseElementDefinitionId, result.CaseElementInstance.CaseElementDefinitionType);
                                return;
                            }

                            if (result.CaseElementInstance.IsComplete())
                            {
                                result.CaseInstance.FinishElement(result.CaseElementInstance.CaseElementDefinitionId);
                            }
                        });
                    }
                }
            }
            else
            {
                foreach (var element in workflowDefinition.Elements.Where(e => !e.IsDiscrete()))
                {
                    workflowInstance.CreateWorkflowElementInstance(element);
                }
            }

            foreach(var element in workflowDefinition.Elements.Where(e => e.IsDiscrete()))
            {
                if (workflowInstance.IsPlanned(element.Id))
                {
                    if (workflowInstance.GetLastWorkflowElementInstance(element.Id) == null)
                    {
                        workflowInstance.CreateWorkflowElementInstance(element);
                    }
                }
                else if (!workflowInstance.IsInPlanning(element.Id))
                {
                    workflowInstance.CreateTableItem(element.Id, element.TableItem.AuthorizedRoleRef);
                }
            }

            var children = workflowDefinition.Elements.Where(c => !c.IsDiscrete()).Select(e => e.Id).ToList();
            foreach(var child in workflowDefinition.Elements.Where(c => c.IsDiscrete()).Select(c => c.Id))
            {
                if (workflowInstance.IsPlanned(child))
                {
                    children.Add(child);
                }
            }

            while (continueExecution)
            {
                Thread.Sleep(CMMNConstants.WAIT_INTERVAL_MS);
                if (isSuspend)
                {
                    continue;
                }

                try
                {
                    cancellationTokenSource.Token.ThrowIfCancellationRequested();
                    if (children.Any(c => workflowInstance.IsWorkflowElementDefinitionFailed(c) && workflowDefinition.GetElement(c).Type != CaseElementTypes.Stage))
                    {
                        continueExecution = false;
                        workflowInstance.MakeTransition(CMMNTransitions.Fault);
                    }
                    else if (children.All(c => workflowInstance.IsWorkflowElementDefinitionFinished(c)))
                    {
                        continueExecution = false;
                        workflowInstance.MakeTransition(CMMNTransitions.Complete);
                    }
                }
                catch (OperationCanceledException)
                {
                    continueExecution = false;
                }
                catch(Exception ex)
                {
                    _logger.LogError(ex.ToString());
                    continueExecution = false;
                }
            }

            workflowInstance.EventRaised -= evtHandler;
            closeListener.Unsubscribe();
            resumeListener.Unsubscribe();
            suspendListener.Unsubscribe();
            terminateListener.Unsubscribe();
            createListener.Unsubscribe();
            repetitionListener.Unsubscribe();
            cancellationTokenSource.Cancel();
            if (kvp != null)
            {
                if (kvp.Value.Key.IsCanceled || kvp.Value.Key.IsCompleted || kvp.Value.Key.IsFaulted)
                {
                    kvp.Value.Key.Dispose();
                }

                kvp.Value.Value.Unsubscribe();
            }
        }

        private class CreateListener
        {
            private readonly CaseDefinition _workflowDefinition;
            private readonly Domains.CaseInstance _workflowInstance;
            private readonly IEnumerable<IProcessor> _processors;
            private readonly CancellationToken _token;

            public CreateListener(CaseDefinition workflowDefinition, Domains.CaseInstance workflowInstance, IEnumerable<IProcessor> processors, CancellationToken token)
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
                var elementCreated = args.DomainEvt as CaseElementCreatedEvent;
                if (elementCreated == null)
                {
                    return;
                }

                var processor = _processors.First(p => p.Type == elementCreated.CaseElementDefinitionType);
                var parameter = new ProcessorParameter(_workflowDefinition, _workflowInstance, _workflowInstance.GetWorkflowElementInstance(elementCreated.CaseElementId));
                var workflowElementInstance = parameter.CaseInstance.GetLastWorkflowElementInstance(elementCreated.CaseElementDefinitionId);
                if (workflowElementInstance == null || workflowElementInstance.Version == 0)
                {
                    _workflowInstance.StartElement(elementCreated.CaseElementDefinitionId);
                }

                // Note : Ignore TimerEventListener and CaseFileItem.
                if (workflowElementInstance.Version > 0 && (workflowElementInstance.CaseElementDefinitionType == CaseElementTypes.TimerEventListener || workflowElementInstance.CaseElementDefinitionType == CaseElementTypes.CaseFileItem))
                {
                    return;
                }

                processor.Handle(parameter, _token).ContinueWith((obj) =>
                {
                    var result = obj.Result;
                    if (result.CaseInstance.IsRepetitionRuleSatisfied(result.CaseElementInstance.CaseElementDefinitionId, result.CaseDefinition, false))
                    {
                        result.CaseInstance.CreateWorkflowElementInstance(result.CaseElementInstance.CaseElementDefinitionId, result.CaseElementInstance.CaseElementDefinitionType);
                        return;
                    }
                    
                    if (result.CaseElementInstance.IsComplete())
                    {
                        result.CaseInstance.FinishElement(result.CaseElementInstance.CaseElementDefinitionId);
                    }
                });
            }
        }

        private class RepetitionListener
        {
            private readonly CaseDefinition _workflowDefinition;
            private readonly Domains.CaseInstance _workflowInstance;

            public RepetitionListener(CaseDefinition workflowDefinition, Domains.CaseInstance workflowInstance)
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
                var raisedEvt = e.DomainEvt as CaseElementTransitionRaisedEvent;
                if (raisedEvt == null)
                {
                    return;
                }

                var sourcePlanItemInstance = _workflowInstance.GetWorkflowElementInstance(raisedEvt.CaseElementId);
                foreach (var planItem in _workflowDefinition.Elements)
                {
                    if(planItem.EntryCriterions.Any(ec => ec.SEntry.PlanItemOnParts.Any(pi => pi.StandardEvent == raisedEvt.Transition && pi.SourceRef == sourcePlanItemInstance.CaseElementDefinitionId)) && planItem.ActivationRule == ActivationRuleTypes.Repetition)
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
