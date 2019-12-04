using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace CaseManagement.Workflow.Infrastructure.Bus.StopProcess
{
    public class StopProcessMessageConsumer : BaseMessageConsumer
    {
        public StopProcessMessageConsumer(IRunningTaskPool taskPool, IQueueProvider queueProvider, IOptions<BusOptions> options) : base(taskPool, queueProvider, options)
        {
        }

        public override string QueueName => "stop-process";

        protected override Task<RunningTask> Execute(string queueMessage)
        {
            var message = JsonConvert.DeserializeObject<StopProcessMessage>(queueMessage);
            var runningTask = TaskPool.Get(message.ProcessFlowId);
            if (runningTask == null)
            {
                return Task.FromResult<RunningTask>(null);
            }

            runningTask.CancellationTokenSource.Cancel();
            return Task.FromResult<RunningTask>(null);
        }
    }
}
