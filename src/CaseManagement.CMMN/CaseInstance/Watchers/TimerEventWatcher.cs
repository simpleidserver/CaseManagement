using CaseManagement.CMMN.Domains;
using CaseManagement.Workflow.Domains;
using CaseManagement.Workflow.Engine;
using CaseManagement.Workflow.ISO8601;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CaseInstance.Watchers
{
    public class TimerEventWatcher : ITimerEventWatcher
    {
        private class Job
        {
            public Job(string processId, DateTime scheduleDateTime)
            {
                ProcessId = processId;
                ScheduleDateTime = scheduleDateTime;
            }

            public string ProcessId { get; set; }
            public DateTime ScheduleDateTime { get; set; }
        }
        
        private readonly List<Job> _jobs;
        private WorkflowHandlerContext _context;
        private CancellationToken _token;

        public TimerEventWatcher()
        {
            _jobs = new List<Job>();
        }

        public Task Task { get; private set; }

        public Task Start(WorkflowHandlerContext context, CancellationToken token)
        {
            _context = context;
            _token = token;
            Task = new Task(HandleTask, token, TaskCreationOptions.LongRunning);
            Task.Start();
            return Task.CompletedTask;
        }

        public void ScheduleJob(DateTime dateTime, string id)
        {
            lock(_jobs)
            {
                _jobs.Add(new Job(id, dateTime));
            }
        }

        private void HandleTask()
        {
            var factory = _context.Factory;
            while (!_token.IsCancellationRequested)
            {
                var occurence = TakeNextOccurence();
                if (occurence == null)
                {
                    continue;
                }

                var flowInstance = _context.ProcessFlowInstance;
                var tasks = new List<Task>();
                var element = _context.CurrentElement as CMMNPlanItem;
                flowInstance.CreatePlanItem(element);
                flowInstance.OccurPlanItem(element);
                foreach (var nextElt in flowInstance.NextElements(element.Id))
                {
                    var newContext = new WorkflowHandlerContext(flowInstance, nextElt, factory);
                    tasks.Add(Task.Run(() => newContext.Execute(_token)));
                }

                Task.WhenAll(tasks).Wait();
                var nbOccures = element.TransitionHistories.Where(t => t.Transition == CMMNPlanItemTransitions.Occur).Count();
                var timerEventListener = element.PlanItemDefinitionTimerEventListener;
                var repeatingInterval = ISO8601Parser.ParseRepeatingTimeInterval(timerEventListener.TimerExpression.Body);
                var time = ISO8601Parser.ParseTime(timerEventListener.TimerExpression.Body);
                if (repeatingInterval.RecurringTimeInterval == nbOccures || time != null)
                {
                    break;
                }
            }

            var pf = _context.ProcessFlowInstance;
            if (_token.IsCancellationRequested)
            {
                pf.CancelElement(_context.CurrentElement);
            }
            else
            {
                _context.Complete();
            }
        }

        private DateTime? TakeNextOccurence()
        {
            var currentDateTime = DateTime.UtcNow;
            var filtered = _jobs.Where(j => j.ProcessId == _context.ProcessFlowInstance.Id && j.ScheduleDateTime <= currentDateTime).OrderBy(d => d.ScheduleDateTime);
            if (!filtered.Any())
            {
                return null;
            }

            var result = filtered.First();
            lock (filtered)
            {
                _jobs.Remove(result);
            }

            return result.ScheduleDateTime;
        }

        private async Task InternalStart(WorkflowHandlerContext context, CancellationToken token)
        {
            var processor = context.Factory.Build(context.CurrentElement);
            await processor.Handle(context, token);
            if (context.CurrentElement.Status != ProcessFlowInstanceElementStatus.Finished)
            {
                return;
            }

            var nextElts = context.ProcessFlowInstance.NextElements(context.CurrentElement.Id);
            if (!nextElts.Any())
            {
                return;
            }

            foreach (var nextElt in nextElts)
            {
                await Start(context, token);
            }
        }
    }
}
