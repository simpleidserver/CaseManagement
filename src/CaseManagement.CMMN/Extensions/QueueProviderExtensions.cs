using CaseManagement.CMMN.Infrastructures.Bus.ConfirmForm;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace CaseManagement.Workflow.Infrastructure.Bus
{
    public static class QueueProviderExtensions
    {
        public static Task QueueConfirmForm(this IQueueProvider queueProvider, string processId, string elementId, JObject content)
        {
            var message = new ConfirmFormMessage(processId, elementId, content);
            return queueProvider.Queue(ConfirmFormConsumer.QUEUE_NAME, JsonConvert.SerializeObject(message));
        }
    }
}
