using CaseManagement.CMMN.Domains;
using CaseManagement.Workflow.Domains;
using CaseManagement.Workflow.Engine;
using CaseManagement.Workflow.Infrastructure;
using CaseManagement.Workflow.Infrastructure.EvtStore;
using CaseManagement.Workflow.Infrastructure.Scheduler;
using CaseManagement.Workflow.ISO8601;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Infrastructures.Scheduler
{
    public class CMMNTimerEventHandler : IScheduleJobHandler<TimerEventMessage>
    {
        private readonly IProcessFlowElementProcessorFactory _factory;
        private readonly ICommitAggregateHelper _commitAggregateHelper;
        private readonly IEventStoreRepository _eventStoreRepository;

        public CMMNTimerEventHandler(IProcessFlowElementProcessorFactory factory, ICommitAggregateHelper commitAggregateHelper, IEventStoreRepository eventStoreRepository)
        {
            _factory = factory;
            _commitAggregateHelper = commitAggregateHelper;
            _eventStoreRepository = eventStoreRepository;
        }

        public async Task Handle(TimerEventMessage message, CancellationToken token)
        {
            var flowInstance = await _eventStoreRepository.GetLastAggregate<ProcessFlowInstance>(message.ProcessId, ProcessFlowInstance.GetStreamName(message.ProcessId));
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
