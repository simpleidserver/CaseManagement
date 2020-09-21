using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.Common.Domains
{
    public interface IDomainEvtConsumerGeneric<T> where T : DomainEvent
    {
        Task Handle(T message, CancellationToken token);
    }
}
