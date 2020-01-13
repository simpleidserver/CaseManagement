using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Domains.Events;
using CaseManagement.CMMN.Persistence;
using CaseManagement.CMMN.Persistence.Parameters;
using CaseManagement.Workflow.Infrastructure;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CaseInstance.EventHandlers
{
    public class StatisticHandler : IDomainEventHandler<CaseInstanceCreatedEvent>, IDomainEventHandler<CaseElementCreatedEvent>, IDomainEventHandler<CaseTransitionRaisedEvent>, IDomainEventHandler<CaseElementInstanceFormCreatedEvent>, IDomainEventHandler<CaseElementInstanceFormSubmittedEvent>, IDomainEventHandler<CaseElementTransitionRaisedEvent>
    {
        private readonly IStatisticCommandRepository _staticicCommandRepository;
        private readonly IStatisticQueryRepository _statisticQueryRepository;
        private readonly IWorkflowInstanceQueryRepository _cmmnWorkflowInstanceQueryRepository;

        public StatisticHandler(IStatisticCommandRepository statisticCommandRepository, IStatisticQueryRepository statisticQueryRepository, IWorkflowInstanceQueryRepository cmmnWorkflowInstanceQueryRepository)
        {
            _staticicCommandRepository = statisticCommandRepository;
            _statisticQueryRepository = statisticQueryRepository;
            _cmmnWorkflowInstanceQueryRepository = cmmnWorkflowInstanceQueryRepository;
        }

        public async Task Handle(CaseInstanceCreatedEvent @event, CancellationToken cancellationToken)
        {
            var stat = await _statisticQueryRepository.FindById(@event.CaseDefinitionId);
            if (stat == null)
            {
                _staticicCommandRepository.Add(new CaseDefinitionStatisticAggregate
                {
                    NbInstances = 1,
                    CaseDefinitionId  = @event.CaseDefinitionId
                });
            }
            else
            {
                stat.NbInstances++;
                _staticicCommandRepository.Update(stat);
            }

            await _staticicCommandRepository.SaveChanges();
        }

        public async Task Handle(CaseElementCreatedEvent @event, CancellationToken cancellationToken)
        {            
            var workflowInstance = await _cmmnWorkflowInstanceQueryRepository.FindFlowInstanceById(@event.AggregateId);
            var stat = await _statisticQueryRepository.FindById(workflowInstance.CaseDefinitionId);
            var statElt = stat.Statistics.FirstOrDefault(s => s.CaseElementDefinitionId == @event.CaseElementDefinitionId);
            if (statElt == null)
            {
                statElt = new CaseElementDefinitionStatistic { CaseElementDefinitionId = @event.CaseElementDefinitionId, NbInstances = 1 };
                stat.Statistics.Add(statElt);
            }
            else
            {
                statElt.NbInstances++;
                _staticicCommandRepository.Update(stat);
            }

            await _staticicCommandRepository.SaveChanges();
        }

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
                _staticicCommandRepository.Add(dailyCaseStatistic);
            }
            else
            {
                var dailyCaseStatistic = searchDailyCaseStatistics.Content.First();
                dailyCaseStatistic.Increment(@event.Transition);
                _staticicCommandRepository.Update(dailyCaseStatistic);
            }

            await _staticicCommandRepository.SaveChanges();
        }

        public async Task Handle(CaseElementInstanceFormCreatedEvent @event, CancellationToken cancellationToken)
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
                _staticicCommandRepository.Add(dailyCaseStatistic);
            }
            else
            {
                var dailyCaseStatistic = searchDailyCaseStatistics.Content.First();
                dailyCaseStatistic.IncrementCreatedForm();
                _staticicCommandRepository.Update(dailyCaseStatistic);
            }

            await _staticicCommandRepository.SaveChanges();
        }

        public async Task Handle(CaseElementInstanceFormSubmittedEvent @event, CancellationToken cancellationToken)
        {
            var searchDailyCaseStatistics = await _statisticQueryRepository.FindDailyStatistics(new FindDailyStatisticsParameter
            {
                StartDateTime = @event.UpdatedDateTime.Date,
                EndDateTime = @event.UpdatedDateTime.Date
            });
            if (searchDailyCaseStatistics.TotalLength == 0)
            {
                var dailyCaseStatistic = new DailyStatisticAggregate
                {
                    DateTime = @event.UpdatedDateTime.Date
                };
                dailyCaseStatistic.IncrementConfirmedForm();
                _staticicCommandRepository.Add(dailyCaseStatistic);
            }
            else
            {
                var dailyCaseStatistic = searchDailyCaseStatistics.Content.First();
                dailyCaseStatistic.IncrementConfirmedForm();
                _staticicCommandRepository.Update(dailyCaseStatistic);
            }

            await _staticicCommandRepository.SaveChanges();
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

                _staticicCommandRepository.Add(dailyCaseStatistic);
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

                _staticicCommandRepository.Update(dailyCaseStatistic);
            }
        }
    }
}
