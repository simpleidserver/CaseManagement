using CaseManagement.CMMN.CaseInstance.Processors.Listeners;
using CaseManagement.CMMN.Domains;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CaseInstance.Processors
{
    public abstract class BaseCMMNTaskProcessor : ICMMNPlanItemProcessor
    {
        public abstract CMMNWorkflowElementTypes Type { get; }

        public Task<PlanItemProcessorParameter> Handle(PlanItemProcessorParameter parameter, CancellationToken token)
        {
            var task = new Task<PlanItemProcessorParameter>(() =>
            {
                var cancellationTokenSource = new CancellationTokenSource();
                return HandleTask(parameter, cancellationTokenSource).Result;
            }, token, TaskCreationOptions.LongRunning);
            task.Start();
            return task;
        }

        protected abstract Task Run(PlanItemProcessorParameter parameter, CancellationToken token);
        protected abstract void Unsubscribe();

        private async Task<PlanItemProcessorParameter> HandleTask(PlanItemProcessorParameter parameter, CancellationTokenSource tokenSource)
        {
            CMMNCriterionListener.Listen(parameter);
            var isManuallyActivated = CMMNManualActivationListener.Listen(parameter);
            if (!isManuallyActivated)
            {
                parameter.WorkflowInstance.MakeTransition(parameter.WorkflowElementInstance.Id, CMMNTransitions.Start);
            }

            bool isSuspend = false;
            bool isTerminate = false;
            bool isOperationExecuted = false;
            bool continueExecution = true;
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
            while (continueExecution)
            {
                Thread.Sleep(100);
                if (isSuspend)
                {
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
                    catch(Exception)
                    {
                        parameter.WorkflowInstance.MakeTransition(parameter.WorkflowElementInstance.Id, CMMNTransitions.Fault);
                        isSuspend = true;
                        Unsubscribe();
                    }
                }
            }

            parentTerminateEvtListener.Unsubscribe();
            parentSuspendEvtListener.Unsubscribe();
            parentResumeEvtListener.Unsubscribe();
            suspendEvtListener.Unsubscribe();
            terminateEvtListener.Unsubscribe();
            resumeEvtListener.Unsubscribe();
            return parameter;
        }
    }
}
