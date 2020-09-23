using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Infrastructure.ExternalEvts;
using CaseManagement.CMMN.ISO8601;
using CaseManagement.Common.Bus;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CasePlanInstance.Processors
{
    public class TimerEventListenerProcessor : BaseMilestoneOrTimerProcessor<TimerEventListener>
    {
        private readonly IMessageBroker _messageBroker;

        public TimerEventListenerProcessor(ISubscriberRepository subscriberRepository, IMessageBroker messageBroker) : base(subscriberRepository)
        {
            _messageBroker = messageBroker;
        }

        protected override async Task ProtectedProcess(CMMNExecutionContext executionContext, TimerEventListener elt, CancellationToken cancellationToken)
        {
            if (elt.NbOccurrence == 0)
            {
                var subExists = (await SubscriberRepository.Get(executionContext.Instance.AggregateId, elt.Id, CMMNConstants.ExternalTransitionNames.Occur, cancellationToken) != null);
                if (!subExists)
                {
                    await Init(executionContext, elt, cancellationToken);
                    return;
                }
            }

            var subscription = await TrySubscribe(executionContext, elt, CMMNConstants.ExternalTransitionNames.Occur, cancellationToken);
            if (subscription.IsCaptured)
            {
                executionContext.Instance.MakeTransition(elt, CMMNTransitions.Occur);
            }
        }

        private async Task Init(CMMNExecutionContext executionContext, TimerEventListener elt, CancellationToken token)
        {
            var currentDateTime = DateTime.UtcNow;
            var elapsedTime = ISO8601Parser.ParseTime(elt.TimerExpression.Body);
            var repeatingInterval = ISO8601Parser.ParseRepeatingTimeInterval(elt.TimerExpression.Body);
            if (repeatingInterval != null)
            {
                if (currentDateTime >= repeatingInterval.Interval.EndDateTime)
                {
                    return;
                }

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
                    var newInstance = elt;
                    if (i > 0)
                    {
                        newInstance = executionContext.Instance.TryCreateInstance(elt) as TimerEventListener;
                    }

                    await TrySubscribe(executionContext, newInstance, CMMNConstants.ExternalTransitionNames.Occur, token);
                    await _messageBroker.ScheduleExternalEvt(CMMNConstants.ExternalTransitionNames.Occur,
                        executionContext.Instance.AggregateId,
                        newInstance.Id,
                        currentDateTime,
                        token);
                }
            }

            if (elapsedTime != null)
            {
                if (currentDateTime >= elapsedTime)
                {
                    return;
                }

                await TrySubscribe(executionContext, elt, CMMNConstants.ExternalTransitionNames.Occur, token);
                await _messageBroker.ScheduleExternalEvt(CMMNConstants.ExternalTransitionNames.Occur, 
                    executionContext.Instance.AggregateId,
                    elt.Id, 
                    elapsedTime.Value, 
                    token);
            }
        }
    }
}
