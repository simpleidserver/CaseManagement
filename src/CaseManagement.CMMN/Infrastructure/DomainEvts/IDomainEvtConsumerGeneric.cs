using CaseManagement.CMMN.Domains;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Infrastructures.DomainEvts
{
    public interface IDomainEvtConsumerGeneric<T> where T : DomainEvent
    {
        Task Handle(T message, CancellationToken token);
    }
}
