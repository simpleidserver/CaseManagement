using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Infrastructures
{
    public interface IDomainEventHandler<in TDomainEvent> : IDomainEventHandler where TDomainEvent : DomainEvent
    {
        Task Handle(TDomainEvent @event, CancellationToken cancellationToken);
    }

    public interface IDomainEventHandler
    {
    }
}
