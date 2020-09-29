using CaseManagement.Common.Jobs.Persistence;
using CaseManagement.Common.Lock;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.Common.Jobs
{
    public abstract class BaseScheduledJob : IJob
    {
        public BaseScheduledJob(IDistributedLock distributedLock, IOptions<CommonOptions> options, ILogger<BaseScheduledJob> logger, IScheduledJobStore scheduledJobStore)
        {
            DistributedLock = distributedLock;
            Options = options.Value;
            Logger = logger;
            ScheduledJobStore = scheduledJobStore;
        }

        protected IDistributedLock DistributedLock { get; set; }
        protected CommonOptions Options { get; set; }
        protected ILogger<BaseScheduledJob> Logger { get; set; }
        protected IScheduledJobStore ScheduledJobStore { get; set; }
        protected CancellationTokenSource CancellationTokenSource { get; set; }
        protected Task CurrentTask { get; set; }
        protected DateTime? NextExecutionDateTime { get; set; }
        protected abstract string LockName { get; }

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
                if (NextExecutionDateTime != null && DateTime.UtcNow <= NextExecutionDateTime.Value)
                {
                    continue;
                }

                if (!await DistributedLock.TryAcquireLock(LockName, cancellationToken))
                {
                    continue;
                }

                try
                {
                    await Execute(cancellationToken);
                }
                catch(Exception ex)
                {
                    Logger.LogError(ex.ToString());
                }
                finally
                {
                    await DistributedLock.ReleaseLock(LockName, cancellationToken);
                    var schedulingResult = await ScheduledJobStore.TryGetNextScheduling(this.GetType().FullName, cancellationToken);
                    NextExecutionDateTime = schedulingResult.NextDateTime;
                }
            }
        }

        protected abstract Task Execute(CancellationToken token);
    }
}
