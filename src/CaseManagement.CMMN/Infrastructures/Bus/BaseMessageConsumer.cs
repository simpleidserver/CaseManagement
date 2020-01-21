﻿using Microsoft.Extensions.Options;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Infrastructures.Bus
{
    public abstract class BaseMessageConsumer : IMessageConsumer
    {
        public BaseMessageConsumer(IRunningTaskPool taskPool, IQueueProvider queueProvider, IOptions<CMMNServerOptions> serverOptions)
        {
            TaskPool = taskPool;
            QueueProvider = queueProvider;
            ServerOptions = serverOptions.Value;
        }

        protected CancellationTokenSource CancellationTokenSource { get; private set; }
        protected Task CurrentTask { get; private set; }
        protected IRunningTaskPool TaskPool { get; private set; }
        protected IQueueProvider QueueProvider { get; private set; }
        public CMMNServerOptions ServerOptions { get; private set; }

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
                await Task.Delay(ServerOptions.BlockThreadMS);
                if (TaskPool.NbTasks() > ServerOptions.MaxConcurrentTask)
                {
                    continue;
                }

                var queueMessage = await QueueProvider.Peek(QueueName);
                if (queueMessage == null)
                {
                    continue;
                }

                Debug.WriteLine($"Received message : {queueMessage}, Number of tasks : {TaskPool.NbTasks()}");
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
