using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.Workflow.Infrastructure
{
    public interface IDomainEventHandler<in TDomainEvent> : IDomainEventHandler where TDomainEvent : DomainEvent
    {
        Task Handle(TDomainEvent @event, CancellationToken cancellationToken);
    }

    public interface IDomainEventHandler
    {
    }
}
