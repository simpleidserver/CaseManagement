using CaseManagement.CMMN.CasePlanInstance.Processors.Listeners;
using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.ISO8601;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CasePlanInstance.Processors
{
    public class CMMNTimerEventListenerProcessor : IProcessor
    {
        public CaseElementTypes Type => CaseElementTypes.TimerEventListener;

        public Task<ProcessorParameter> Handle(ProcessorParameter parameter, CancellationToken token)
        {
            var task = new Task<ProcessorParameter>(() =>
            {
                return HandleTask(parameter, new CancellationTokenSource()).Result;
            }, token, TaskCreationOptions.LongRunning);
            task.Start();
            return task;
        }

        private async Task<ProcessorParameter> HandleTask(ProcessorParameter parameter, CancellationTokenSource tokenSource)
        {
            var timerEventListener = (parameter.CaseInstance.GetWorkflowElementDefinition(parameter.CaseElementInstance.Id, parameter.CaseDefinition) as PlanItemDefinition).PlanItemDefinitionTimerEventListener;
            var repeatingInterval = ISO8601Parser.ParseRepeatingTimeInterval(timerEventListener.TimerExpression.Body);
            var time = ISO8601Parser.ParseTime(timerEventListener.TimerExpression.Body);
            var currentDateTime = DateTime.UtcNow;
            var taskLst = new List<Task>();
            var dates = new Queue<DateTime>();
            if (repeatingInterval != null)
            {
                if (currentDateTime < repeatingInterval.Interval.EndDateTime)
                {
                    var startDate = currentDateTime;
                    if (startDate < repeatingInterval.Interval.StartDateTime)
                    {
                        startDate = repeatingInterval.Interval.StartDateTime;
                    }

                    var diff = repeatingInterval.Interval.EndDateTime.Subtract(startDate);
                    var newTimespan = new TimeSpan(diff.Ticks / (repeatingInterval.RecurringTimeInterval));
                    for (var i = 0; i < repeatingInterval.RecurringTimeInterval; i++)
                    {
                        currentDateTime = currentDateTime.Add(newTimespan);
                        var subParameter = parameter;
                        if (i > 0)
                        {
                            var newInstance = parameter.CaseInstance.CreateWorkflowElementInstance(parameter.CaseElementInstance.CasePlanElementId, parameter.CaseElementInstance.CasePlanElementName, parameter.CaseElementInstance.CasePlanElementType);
                            subParameter = new ProcessorParameter(parameter.CaseDefinition, parameter.CaseInstance, newInstance);
                        }

                        taskLst.Add(BuildTask(subParameter, currentDateTime));
                    }
                }
            }
            else if (time != null)
            {
                var subTask = Task.Factory.StartNew(() =>
                {
                    HandleTask(parameter, time.Value);
                }, TaskCreationOptions.LongRunning);
                taskLst.Add(subTask);
            }

            await Task.WhenAll(taskLst);
            return parameter;
        }

        private Task BuildTask(ProcessorParameter parameter, DateTime date)
        {
            var result = Task.Factory.StartNew(() =>
            {
                HandleTask(parameter, date);
            }, TaskCreationOptions.LongRunning);
            return result;
        }

        private void HandleTask(ProcessorParameter parameter, DateTime date)
        {
            bool continueExecution = true;
            bool isSuspend = false;
            var parentSuspendEvtListener = CasePlanElementInstanceTransitionListener.Listen(parameter, CMMNTransitions.ParentSuspend, () =>
            {
                isSuspend = true;
            });
            var parentResumeEvtListener = CasePlanElementInstanceTransitionListener.Listen(parameter, CMMNTransitions.ParentResume, () =>
            {
                isSuspend = false;
            });
            var parentTerminateEvtListener = CasePlanElementInstanceTransitionListener.Listen(parameter, CMMNTransitions.ParentTerminate, () =>
            {
                continueExecution = false;
            });
            var suspendEvtListener = CasePlanElementInstanceTransitionListener.Listen(parameter, CMMNTransitions.Suspend, () =>
            {
                isSuspend = true;
            });
            var resumeEvtListener = CasePlanElementInstanceTransitionListener.Listen(parameter, CMMNTransitions.Resume, () =>
            {
                isSuspend = false;
            });
            var terminateEvtListener = CasePlanElementInstanceTransitionListener.Listen(parameter, CMMNTransitions.Terminate, () =>
            {
                continueExecution = false;
            });
            while (continueExecution)
            {
                Thread.Sleep(CMMNConstants.WAIT_INTERVAL_MS);
                if (isSuspend)
                {
                    continue;
                }

                if (DateTime.UtcNow >= date)
                {
                    parameter.CaseInstance.MakeTransitionOccur(parameter.CaseElementInstance.Id);
                    continueExecution = false;
                }
            }

            parentSuspendEvtListener.Unsubscribe();
            parentResumeEvtListener.Unsubscribe();
            parentTerminateEvtListener.Unsubscribe();
            suspendEvtListener.Unsubscribe();
            resumeEvtListener.Unsubscribe();
            terminateEvtListener.Unsubscribe();
        }
    }
}
