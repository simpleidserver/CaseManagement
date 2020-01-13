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
                return HandleTask(parameter);
            }, token, TaskCreationOptions.LongRunning);
            task.Start();
            return task;
        }

        private ProcessorParameter HandleTask(ProcessorParameter parameter)
        {
            bool isSuspend = false;
            bool continueExecution = true;
            ListenEntryCriteriaResult entryCriteriaResult = null;
            var initListener = new Action(() =>
            {
                entryCriteriaResult = CMMNCriterionListener.ListenEntryCriteriasBg(parameter);
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
            initListener();
            var parentSuspendEvtListener = CMMNPlanItemTransitionListener.Listen(parameter, CMMNTransitions.ParentSuspend, () =>
            {
                isSuspend = true;
                resetListener();
            });
            var parentResumeEvtListener = CMMNPlanItemTransitionListener.Listen(parameter, CMMNTransitions.ParentResume, () =>
            {
                isSuspend = false;
                initListener();
            });
            var parentTerminateEvtListener = CMMNPlanItemTransitionListener.Listen(parameter, CMMNTransitions.ParentTerminate, () =>
            {
                continueExecution = false;
            });
            var suspendEvtListener = CMMNPlanItemTransitionListener.Listen(parameter, CMMNTransitions.Suspend, () =>
            {
                isSuspend = true;
                resetListener();
            });
            var resumeEvtListener = CMMNPlanItemTransitionListener.Listen(parameter, CMMNTransitions.Resume, () =>
            {
                isSuspend = false;
                initListener();
            });
            var terminateEvtListener = CMMNPlanItemTransitionListener.Listen(parameter, CMMNTransitions.Terminate, () =>
            {
                continueExecution = false;
            });
            while (continueExecution)
            {
                if (isSuspend)
                {
                    Thread.Sleep(100);
                    continue;
                }
            }

            parentSuspendEvtListener.Unsubscribe();
            parentResumeEvtListener.Unsubscribe();
            parentTerminateEvtListener.Unsubscribe();
            suspendEvtListener.Unsubscribe();
            resumeEvtListener.Unsubscribe();
            terminateEvtListener.Unsubscribe();
            resetListener();
            return parameter;
        }
    }
}
