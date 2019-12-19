using CaseManagement.CMMN.Infrastructures.Bus.ConsumeDomainEvent;
using CaseManagement.Workflow.Infrastructure;
using CaseManagement.Workflow.Infrastructure.Bus;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Infrastructure.Bus
{
    public static class QueueProviderExtensions
    {
        public static Task Queue<T>(this IQueueProvider queueProvider, T domainEvent) where T : DomainEvent
        {
            var message = new DomainEventMessage
            {
                AssemblyQualifiedName = domainEvent.GetType().AssemblyQualifiedName,
                Content = JsonConvert.SerializeObject(domainEvent)
            };
            return queueProvider.Queue(DomainEventMessageConsumer.QUEUE_NAME, JsonConvert.SerializeObject(message));
        }
    }
}
