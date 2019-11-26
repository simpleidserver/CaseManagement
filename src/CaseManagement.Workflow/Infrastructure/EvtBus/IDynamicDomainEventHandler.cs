using System.Threading.Tasks;

namespace CaseManagement.Workflow.Infrastructure.EvtBus
{
    public interface IDynamicDomainEventHandler
    {
        Task Handle(dynamic eventData);
    }
}
