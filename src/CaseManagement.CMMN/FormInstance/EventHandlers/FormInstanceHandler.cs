using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Domains.FormInstance.Events;
using CaseManagement.CMMN.Infrastructures;
using CaseManagement.CMMN.Infrastructures.Bus;
using CaseManagement.CMMN.Persistence;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CasePlanInstance.EventHandlers
{
    public class FormInstanceHandler : IMessageBrokerConsumerGeneric<FormInstanceAddedEvent>, IMessageBrokerConsumerGeneric<FormInstanceSubmittedEvent>
    {
        private readonly IFormInstanceCommandRepository _formInstanceCommandRepository;
        private readonly IFormInstanceQueryRepository _formInstanceQueryRepository;

        public FormInstanceHandler(IFormInstanceCommandRepository formInstanceCommandRepository, IFormInstanceQueryRepository formInstanceQueryRepository)
        {
            _formInstanceCommandRepository = formInstanceCommandRepository;
            _formInstanceQueryRepository = formInstanceQueryRepository;
        }

        public string QueueName => CMMNConstants.QueueNames.FormInstances;

        public async Task Handle(FormInstanceAddedEvent @event, CancellationToken cancellationToken)
        {
            var formInstance = FormInstanceAggregate.New(new List<DomainEvent> { @event });
            _formInstanceCommandRepository.Add(formInstance);
            await _formInstanceCommandRepository.SaveChanges();
        }

        public async Task Handle(FormInstanceSubmittedEvent @event, CancellationToken cancellationToken)
        {
            var formInstance = await _formInstanceQueryRepository.FindById(@event.AggregateId);
            formInstance.Handle(@event);
            _formInstanceCommandRepository.Update(formInstance);
            await _formInstanceCommandRepository.SaveChanges();
        }
    }
}
