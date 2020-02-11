using CaseManagement.CMMN.CasePlanInstance.Exceptions;
using CaseManagement.CMMN.CasePlanInstance.Processors.Listeners;
using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Infrastructures;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static CaseManagement.CMMN.CasePlanInstance.Processors.Listeners.CriteriaListener;

namespace CaseManagement.CMMN.CasePlanInstance.Processors
{
    public class CMMNStageProcessor : IProcessor
    {
        private readonly ILogger _logger;
        private readonly ICommitAggregateHelper _commitAggregateHelper;
        public CaseElementTypes Type => CaseElementTypes.Stage;

        public CMMNStageProcessor(ILogger<CMMNStageProcessor> logger, ICommitAggregateHelper commitAggregateHelper)
        {
            _logger = logger;
            _commitAggregateHelper = commitAggregateHelper;
        }

        public Task<ProcessorParameter> Handle(ProcessorParameter parameter, CancellationToken token)
        {
            var cancellationTokenSource = new CancellationTokenSource();
            var task = new Task<ProcessorParameter>(() => HandleTask(parameter, cancellationTokenSource), TaskCreationOptions.LongRunning);
            task.Start();
            return task;
        }

        private ProcessorParameter HandleTask(ProcessorParameter parameter, CancellationTokenSource tokenSource)
        {
            KeyValuePair<Task, CriterionListener>? kvp = null;
            CasePlanElementInstanceTransitionListener.EventListener parentTerminateEvtListener = null;
            CasePlanElementInstanceTransitionListener.EventListener parentSuspendEvtListener = null;
            CasePlanElementInstanceTransitionListener.EventListener parentResumeEvtListener = null;
            CasePlanElementInstanceTransitionListener.EventListener reactivateEvtListener = null;
            CasePlanElementInstanceTransitionListener.EventListener suspendEvtListener = null;
            CasePlanElementInstanceTransitionListener.EventListener resumeEvtListener = null;
            CasePlanElementInstanceTransitionListener.EventListener terminateEvtListener = null;
            try
            {
                bool continueExecution = true;
                parentTerminateEvtListener = CasePlanElementInstanceTransitionListener.Listen(parameter, CMMNTransitions.ParentTerminate, () =>
                {
                    tokenSource.Cancel();
                    var workflowElementInstances = parameter.CaseInstance.GetWorkflowElementInstancesByParentId(parameter.CaseElementInstance.Id);
                    foreach (var workflowElementInstance in workflowElementInstances)
                    {
                        parameter.CaseInstance.MakeTransitionParentTerminate(workflowElementInstance.Id);
                    }

                    continueExecution = false;
                });
                CriteriaListener.ListenEntryCriterias(parameter, tokenSource.Token);
                var planItemDefinition = parameter.CaseDefinition.GetElement(parameter.CaseElementInstance.CaseElementDefinitionId);
                if (parameter.CaseInstance.IsManualActivationRuleSatisfied(parameter.CaseElementInstance.Id, parameter.CaseDefinition))
                {
                    var caseworkertask = CaseWorkerTaskAggregate.New(string.Empty, parameter.CaseInstance.Id, parameter.CaseElementInstance.Id, CaseWorkerTaskTypes.ActivateCasePlanElement);
                    _commitAggregateHelper.Commit(caseworkertask, CaseWorkerTaskAggregate.GetStreamName(caseworkertask.Id), CMMNConstants.QueueNames.CaseWorkerTasks).Wait();
                }

                var isManuallyActivated = ManualActivationListener.Listen(parameter, tokenSource.Token);
                if (!isManuallyActivated)
                {
                    parameter.CaseInstance.MakeTransitionStart(parameter.CaseElementInstance.Id);
                }

                var cmmnStageDefinition = (parameter.CaseInstance.GetWorkflowElementDefinition(parameter.CaseElementInstance.Id, parameter.CaseDefinition) as PlanItemDefinition).Stage;
                foreach (var elt in cmmnStageDefinition.Elements.Where(e => e.TableItem == null))
                {
                    parameter.CaseInstance.CreateWorkflowElementInstance(elt, parameter.CaseElementInstance.Id);
                }

                var children = cmmnStageDefinition.Elements.Select(e => e.Id);
                bool isSuspend = false;
                parentSuspendEvtListener = CasePlanElementInstanceTransitionListener.Listen(parameter, CMMNTransitions.ParentSuspend, () =>
                {
                    var workflowElementInstances = parameter.CaseInstance.GetWorkflowElementInstancesByParentId(parameter.CaseElementInstance.Id);
                    foreach (var workflowElementInstance in workflowElementInstances)
                    {
                        parameter.CaseInstance.MakeTransitionParentSuspend(workflowElementInstance.Id);
                    }

                    isSuspend = true;
                });
                parentResumeEvtListener = CasePlanElementInstanceTransitionListener.Listen(parameter, CMMNTransitions.ParentResume, () =>
                {
                    var workflowElementInstances = parameter.CaseInstance.GetWorkflowElementInstancesByParentId(parameter.CaseElementInstance.Id);
                    foreach (var workflowElementInstance in workflowElementInstances)
                    {
                        parameter.CaseInstance.MakeTransitionParentResume(workflowElementInstance.Id);
                    }

                    isSuspend = false;
                });
                reactivateEvtListener = CasePlanElementInstanceTransitionListener.Listen(parameter, CMMNTransitions.Reactivate, () =>
                {
                    var workflowElementInstances = parameter.CaseInstance.GetWorkflowElementInstancesByParentId(parameter.CaseElementInstance.Id);
                    foreach (var workflowElementInstance in workflowElementInstances)
                    {
                        parameter.CaseInstance.MakeTransitionReactivate(workflowElementInstance.Id);
                    }

                    isSuspend = false;
                });
                suspendEvtListener = CasePlanElementInstanceTransitionListener.Listen(parameter, CMMNTransitions.Suspend, () =>
                {
                    var workflowElementInstances = parameter.CaseInstance.GetWorkflowElementInstancesByParentId(parameter.CaseElementInstance.Id);
                    foreach (var workflowElementInstance in workflowElementInstances)
                    {
                        parameter.CaseInstance.MakeTransitionParentSuspend(workflowElementInstance.Id);
                    }

                    isSuspend = true;
                });
                resumeEvtListener = CasePlanElementInstanceTransitionListener.Listen(parameter, CMMNTransitions.Resume, () =>
                {
                    var workflowElementInstances = parameter.CaseInstance.GetWorkflowElementInstancesByParentId(parameter.CaseElementInstance.Id);
                    foreach (var workflowElementInstance in workflowElementInstances)
                    {
                        parameter.CaseInstance.MakeTransitionParentResume(workflowElementInstance.Id);
                    }

                    isSuspend = false;
                });
                terminateEvtListener = CasePlanElementInstanceTransitionListener.Listen(parameter, CMMNTransitions.Terminate, () =>
                {
                    tokenSource.Cancel();
                    var workflowElementInstances = parameter.CaseInstance.GetWorkflowElementInstancesByParentId(parameter.CaseElementInstance.Id);
                    foreach (var workflowElementInstance in workflowElementInstances)
                    {
                        parameter.CaseInstance.MakeTransitionParentTerminate(workflowElementInstance.Id);
                    }

                    continueExecution = false;
                });
                try
                {
                    kvp = CriteriaListener.ListenExitCriterias(parameter, tokenSource.Token);
                    if (kvp != null)
                    {
                        kvp.Value.Key.ContinueWith((r) =>
                        {
                            r.Wait();
                            parameter.CaseInstance.MakeTransitionTerminate(parameter.CaseElementInstance.Id);
                        });
                    }
                }
                catch (TerminateCaseInstanceElementException)
                {
                    parameter.CaseInstance.MakeTransitionTerminate(parameter.CaseElementInstance.Id);
                }

                while (continueExecution)
                {
                    Thread.Sleep(100);
                    if (isSuspend)
                    {
                        continue;
                    }

                    try
                    {
                        tokenSource.Token.ThrowIfCancellationRequested();
                        if (children.Any(c => parameter.CaseInstance.IsWorkflowElementDefinitionFailed(c) && parameter.CaseDefinition.GetElement(c).Type != CaseElementTypes.Stage))
                        {
                            isSuspend = true;
                            parameter.CaseInstance.MakeTransitionFault(parameter.CaseElementInstance.Id);
                        }
                        else if (children.All(c => parameter.CaseInstance.IsWorkflowElementDefinitionFinished(c)))
                        {
                            continueExecution = false;
                            parameter.CaseInstance.MakeTransitionComplete(parameter.CaseElementInstance.Id);
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

            }
            finally
            {
                if (parentTerminateEvtListener != null)
                {
                    parentTerminateEvtListener.Unsubscribe();
                }

                if (parentSuspendEvtListener != null)
                {
                    parentSuspendEvtListener.Unsubscribe();
                }

                if (parentResumeEvtListener != null)
                {
                    parentResumeEvtListener.Unsubscribe();
                }

                if (reactivateEvtListener != null)
                {
                    reactivateEvtListener.Unsubscribe();
                }

                if (parentResumeEvtListener != null)
                {
                    parentResumeEvtListener.Unsubscribe();
                }

                if (suspendEvtListener != null)
                {
                    suspendEvtListener.Unsubscribe();
                }

                if (resumeEvtListener != null)
                {
                    resumeEvtListener.Unsubscribe();
                }

                if (terminateEvtListener != null)
                {
                    terminateEvtListener.Unsubscribe();
                }

                if (kvp != null)
                {
                    if (kvp.Value.Key.IsCanceled || kvp.Value.Key.IsCompleted || kvp.Value.Key.IsFaulted)
                    {
                        kvp.Value.Key.Dispose();
                    }

                    kvp.Value.Value.Unsubscribe();
                }
            }

            return parameter;
        }
    }
}
