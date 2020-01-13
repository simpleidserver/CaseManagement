using CaseManagement.CMMN.CaseInstance.Exceptions;
using CaseManagement.CMMN.CaseInstance.Processors.Listeners;
using CaseManagement.CMMN.Domains;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using static CaseManagement.CMMN.CaseInstance.Processors.Listeners.CMMNCriterionListener;

namespace CaseManagement.CMMN.CaseInstance.Processors
{
    public abstract class BaseCMMNTaskProcessor : IProcessor
    {
        public abstract CaseElementTypes Type { get; }

        public Task<ProcessorParameter> Handle(ProcessorParameter parameter, CancellationToken token)
        {
            var task = new Task<ProcessorParameter>(() =>
            {
                var cancellationTokenSource = new CancellationTokenSource();
                return HandleTask(parameter, cancellationTokenSource).Result;
            }, token, TaskCreationOptions.LongRunning);
            task.Start();
            return task;
        }

        protected abstract Task Run(ProcessorParameter parameter, CancellationToken token);
        protected abstract void Unsubscribe();

        private async Task<ProcessorParameter> HandleTask(ProcessorParameter parameter, CancellationTokenSource tokenSource)
        {
            var planItemDefinition = parameter.CaseDefinition.GetElement(parameter.CaseElementInstance.CaseElementDefinitionId);
            CMMNCriterionListener.ListenEntryCriterias(parameter);
            var isManuallyActivated = CMMNManualActivationListener.Listen(parameter);
            if (!isManuallyActivated)
            {
                parameter.CaseInstance.MakeTransitionStart(parameter.CaseElementInstance.Id);
            }

            bool isSuspend = false;
            bool isTerminate = false;
            bool isOperationExecuted = false;
            bool continueExecution = true;
            var parentReactivateEvtListener = CMMNPlanItemTransitionListener.Listen(parameter, CMMNTransitions.Reactivate, () =>
            {
                tokenSource = new CancellationTokenSource();
                isSuspend = false;
                isOperationExecuted = false;
            });
            var parentTerminateEvtListener = CMMNPlanItemTransitionListener.Listen(parameter, CMMNTransitions.ParentTerminate, () =>
            {
                isTerminate = true;
                tokenSource.Cancel();
            });
            var parentSuspendEvtListener = CMMNPlanItemTransitionListener.Listen(parameter, CMMNTransitions.ParentSuspend, () =>
            {
                isSuspend = true;
                tokenSource.Cancel();
            });
            var parentResumeEvtListener = CMMNPlanItemTransitionListener.Listen(parameter, CMMNTransitions.ParentResume, () =>
            {
                tokenSource = new CancellationTokenSource();
                isSuspend = false;
                isOperationExecuted = false;
            });
            var parentExitEvtListener = CMMNPlanItemTransitionListener.Listen(parameter, CMMNTransitions.ParentExit, () =>
            {
                isTerminate = true;
                tokenSource.Cancel();
            });
            var resumeEvtListener = CMMNPlanItemTransitionListener.Listen(parameter, CMMNTransitions.Resume, () =>
            {
                tokenSource = new CancellationTokenSource();
                isSuspend = false;
                isOperationExecuted = false;
            });
            var suspendEvtListener = CMMNPlanItemTransitionListener.Listen(parameter, CMMNTransitions.Suspend, () =>
            {
                isSuspend = true;
                tokenSource.Cancel();
            });
            var terminateEvtListener = CMMNPlanItemTransitionListener.Listen(parameter, CMMNTransitions.Terminate, () =>
            {
                isTerminate = true;
                tokenSource.Cancel();
            });
            var exitEvtListener = CMMNPlanItemTransitionListener.Listen(parameter, CMMNTransitions.Exit, () =>
            {
                isTerminate = true;
                tokenSource.Cancel();
            });
            KeyValuePair<Task, CriterionListener>? kvp = null;
            try
            {
                kvp = CMMNCriterionListener.ListenExitCriterias(parameter);
                if (kvp != null)
                {
                    kvp.Value.Key.ContinueWith((r) =>
                    {
                        r.Wait();
                        parameter.CaseInstance.MakeTransitionExit(parameter.CaseElementInstance.Id);
                    });
                }
            }
            catch (TerminateCaseInstanceElementException)
            {
                parameter.CaseInstance.MakeTransitionExit(parameter.CaseElementInstance.Id);
                return parameter;
            }

            while (continueExecution)
            {
                if (isSuspend)
                {
                    Thread.Sleep(100);
                    continue;
                }

                if (!isOperationExecuted)
                {
                    isOperationExecuted = true;
                    try
                    {
                        await Run(parameter, tokenSource.Token);
                        tokenSource.Token.ThrowIfCancellationRequested();
                        continueExecution = false;
                    }
                    catch (OperationCanceledException)
                    {
                        if (isTerminate)
                        {
                            continueExecution = false;
                        }

                        Unsubscribe();
                    }
                    catch (Exception)
                    {
                        parameter.CaseInstance.MakeTransitionFault(parameter.CaseElementInstance.Id);
                        // NOTE : If the task doesn't belong to a stage then exit the loop.
                        if (string.IsNullOrWhiteSpace(parameter.CaseElementInstance.ParentId))
                        {
                            continueExecution = false;
                        }
                        else
                        {
                            isSuspend = true;
                        }

                        Unsubscribe();
                    }
                }
            }

            parentReactivateEvtListener.Unsubscribe();
            parentTerminateEvtListener.Unsubscribe();
            parentSuspendEvtListener.Unsubscribe();
            parentResumeEvtListener.Unsubscribe();
            parentExitEvtListener.Unsubscribe();
            suspendEvtListener.Unsubscribe();
            terminateEvtListener.Unsubscribe();
            resumeEvtListener.Unsubscribe();
            exitEvtListener.Unsubscribe();
            if (kvp != null)
            {
                if (kvp.Value.Key.IsCanceled || kvp.Value.Key.IsCompleted || kvp.Value.Key.IsFaulted)
                {
                    kvp.Value.Key.Dispose();
                }

                kvp.Value.Value.Unsubscribe();
            }

            return parameter;
        }
    }
}
