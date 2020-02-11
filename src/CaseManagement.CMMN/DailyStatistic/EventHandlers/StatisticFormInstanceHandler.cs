using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Domains.FormInstance.Events;
using CaseManagement.CMMN.Infrastructures.Bus;
using CaseManagement.CMMN.Persistence;
using CaseManagement.CMMN.Persistence.Parameters;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.DailyStatistic.EventHandlers
{
    public class StatisticFormInstanceHandler : IMessageBrokerConsumerGeneric<FormInstanceAddedEvent>, IMessageBrokerConsumerGeneric<FormInstanceSubmittedEvent>
    {
        private readonly IDailyStatisticCommandRepository _dailyStatisticCommandRepository;
        private readonly IStatisticQueryRepository _statisticQueryRepository;

        public StatisticFormInstanceHandler(IDailyStatisticCommandRepository dailyStatisticCommandRepository, IStatisticQueryRepository statisticQueryRepository)
        {
            _dailyStatisticCommandRepository = dailyStatisticCommandRepository;
            _statisticQueryRepository = statisticQueryRepository;
        }

        public string QueueName => CMMNConstants.QueueNames.FormInstances;
        
        public async Task Handle(FormInstanceAddedEvent @event, CancellationToken cancellationToken)
        {
            var searchDailyCaseStatistics = await _statisticQueryRepository.FindDailyStatistics(new FindDailyStatisticsParameter
            {
                StartDateTime = @event.CreateDateTime.Date,
                EndDateTime = @event.CreateDateTime.Date
            });
            if (searchDailyCaseStatistics.TotalLength == 0)
            {
                var dailyCaseStatistic = new DailyStatisticAggregate
                {
                    DateTime = @event.CreateDateTime.Date
                };
                dailyCaseStatistic.IncrementCreatedForm();
                _dailyStatisticCommandRepository.Add(dailyCaseStatistic);
            }
            else
            {
                var dailyCaseStatistic = searchDailyCaseStatistics.Content.First();
                dailyCaseStatistic.IncrementCreatedForm();
                _dailyStatisticCommandRepository.Update(dailyCaseStatistic);
            }

            await _dailyStatisticCommandRepository.SaveChanges();
        }

        public async Task Handle(FormInstanceSubmittedEvent @event, CancellationToken cancellationToken)
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
                dailyCaseStatistic.IncrementConfirmedForm();
                _dailyStatisticCommandRepository.Add(dailyCaseStatistic);
            }
            else
            {
                var dailyCaseStatistic = searchDailyCaseStatistics.Content.First();
                dailyCaseStatistic.IncrementConfirmedForm();
                _dailyStatisticCommandRepository.Update(dailyCaseStatistic);
            }

            await _dailyStatisticCommandRepository.SaveChanges();
        }
    }
}
