using CaseManagement.Workflow.Infrastructure.Bus.ConsumeDomainEvent;
using CaseManagement.Workflow.Infrastructure.Bus.LaunchProcess;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace CaseManagement.Workflow.Infrastructure.Bus
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

        public static Task QueueLaunchProcess(this IQueueProvider queueProvider, string processId)
        {
            var message = new LaunchProcessMessage(processId);
            return queueProvider.Queue(LaunchProcessMessageConsumer.QUEUE_NAME, JsonConvert.SerializeObject(message));
        }
    }
}
