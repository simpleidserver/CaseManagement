using CaseManagement.CMMN.CaseInstance.Processors.Listeners;
using CaseManagement.CMMN.Domains;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using static CaseManagement.CMMN.CaseInstance.Processors.Listeners.CMMNCriterionListener;

namespace CaseManagement.CMMN.CaseInstance.Processors
{
    public class CMMNMilestoneProcessor : IProcessor
    {
        public CMMNWorkflowElementTypes Type => CMMNWorkflowElementTypes.Milestone;

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
            KeyValuePair<Task, CriterionListener>? kvp = null;
            var initListener = new Action(() =>
            {
                kvp = CMMNCriterionListener.ListenEntryCriteriasBg(parameter);
                if (kvp != null)
                {
                    kvp.Value.Key.ContinueWith((o) =>
                    {
                        o.Wait();
                        if (parameter.WorkflowElementInstance.State == Enum.GetName(typeof(CMMNMilestoneStates), CMMNMilestoneStates.Available))
                        {
                            parameter.WorkflowInstance.MakeTransition(parameter.WorkflowElementInstance.Id, CMMNTransitions.Occur);
                            continueExecution = false;
                        }
                    });
                }
            });
            var resetListener = new Action(() =>
            {
                if (kvp != null)
                {
                    if (kvp.Value.Key.IsCanceled || kvp.Value.Key.IsCompleted || kvp.Value.Key.IsFaulted)
                    {
                        kvp.Value.Key.Dispose();
                    }

                    kvp.Value.Value.Unsubscribe();
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
                Thread.Sleep(100);
                if (isSuspend)
                {
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
