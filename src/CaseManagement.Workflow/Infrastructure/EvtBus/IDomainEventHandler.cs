using System.Threading.Tasks;

namespace CaseManagement.Workflow.Infrastructure.EvtBus
{
    public interface IDomainEventHandler<in TDomainEvent> : IDomainEventHandler where TDomainEvent : DomainEvent
    {
        Task Handle(TDomainEvent @event);
    }

    public interface IDomainEventHandler
    {
    }
}
