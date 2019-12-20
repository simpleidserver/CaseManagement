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

        public Task Handle(PlanItemProcessorParameter parameter, CancellationToken token)
        {
            var cancellationTokenSource = new CancellationTokenSource();
            var task = new Task(() => HandleTask(parameter, cancellationTokenSource), token, TaskCreationOptions.LongRunning);
            task.Start();
            return task;
        }

        protected abstract Task Run(PlanItemProcessorParameter parameter, CancellationToken token);

        private async void HandleTask(PlanItemProcessorParameter parameter, CancellationTokenSource tokenSource)
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
                    }
                    catch(Exception)
                    {
                        parameter.WorkflowInstance.MakeTransition(parameter.WorkflowElementInstance.Id, CMMNTransitions.Fault);
                        isSuspend = true;
                    }
                }
            }

            suspendEvtListener.Unsubscribe();
            terminateEvtListener.Unsubscribe();
            resumeEvtListener.Unsubscribe();
        }
    }
}
