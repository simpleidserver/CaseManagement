using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Extensions;
using CaseManagement.Workflow.Domains;
using CaseManagement.Workflow.Engine;
using CaseManagement.Workflow.Infrastructure.Scheduler;
using CaseManagement.Workflow.ISO8601;
using CaseManagement.Workflow.Persistence;
using Hangfire;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CaseManagement.CMMN.Infrastructures.Scheduler;

namespace CaseManagement.CMMN.CaseInstance.Processors
{
    public class CMMNTimerEventListenerProcessor : IProcessFlowElementProcessor
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IProcessFlowInstanceQueryRepository _processFlowInstanceQueryRepository;
        private readonly IProcessFlowInstanceCommandRepository _processFlowInstanceCommandRepository;
        private readonly IScheduleJobStore _scheduleJobStore;
        private readonly IBackgroundJobClient _backgroundJobClient;

        public CMMNTimerEventListenerProcessor(IServiceProvider serviceProvider, IProcessFlowInstanceQueryRepository processFlowInstanceQueryRepository, IProcessFlowInstanceCommandRepository processFlowInstanceCommandRepository, IScheduleJobStore schedulerJobStore, IBackgroundJobClient backgroundJobClient)
        {
            _serviceProvider = serviceProvider;
            _processFlowInstanceQueryRepository = processFlowInstanceQueryRepository;
            _processFlowInstanceCommandRepository = processFlowInstanceCommandRepository;
            _scheduleJobStore = schedulerJobStore;
            _backgroundJobClient = backgroundJobClient;
        }

        public string ProcessFlowElementType => Enum.GetName(typeof(CMMNPlanItemDefinitionTypes), CMMNPlanItemDefinitionTypes.TimerEventListener).ToLowerInvariant();

        public Task Handle(WorkflowHandlerContext context, CancellationToken token)
        {
            var pf = context.ProcessFlowInstance;
            var planItem = context.GetCMMNPlanItem();
            var timerEventListener = planItem.PlanItemDefinitionTimerEventListener;
            var repeatingInterval = ISO8601Parser.ParseRepeatingTimeInterval(timerEventListener.TimerExpression.Body);
            var time = ISO8601Parser.ParseTime(timerEventListener.TimerExpression.Body);
            var currentDateTime = DateTime.UtcNow;
            if (repeatingInterval != null)
            {
                if (currentDateTime < repeatingInterval.Interval.EndDateTime)
                {
                    pf.CreatePlanItem(planItem);                    
                    var startDate = currentDateTime;
                    if (startDate < repeatingInterval.Interval.StartDateTime)
                    {
                        startDate = repeatingInterval.Interval.StartDateTime;
                    }

                    var diff = repeatingInterval.Interval.EndDateTime.Subtract(startDate);
                    var newTimespan = new TimeSpan(diff.Ticks / (repeatingInterval.RecurringTimeInterval));
                    for(var i = 0; i < repeatingInterval.RecurringTimeInterval; i++)
                    {
                        currentDateTime = currentDateTime.Add(newTimespan);
                        _scheduleJobStore.ScheduleJob(new TimerEventMessage { ProcessId = pf.Id, ElementId = planItem.Id}, currentDateTime);
                    }
                }
            }

            if (time != null && currentDateTime < time.Value)
            {
                pf.CreatePlanItem(planItem);
                _scheduleJobStore.ScheduleJob(new TimerEventMessage { ProcessId = pf.Id, ElementId = planItem.Id }, currentDateTime);
                // _backgroundJobClient.Schedule(() => HandleListener(pf.Id, planItem.Id), time.Value);
            }

            return Task.FromResult(0);
        }

        [DisableConcurrentExecution(60)]
        public async Task HandleListener(string processInstanceId, string processInstanceEltId)
        {
            var processFlowInstance = await _processFlowInstanceQueryRepository.FindFlowInstanceById(processInstanceId);
            var currentElement = processFlowInstance.Elements.First(e => e.Id == processInstanceEltId) as CMMNPlanItem;
            processFlowInstance.StartElement(currentElement);
            processFlowInstance.OccurPlanItem(processInstanceEltId);
            var factory = (IProcessFlowElementProcessorFactory)_serviceProvider.GetService(typeof(IProcessFlowElementProcessorFactory));
            foreach (var nextElement in processFlowInstance.NextElements(currentElement.Id))
            {
                await Start(processFlowInstance, nextElement, factory);
            }

            var nbOccures = currentElement.TransitionHistories.Where(t => t.Transition == CMMNPlanItemTransitions.Occur).Count();
            var timerEventListener = currentElement.PlanItemDefinitionTimerEventListener;
            var repeatingInterval = ISO8601Parser.ParseRepeatingTimeInterval(timerEventListener.TimerExpression.Body);
            var time = ISO8601Parser.ParseTime(timerEventListener.TimerExpression.Body);
            processFlowInstance.CompleteElement(currentElement);
            if ((repeatingInterval.RecurringTimeInterval == nbOccures || time != null) && processFlowInstance.IsFinished())
            {
                if (processFlowInstance.IsFinished())
                {
                    processFlowInstance.Complete();
                }
            }

            _processFlowInstanceCommandRepository.Update(processFlowInstance);
            await _processFlowInstanceCommandRepository.SaveChanges();
        }

        private Task Start(ProcessFlowInstance processFlowInstance, ProcessFlowInstanceElement elt, IProcessFlowElementProcessorFactory factory)
        {
            /*
            var processor = factory.Build(elt);
            await processor.Handle(processFlowInstance, elt);
            if (elt.Status != ProcessFlowInstanceElementStatus.Finished)
            {
                return;
            }

            var nextElts = processFlowInstance.NextElements(elt.Id);
            if (!nextElts.Any())
            {
                return;
            }

            foreach (var nextElt in nextElts)
            {
                await Start(processFlowInstance, nextElt, factory);
            }
            */
            return Task.FromResult(0);
        }
    }
}
