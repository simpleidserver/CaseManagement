namespace CaseManagement.CMMN.CaseInstance.Processors
{
    /*
    public class CMMNTimerEventListenerProcessor : IProcessFlowElementProcessor
    {
        private readonly ITimerEventWatcher _timerEventWatcher;
        private readonly IServiceProvider _serviceProvider;
        private readonly IProcessFlowInstanceQueryRepository _processFlowInstanceQueryRepository;
        private readonly IProcessFlowInstanceCommandRepository _processFlowInstanceCommandRepository;

        public CMMNTimerEventListenerProcessor(ITimerEventWatcher timerEventWatcher, IServiceProvider serviceProvider, IProcessFlowInstanceQueryRepository processFlowInstanceQueryRepository, IProcessFlowInstanceCommandRepository processFlowInstanceCommandRepository)
        {
            _timerEventWatcher = timerEventWatcher;
            _serviceProvider = serviceProvider;
            _processFlowInstanceQueryRepository = processFlowInstanceQueryRepository;
            _processFlowInstanceCommandRepository = processFlowInstanceCommandRepository;
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
            if (planItem.Status != ProcessFlowInstanceElementStatus.Launched)
            {
            }

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
                    for(var i = 0; i < repeatingInterval.RecurringTimeInterval; i++)
                    {
                        currentDateTime = currentDateTime.Add(newTimespan);
                        _timerEventWatcher.ScheduleJob(currentDateTime, pf.Id);
                    }
                }
            }

            if (time != null && currentDateTime < time.Value)
            {
                pf.CreatePlanItem(planItem);
                _timerEventWatcher.ScheduleJob(currentDateTime, pf.Id);
            }

            return Task.CompletedTask;
        }
    }
    */
}
