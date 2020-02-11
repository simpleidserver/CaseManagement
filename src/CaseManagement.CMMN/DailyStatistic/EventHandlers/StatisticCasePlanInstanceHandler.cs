using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Domains.Events;
using CaseManagement.CMMN.Infrastructures.Bus;
using CaseManagement.CMMN.Persistence;
using CaseManagement.CMMN.Persistence.Parameters;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.DailyStatistic.EventHandlers
{
    public class StatisticCasePlanInstanceHandler : IMessageBrokerConsumerGeneric<CaseTransitionRaisedEvent>, IMessageBrokerConsumerGeneric<CaseElementTransitionRaisedEvent>
    {
        private readonly IDailyStatisticCommandRepository _dailyStatisticCommandRepository;
        private readonly IStatisticQueryRepository _statisticQueryRepository;

        public StatisticCasePlanInstanceHandler(IDailyStatisticCommandRepository dailyStatisticCommandRepository, IStatisticQueryRepository statisticQueryRepository)
        {
            _dailyStatisticCommandRepository = dailyStatisticCommandRepository;
            _statisticQueryRepository = statisticQueryRepository;
        }

        public string QueueName => CMMNConstants.QueueNames.CasePlanInstances;
        
        public async Task Handle(CaseTransitionRaisedEvent @event, CancellationToken cancellationToken)
        {
            var searchDailyCaseStatistics = await _statisticQueryRepository.FindDailyStatistics(new FindDailyStatisticsParameter
            {
                StartDateTime = @event.UpdateDateTime.Date,
                EndDateTime = @event.UpdateDateTime.Date
            });
            if (searchDailyCaseStatistics.TotalLength == 0)
            {
                var dailyCaseStatistic = new DailyStatisticAggregate
                {
                    DateTime = @event.UpdateDateTime.Date
                };
                dailyCaseStatistic.Increment(@event.Transition);
                _dailyStatisticCommandRepository.Add(dailyCaseStatistic);
            }
            else
            {
                var dailyCaseStatistic = searchDailyCaseStatistics.Content.First();
                dailyCaseStatistic.Increment(@event.Transition);
                _dailyStatisticCommandRepository.Update(dailyCaseStatistic);
            }

            await _dailyStatisticCommandRepository.SaveChanges();
        }

        public async Task Handle(CaseElementTransitionRaisedEvent @event, CancellationToken cancellationToken)
        {
            if (@event.Transition != CMMNTransitions.Enable && @event.Transition != CMMNTransitions.ManualStart)
            {
                return;
            }

            var searchDailyCaseStatistics = await _statisticQueryRepository.FindDailyStatistics(new FindDailyStatisticsParameter
            {
                StartDateTime = @event.UpdateDateTime.Date,
                EndDateTime = @event.UpdateDateTime.Date
            });
            if (searchDailyCaseStatistics.TotalLength == 0)
            {
                var dailyCaseStatistic = new DailyStatisticAggregate
                {
                    DateTime = @event.UpdateDateTime.Date
                };
                if(@event.Transition == CMMNTransitions.Enable)
                {
                    dailyCaseStatistic.IncrementCreatedActivation();
                }
                else
                {
                    dailyCaseStatistic.IncrementConfirmedActivation();
                }

                _dailyStatisticCommandRepository.Add(dailyCaseStatistic);
            }
            else
            {
                var dailyCaseStatistic = searchDailyCaseStatistics.Content.First();
                if (@event.Transition == CMMNTransitions.Enable)
                {
                    dailyCaseStatistic.IncrementCreatedActivation();
                }
                else
                {
                    dailyCaseStatistic.IncrementConfirmedActivation();
                }

                _dailyStatisticCommandRepository.Update(dailyCaseStatistic);
            }
        }
    }
}
