using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Domains.Events;
using CaseManagement.CMMN.Persistence;
using CaseManagement.Workflow.Infrastructure;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CaseInstance.EventHandlers
{
    public class StatisticHandler : IDomainEventHandler<CMMNWorkflowInstanceCreatedEvent>, IDomainEventHandler<CMMNWorkflowElementCreatedEvent>
    {
        private readonly IStatisticCommandRepository _staticicCommandRepository;
        private readonly IStatisticQueryRepository _statisticQueryRepository;
        private readonly ICMMNWorkflowInstanceQueryRepository _cmmnWorkflowInstanceQueryRepository;

        public StatisticHandler(IStatisticCommandRepository statisticCommandRepository, IStatisticQueryRepository statisticQueryRepository, ICMMNWorkflowInstanceQueryRepository cmmnWorkflowInstanceQueryRepository)
        {
            _staticicCommandRepository = statisticCommandRepository;
            _statisticQueryRepository = statisticQueryRepository;
            _cmmnWorkflowInstanceQueryRepository = cmmnWorkflowInstanceQueryRepository;
        }

        public async Task Handle(CMMNWorkflowInstanceCreatedEvent @event, CancellationToken cancellationToken)
        {
            var stat = await _statisticQueryRepository.FindById(@event.DefinitionId);
            if (stat == null)
            {
                _staticicCommandRepository.Add(new CMMNWorkflowDefinitionStatisticAggregate
                {
                    NbInstances = 1,
                    WorkflowDefinitionId  = @event.DefinitionId
                });
            }
            else
            {
                stat.NbInstances++;
                _staticicCommandRepository.Update(stat);
            }

            await _staticicCommandRepository.SaveChanges();
        }

        public async Task Handle(CMMNWorkflowElementCreatedEvent @event, CancellationToken cancellationToken)
        {            
            var workflowInstance = await _cmmnWorkflowInstanceQueryRepository.FindFlowInstanceById(@event.AggregateId);
            var stat = await _statisticQueryRepository.FindById(workflowInstance.WorkflowDefinitionId);
            var statElt = stat.Statistics.FirstOrDefault(s => s.ElementDefinitionId == @event.WorkflowElementDefinitionId);
            if (statElt == null)
            {
                statElt = new CMMNWorkflowElementDefinitionStatistic { ElementDefinitionId = @event.WorkflowElementDefinitionId, NbInstances = 1 };
                stat.Statistics.Add(statElt);
            }
            else
            {
                statElt.NbInstances++;
                _staticicCommandRepository.Update(stat);
            }

            await _staticicCommandRepository.SaveChanges();
        }
    }
}
