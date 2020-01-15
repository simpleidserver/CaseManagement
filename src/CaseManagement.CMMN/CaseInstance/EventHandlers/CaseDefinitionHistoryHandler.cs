using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Domains.Events;
using CaseManagement.CMMN.Persistence;
using CaseManagement.Workflow.Infrastructure;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CaseInstance.EventHandlers
{
    public class CaseDefinitionHistoryHandler : IDomainEventHandler<CaseInstanceCreatedEvent>, IDomainEventHandler<CaseElementCreatedEvent>
    {
        private readonly ICaseInstanceQueryRepository _caseInstanceQueryRepository;
        private readonly ICaseDefinitionQueryRepository _caseDefinitionQueryRepository;
        private readonly ICaseDefinitionCommandRepository _caseDefinitionCommandRepository;

        public CaseDefinitionHistoryHandler(ICaseInstanceQueryRepository caseInstanceQueryRepository, ICaseDefinitionQueryRepository caseDefinitionQueryRepository, ICaseDefinitionCommandRepository caseDefinitionCommandRepository)
        {
            _caseInstanceQueryRepository = caseInstanceQueryRepository;
            _caseDefinitionQueryRepository = caseDefinitionQueryRepository;
            _caseDefinitionCommandRepository = caseDefinitionCommandRepository;
        }

        public async Task Handle(CaseInstanceCreatedEvent @event, CancellationToken cancellationToken)
        {
            var stat = await _caseDefinitionQueryRepository.FindHistoryById(@event.CaseDefinitionId);
            if (stat == null)
            {
                _caseDefinitionCommandRepository.Add(new CaseDefinitionHistoryAggregate
                {
                    NbInstances = 1,
                    CaseDefinitionId = @event.CaseDefinitionId
                });
            }
            else
            {
                stat.NbInstances++;
                _caseDefinitionCommandRepository.Update(stat);
            }

            await _caseDefinitionCommandRepository.SaveChanges();
        }

        public async Task Handle(CaseElementCreatedEvent @event, CancellationToken cancellationToken)
        {
            var workflowInstance = await _caseInstanceQueryRepository.FindFlowInstanceById(@event.AggregateId);
            var stat = await _caseDefinitionQueryRepository.FindHistoryById(workflowInstance.CaseDefinitionId);
            var statElt = stat.Statistics.FirstOrDefault(s => s.CaseElementDefinitionId == @event.CaseElementDefinitionId);
            if (statElt == null)
            {
                statElt = new CaseElementDefinitionHistory { CaseElementDefinitionId = @event.CaseElementDefinitionId, NbInstances = 1 };
                stat.Statistics.Add(statElt);
            }
            else
            {
                statElt.NbInstances++;
                _caseDefinitionCommandRepository.Update(stat);
            }

            await _caseDefinitionCommandRepository.SaveChanges();
        }
    }
}
