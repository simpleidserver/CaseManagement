using CaseManagement.CMMN.Infrastructures.Bus.ConsumeDomainEvent;
using CaseManagement.CMMN.Infrastructures.Bus.LaunchProcess;
using CaseManagement.Workflow.Infrastructure.Bus.StopProcess;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace CaseManagement.Workflow.Infrastructure.Bus
{
    public static class QueueProviderExtensions
    {
        public static Task QueueEvent(this IQueueProvider queueProvider, DomainEvent domainEvent)
        {
            var message = new DomainEventMessage { AssemblyQualifiedName = domainEvent.GetType().AssemblyQualifiedName, Content = JsonConvert.SerializeObject(domainEvent) };
            return queueProvider.Queue(DomainEventMessageConsumer.QUEUE_NAME, JsonConvert.SerializeObject(message));
        }

        public static Task QueueLaunchProcess(this IQueueProvider queueProvider, string processId)
        {
            var message = new LaunchProcessMessage(processId);
            return queueProvider.Queue(CMMNLaunchProcessMessageConsumer.QUEUE_NAME, JsonConvert.SerializeObject(message));
        }

        public static Task QueueStopProcess(this IQueueProvider queueProvider, string processId)
        {
            var message = new StopProcessMessage(processId);
            return queueProvider.Queue(StopProcessMessageConsumer.QUEUE_NAME, JsonConvert.SerializeObject(message));
        }
    }
}
