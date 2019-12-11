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
        private readonly IServiceProvider _serviceProvider;
        private readonly List<DateTime> _dateTimes;
        private WorkflowHandlerContext _context;
        private CancellationToken _token;

        public TimerEventWatcher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _dateTimes = new List<DateTime>();
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

        public void ScheduleJob(DateTime dateTime)
        {
            lock(_dateTimes)
            {
                _dateTimes.Add(dateTime);
            }
        }

        private void HandleTask()
        {
            var factory = (IProcessFlowElementProcessorFactory)_serviceProvider.GetService(typeof(IProcessFlowElementProcessorFactory));
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
                flowInstance.StartElement(element);
                flowInstance.OccurPlanItem(element);
                foreach (var nextElt in flowInstance.NextElements(element.Id))
                {
                    var newContext = new WorkflowHandlerContext(flowInstance, nextElt, factory);
                    tasks.Add(Task.Run(() => InternalStart(newContext, _token)));
                }

                Task.WhenAll(tasks).Wait();
                flowInstance.CompleteElement(element);
                var nbOccures = element.TransitionHistories.Where(t => t.Transition == CMMNPlanItemTransitions.Occur).Count();
                var timerEventListener = element.PlanItemDefinitionTimerEventListener;
                var repeatingInterval = ISO8601Parser.ParseRepeatingTimeInterval(timerEventListener.TimerExpression.Body);
                var time = ISO8601Parser.ParseTime(timerEventListener.TimerExpression.Body);
                if (repeatingInterval.RecurringTimeInterval == nbOccures || time != null)
                {
                    return;
                }
            }

            if (_token.IsCancellationRequested)
            {
                var pf = _context.ProcessFlowInstance;
                pf.CancelElement(_context.CurrentElement);
            }
        }

        private DateTime? TakeNextOccurence()
        {
            var currentDateTime = DateTime.UtcNow;
            var filtered = _dateTimes.OrderBy(d => d).Where(d => d <= currentDateTime);
            if (!filtered.Any())
            {
                return null;
            }

            var result = filtered.First();
            lock (_dateTimes)
            {
                _dateTimes.Remove(result);
            }

            return result;
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
