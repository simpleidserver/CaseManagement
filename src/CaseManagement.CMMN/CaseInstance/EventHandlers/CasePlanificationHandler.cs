using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Domains.Events;
using CaseManagement.CMMN.Infrastructures;
using CaseManagement.CMMN.Persistence;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CaseInstance.EventHandlers
{
    public class CasePlanificationHandler : IDomainEventHandler<CaseElementPlanificationConfirmedEvent>, IDomainEventHandler<CaseElementPlannedEvent>
    {
        private readonly ICasePlanificationQueryRepository _casePlanificationQueryRepository;
        private readonly ICasePlanificationCommandRepository _casePlanificationCommandRepository;
        private readonly ICaseInstanceQueryRepository _caseInstanceQueryRepository;
        private readonly ICaseDefinitionQueryRepository _caseDefinitionQueryRepository;

        public CasePlanificationHandler(ICasePlanificationQueryRepository casePlanificationQueryRepository, ICasePlanificationCommandRepository casePlanificationCommandRepository, ICaseInstanceQueryRepository caseInstanceQueryRepository, ICaseDefinitionQueryRepository caseDefinitionQueryRepository)
        {
            _casePlanificationQueryRepository = casePlanificationQueryRepository;
            _casePlanificationCommandRepository = casePlanificationCommandRepository;
            _caseInstanceQueryRepository = caseInstanceQueryRepository;
            _caseDefinitionQueryRepository = caseDefinitionQueryRepository;
        }

        public async Task Handle(CaseElementPlanificationConfirmedEvent @event, CancellationToken cancellationToken)
        {
            var record = await _casePlanificationQueryRepository.FindById(@event.AggregateId, @event.CaseElementDefinitionId);
            _casePlanificationCommandRepository.Delete(record);
            await _casePlanificationCommandRepository.SaveChanges();
        }

        public async Task Handle(CaseElementPlannedEvent @event, CancellationToken cancellationToken)
        {
            var caseInstance = await _caseInstanceQueryRepository.FindFlowInstanceById(@event.AggregateId);
            var caseDefinition = await _caseDefinitionQueryRepository.FindById(caseInstance.CaseDefinitionId);
            var record = new CasePlanificationAggregate
            {
                CaseElementId = @event.CaseElementDefinitionId,
                CaseInstanceId = @event.AggregateId,
                CreateDateTime = @event.CreateDateTime,
                UserRole = @event.UserRole,
                CaseName = caseDefinition.Name,
                CaseDescription = caseDefinition.Description,
                CaseElementName = caseDefinition.GetElement(@event.CaseElementDefinitionId).Name
            };
            _casePlanificationCommandRepository.Add(record);
            await _casePlanificationCommandRepository.SaveChanges();
        }
    }
}
