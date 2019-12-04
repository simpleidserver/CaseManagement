using Microsoft.Extensions.Options;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.Workflow.Infrastructure.Bus
{
    public abstract class BaseMessageConsumer : IMessageConsumer
    {
        public BaseMessageConsumer(IRunningTaskPool taskPool, IQueueProvider queueProvider, IOptions<BusOptions> options)
        {
            TaskPool = taskPool;
            QueueProvider = queueProvider;
            Options = options.Value;
        }

        protected CancellationTokenSource CancellationTokenSource { get; private set; }
        protected Task CurrentTask { get; private set; }
        protected IRunningTaskPool TaskPool { get; private set; }
        protected BusOptions Options;
        protected IQueueProvider QueueProvider { get; private set; }

        public void Start()
        {
            CancellationTokenSource = new CancellationTokenSource();
            CurrentTask = new Task(Handle, TaskCreationOptions.LongRunning);
            CurrentTask.Start();
        }

        public void Stop()
        {
            CancellationTokenSource.Cancel();
            CurrentTask.Wait();
        }

        public virtual async void Handle()
        {
            var token = CancellationTokenSource.Token;
            while (!token.IsCancellationRequested)
            {
                if (TaskPool.NbTasks() > Options.MaxConcurrentTask)
                {
                    await Task.Delay(Options.IdleTimeInMs);
                    continue;
                }

                var queueMessage = await QueueProvider.Dequeue(QueueName);
                if (queueMessage == null)
                {
                    continue;
                }

                var task = await Execute(queueMessage);
                if (task == null)
                {
                    continue;
                }

                TaskPool.AddTask(task);
                task.Task.Start();
            }

            await Task.WhenAll(TaskPool.Tasks);
        }

        protected abstract Task<RunningTask> Execute(string queueMessage);

        public abstract string QueueName { get; }
    }
}
