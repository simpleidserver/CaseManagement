using CaseManagement.CMMN.Infrastructure.Bus;
using Microsoft.Extensions.Options;
using System;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Infrastructure.Jobs
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
            return CurrentTask;
        }

        private async void Handle()
        {
            var cancellationToken = CancellationTokenSource.Token;
            while (!CancellationTokenSource.IsCancellationRequested)
            {
                Thread.Sleep(Options.BlockThreadMS);
                T dequeue = null;
                try
                {
                    dequeue = await MessageBroker.Dequeue<T>(QueueName, cancellationToken);
                }
                catch(OperationCanceledException)
                {
                    continue;
                }

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
                        var elapsedTime = DateTime.UtcNow.AddMilliseconds(Options.DeadLetterTimeMS);
                        await MessageBroker.QueueDeadLetter(QueueName, dequeue, elapsedTime, cancellationToken);
                    }
                }
            }
        }
    }
}
