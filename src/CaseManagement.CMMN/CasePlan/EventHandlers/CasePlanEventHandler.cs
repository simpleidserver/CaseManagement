using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Domains.Events;
using CaseManagement.CMMN.Infrastructures;
using CaseManagement.CMMN.Infrastructures.Bus;
using CaseManagement.CMMN.Persistence;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CasePlan.EventHandlers
{
    public class CasePlanEventHandler : IMessageBrokerConsumerGeneric<CasePlanAddedEvent>
    {
        private readonly ICasePlanCommandRepository _casePlanCommandRepository;

        public CasePlanEventHandler(ICasePlanCommandRepository casePlanCommandRepository)
        {
            _casePlanCommandRepository = casePlanCommandRepository;
        }

        public string QueueName => CMMNConstants.QueueNames.CasePlans;

        public async Task Handle(CasePlanAddedEvent @event, CancellationToken cancellationToken)
        {
            var casePlan = CasePlanAggregate.New(new List<DomainEvent> { @event });
            _casePlanCommandRepository.Add(casePlan);
            await _casePlanCommandRepository.SaveChanges();
        }
    }
}
