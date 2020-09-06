using CaseManagement.CMMN.Infrastructures.Bus;
using CaseManagement.CMMN.Infrastructures.Jobs.Notifications;
using Microsoft.Extensions.Options;
using System;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Infrastructures.Jobs
{
    public abstract class BaseJob<T> : IJob where T : BaseNotification
    {
        public BaseJob(IMessageBroker messageBroker, IOptions<CMMNServerOptions> options)
        {
            MessageBroker = messageBroker;
            Options = options.Value;
        }

        protected CancellationTokenSource CancellationTokenSource { get; set; }
        protected Task CurrentTask { get; set; }
        protected IMessageBroker MessageBroker;
        protected CMMNServerOptions Options { get; private set; }

        protected abstract string QueueName { get; }

        protected abstract Task ProcessMessage(T message, CancellationToken cancellationToken);

        public Task Start()
        {
            CancellationTokenSource = new CancellationTokenSource();
            CurrentTask = new Task(Handle, TaskCreationOptions.LongRunning);
            CurrentTask.Start();
            return Task.CompletedTask;
        }

        public Task Stop()
        {
            CancellationTokenSource.Cancel();
            CurrentTask.Wait();
            return Task.CompletedTask;
        }

        private async void Handle()
        {
            var cancellationToken = CancellationTokenSource.Token;
            while (!CancellationTokenSource.IsCancellationRequested)
            {
                Thread.Sleep(Options.BlockThreadMS);
                var dequeue = await MessageBroker.Dequeue<T>(QueueName, cancellationToken);
                if (dequeue == null)
                {
                    continue;
                }

                try
                {
                    await ProcessMessage(dequeue, cancellationToken);
                }
                catch
                {
                    dequeue.Increment();
                    if (dequeue.NbRetry < Options.NbRetry)
                    {
                        await MessageBroker.Queue(QueueName, dequeue);
                    }
                }
            }
        }
    }
}
