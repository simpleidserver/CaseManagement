using CaseManagement.CMMN.Domains;
using CaseManagement.Workflow.Domains;
using CaseManagement.Workflow.Infrastructure.EvtBus;
using CaseManagement.Workflow.Infrastructure.EvtStore;
using CaseManagement.Workflow.ISO8601;
using CaseManagement.Workflow.Persistence;
using Hangfire;
using Microsoft.Extensions.Options;
using NEventStore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CaseInstance.Processors
{
    public class CMMNTimerEventListenerProcessor : BaseCommandHandler<ProcessFlowInstance>, ICMMNPlanItemDefinitionProcessor
    {
        private readonly IProcessFlowInstanceQueryRepository _processFlowInstanceQueryRepository;
        private readonly IProcessFlowInstanceCommandRepository _processFlowInstanceCommandRepository;
        private readonly IBackgroundJobClient _backgroundJobClient;

        public CMMNTimerEventListenerProcessor(IProcessFlowInstanceQueryRepository processFlowInstanceQueryRepository, IProcessFlowInstanceCommandRepository processFlowInstanceCommandRepository, IBackgroundJobClient  backgroundJobClient, IStoreEvents storeEvents, IEventBus eventBus, IAggregateSnapshotStore<ProcessFlowInstance> aggregateSnapshotStore, IOptions<SnapshotConfiguration> options) : base(storeEvents, eventBus, aggregateSnapshotStore, options)
        {
            _processFlowInstanceQueryRepository = processFlowInstanceQueryRepository;
            _processFlowInstanceCommandRepository = processFlowInstanceCommandRepository;
            _backgroundJobClient = backgroundJobClient;
        }

        public Type PlanItemDefinitionType => typeof(CMMNTimerEventListener);

        public Task<bool> Handle(CMMNPlanItem cmmnPlanItem, ProcessFlowInstance pf)
        {
            var timerEventListener = (CMMNTimerEventListener)cmmnPlanItem.PlanItemDefinition;
            var repeatingInterval = ISO8601Parser.ParseRepeatingTimeInterval(timerEventListener.TimerExpression.Body);
            var time = ISO8601Parser.ParseTime(timerEventListener.TimerExpression.Body);
            var currentDateTime = DateTime.UtcNow;
            if (repeatingInterval != null)
            {
                if (currentDateTime < repeatingInterval.Interval.EndDateTime)
                {
                    cmmnPlanItem.Create();
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
                        _backgroundJobClient.Schedule(() => HandlePlanItemEventListener(pf.Id, cmmnPlanItem.Id), currentDateTime);
                    }
                }
            }

            if (time != null && currentDateTime < time.Value)
            {
                cmmnPlanItem.Create();
                _backgroundJobClient.Schedule(() => HandlePlanItemEventListener(pf.Id, cmmnPlanItem.Id), time.Value);
            }
            
            return Task.FromResult(false);
        }

        [DisableConcurrentExecution(60)]
        public async Task HandlePlanItemEventListener(string processInstanceId, string processInstanceEltId)
        {
            await ExecuteTimer(processInstanceId, processInstanceEltId);
            var flowInstance = await _processFlowInstanceQueryRepository.FindFlowInstanceById(processInstanceId);
            var cmmnPlanItem = flowInstance.Elements.First(e => e.Id == processInstanceEltId) as CMMNPlanItem;
            var nbOccures = cmmnPlanItem.TransitionHistories.Where(t => t.Transition == CMMNPlanItemTransitions.Occur).Count();
            var timerEventListener = (CMMNTimerEventListener)cmmnPlanItem.PlanItemDefinition;
            var repeatingInterval = ISO8601Parser.ParseRepeatingTimeInterval(timerEventListener.TimerExpression.Body);
            var time = ISO8601Parser.ParseTime(timerEventListener.TimerExpression.Body);
            if (repeatingInterval.RecurringTimeInterval == nbOccures || time != null)
            {
                flowInstance.CompleteElement(cmmnPlanItem);
                if (flowInstance.IsFinished())
                {
                    flowInstance.Complete();
                }
            }

            _processFlowInstanceCommandRepository.Update(flowInstance);
            await _processFlowInstanceCommandRepository.SaveChanges();
        }

        public async Task ExecuteTimer(string processInstanceId, string processInstanceEltId)
        {
            var flowInstance = await _processFlowInstanceQueryRepository.FindFlowInstanceById(processInstanceId);
            var cmmnPlanItem = flowInstance.Elements.First(e => e.Id == processInstanceEltId) as CMMNPlanItem;
            cmmnPlanItem.Occur();
            _processFlowInstanceCommandRepository.Update(flowInstance);
            await _processFlowInstanceCommandRepository.SaveChanges();
            foreach (var elt in flowInstance.NextElements(processInstanceEltId))
            {
                flowInstance.LaunchElement(elt.Id);
            }

            await Commit(flowInstance, flowInstance.GetStreamName());
        }
    }
}
