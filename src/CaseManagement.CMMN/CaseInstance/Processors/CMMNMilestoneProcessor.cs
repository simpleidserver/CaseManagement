using CaseManagement.CMMN.CaseInstance.Processors.Listeners;
using CaseManagement.CMMN.Domains;
using System;
using System.Threading;
using System.Threading.Tasks;
using static CaseManagement.CMMN.CaseInstance.Processors.Listeners.CMMNCriterionListener;

namespace CaseManagement.CMMN.CaseInstance.Processors
{
    public class CMMNMilestoneProcessor : IProcessor
    {
        public CaseElementTypes Type => CaseElementTypes.Milestone;

        public Task<ProcessorParameter> Handle(ProcessorParameter parameter, CancellationToken token)
        {
            var task = new Task<ProcessorParameter>(() =>
            {
                return HandleTask(parameter, new CancellationTokenSource());
            }, token, TaskCreationOptions.LongRunning);
            task.Start();
            return task;
        }

        private ProcessorParameter HandleTask(ProcessorParameter parameter, CancellationTokenSource tokenSource)
        {
            bool isSuspend = false;
            bool continueExecution = true;
            CMMNPlanItemTransitionListener.EventListener parentSuspendEvtListener = null;
            CMMNPlanItemTransitionListener.EventListener parentResumeEvtListener = null;
            CMMNPlanItemTransitionListener.EventListener parentTerminateEvtListener = null;
            CMMNPlanItemTransitionListener.EventListener suspendEvtListener = null;
            CMMNPlanItemTransitionListener.EventListener resumeEvtListener = null;
            CMMNPlanItemTransitionListener.EventListener terminateEvtListener = null;
            ListenEntryCriteriaResult entryCriteriaResult = null;
            var resetListener = new Action(() =>
            {
                if (entryCriteriaResult != null && !entryCriteriaResult.IsCriteriaSatisfied)
                {
                    if (entryCriteriaResult.Task.IsCanceled || entryCriteriaResult.Task.IsCompleted || entryCriteriaResult.Task.IsFaulted)
                    {
                        entryCriteriaResult.Task.Dispose();
                    }

                    entryCriteriaResult.Listener.Unsubscribe();
                }
            });
            try
            {
                parentTerminateEvtListener = CMMNPlanItemTransitionListener.Listen(parameter, CMMNTransitions.ParentTerminate, () =>
                {
                    continueExecution = false;
                    tokenSource.Cancel();
                });
                var initListener = new Action(() =>
                {
                    entryCriteriaResult = CMMNCriterionListener.ListenEntryCriteriasBg(parameter, tokenSource.Token);
                    if (entryCriteriaResult != null)
                    {
                        if (entryCriteriaResult.IsCriteriaSatisfied)
                        {
                            parameter.CaseInstance.MakeTransitionOccur(parameter.CaseElementInstance.Id);
                            continueExecution = false;
                        }
                        else
                        {
                            entryCriteriaResult.Task.ContinueWith((o) =>
                            {
                                o.Wait();
                                parameter.CaseInstance.MakeTransitionOccur(parameter.CaseElementInstance.Id);
                                continueExecution = false;
                            });
                        }
                    }
                });
                initListener();
                parentSuspendEvtListener = CMMNPlanItemTransitionListener.Listen(parameter, CMMNTransitions.ParentSuspend, () =>
                {
                    isSuspend = true;
                    resetListener();
                });
                parentResumeEvtListener = CMMNPlanItemTransitionListener.Listen(parameter, CMMNTransitions.ParentResume, () =>
                {
                    isSuspend = false;
                    initListener();
                });
                suspendEvtListener = CMMNPlanItemTransitionListener.Listen(parameter, CMMNTransitions.Suspend, () =>
                {
                    isSuspend = true;
                    resetListener();
                });
                resumeEvtListener = CMMNPlanItemTransitionListener.Listen(parameter, CMMNTransitions.Resume, () =>
                {
                    isSuspend = false;
                    initListener();
                });
                terminateEvtListener = CMMNPlanItemTransitionListener.Listen(parameter, CMMNTransitions.Terminate, () =>
                {
                    continueExecution = false;
                    tokenSource.Cancel();
                });
                while (continueExecution)
                {
                    Thread.Sleep(CMMNConstants.WAIT_INTERVAL_MS);
                    if (isSuspend)
                    {
                        continue;
                    }
                }
            }
            finally
            {
                if (parentSuspendEvtListener != null)
                {
                    parentSuspendEvtListener.Unsubscribe();
                }
                
                if (parentResumeEvtListener != null)
                {
                    parentResumeEvtListener.Unsubscribe();
                }

                if (parentTerminateEvtListener != null)
                {
                    parentTerminateEvtListener.Unsubscribe();
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

                resetListener();
            }

            return parameter;
        }
    }
}
