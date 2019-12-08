using CaseManagement.CMMN.Domains;
using CaseManagement.Workflow.Domains;
using CaseManagement.Workflow.Engine;
using CaseManagement.Workflow.Infrastructure;
using CaseManagement.Workflow.Infrastructure.Bus;
using CaseManagement.Workflow.Infrastructure.EvtStore;
using CaseManagement.Workflow.Infrastructure.Lock;
using CaseManagement.Workflow.Infrastructure.Scheduler;
using CaseManagement.Workflow.ISO8601;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Infrastructures.Scheduler
{
    public class CMMNTimerEventHandler : IScheduleJobHandler<TimerEventMessage>
    {
        private readonly IDistributedLock _distributedLock;
        private readonly IProcessFlowElementProcessorFactory _factory;
        private readonly ICommitAggregateHelper _commitAggregateHelper;
        private readonly IEventStoreRepository _eventStoreRepository;
        private readonly BusOptions _busOptions;

        public CMMNTimerEventHandler(IDistributedLock distributedLock, IProcessFlowElementProcessorFactory factory, ICommitAggregateHelper commitAggregateHelper, IEventStoreRepository eventStoreRepository, IOptions<BusOptions> options)
        {
            _distributedLock = distributedLock;
            _factory = factory;
            _commitAggregateHelper = commitAggregateHelper;
            _eventStoreRepository = eventStoreRepository;
            _busOptions = options.Value;
        }

        public async Task Handle(TimerEventMessage message, CancellationToken token)
        {
            if (!await _distributedLock.AcquireLock(message.ProcessId))
            {
                await Task.Delay(_busOptions.ConcurrencyExceptionIdleTimeInMs);
                await Handle(message, token);
                return;
            }

            var flowInstance = await _eventStoreRepository.GetLastAggregate<CMMNProcessFlowInstance>(message.ProcessId, CMMNProcessFlowInstance.GetCMMNStreamName(message.ProcessId));
            if (flowInstance == null)
            {
                return;
            }

            try
            {
                var tasks = new List<Task>();
                var element = flowInstance.Elements.First(e => e.Id == message.ElementId) as CMMNPlanItem;
                flowInstance.StartElement(element);
                flowInstance.OccurPlanItem(element);
                foreach(var nextElt in flowInstance.NextElements(element.Id))
                {
                    var context = new WorkflowHandlerContext(flowInstance, nextElt, _factory);
                    tasks.Add(Task.Run(() => Start(context, token)));
                }

                await Task.WhenAll(tasks);
                flowInstance.CompleteElement(element);
                var nbOccures = element.TransitionHistories.Where(t => t.Transition == CMMNPlanItemTransitions.Occur).Count();
                var timerEventListener = element.PlanItemDefinitionTimerEventListener;
                var repeatingInterval = ISO8601Parser.ParseRepeatingTimeInterval(timerEventListener.TimerExpression.Body);
                var time = ISO8601Parser.ParseTime(timerEventListener.TimerExpression.Body);
                if ((repeatingInterval.RecurringTimeInterval == nbOccures || time != null) && flowInstance.IsFinished())
                {
                    if (flowInstance.IsFinished())
                    {
                        flowInstance.Complete();
                    }
                }
            }
            finally
            {
                await _distributedLock.ReleaseLock(message.ProcessId);
                await _commitAggregateHelper.Commit(flowInstance, flowInstance.GetStreamName());
            }
        }
        
        private async Task Start(WorkflowHandlerContext context, CancellationToken token)
        {
            var processor = _factory.Build(context.CurrentElement);
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
