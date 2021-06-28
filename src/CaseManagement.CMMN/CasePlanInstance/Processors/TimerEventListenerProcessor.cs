using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Extensions;
using CaseManagement.CMMN.Persistence;
using CaseManagement.Common.ISO8601;
using MassTransit;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CasePlanInstance.Processors
{
    public class TimerEventListenerProcessor : BaseMilestoneOrTimerProcessor
    {
        private readonly IMessageScheduler _messageScheduler;

        public TimerEventListenerProcessor(ISubscriberRepository subscriberRepository, IMessageScheduler messageScheduler) : base(subscriberRepository)
        {
            _messageScheduler = messageScheduler;
        }

        public override CasePlanElementInstanceTypes Type => CasePlanElementInstanceTypes.TIMER;

        protected override async Task ProtectedProcess(CMMNExecutionContext executionContext, CaseEltInstance elt, CancellationToken cancellationToken)
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
                var sub = await TryReset(executionContext, elt, CMMNConstants.ExternalTransitionNames.Occur, cancellationToken);
                executionContext.Instance.MakeTransition(elt, CMMNTransitions.Occur, incomingTokens: MergeParameters(executionContext, sub.Parameters));
            }
        }

        private async Task Init(CMMNExecutionContext executionContext, CaseEltInstance elt, CancellationToken token)
        {
            var currentDateTime = DateTime.UtcNow;
            var elapsedTime = ISO8601Parser.ParseTime(elt.GetTimerExpression().Body);
            var repeatingInterval = ISO8601Parser.ParseRepeatingTimeInterval(elt.GetTimerExpression().Body);
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
                        newInstance = executionContext.Instance.TryCreateInstance(elt);
                    }

                    await TrySubscribe(executionContext, newInstance, CMMNConstants.ExternalTransitionNames.Occur, token);
                    await _messageScheduler.SchedulePublish(currentDateTime, BuildEvent(executionContext.Instance.AggregateId, newInstance.Id), token);
                }
            }

            if (elapsedTime != null)
            {
                if (currentDateTime >= elapsedTime)
                {
                    return;
                }

                await TrySubscribe(executionContext, elt, CMMNConstants.ExternalTransitionNames.Occur, token);
                await _messageScheduler.SchedulePublish(elapsedTime.Value, BuildEvent(executionContext.Instance.AggregateId, elt.Id), token);
            }
        }

        private static CaseElementOccuredEvent BuildEvent(string aggregateId, string eltId)
        {
            return new CaseElementOccuredEvent(Guid.NewGuid().ToString(), aggregateId, 0, eltId);
        }
    }
}
