using System;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.Workflow.Infrastructure.Scheduler
{
    public class SchedulerHost
    {
        private readonly IServiceProvider _serviceProvider;
        private CancellationTokenSource _cancellationTokenSource;
        private Task _currentTask;

        public SchedulerHost(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Task Start()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _currentTask = new Task(HandleTask, TaskCreationOptions.LongRunning);
            _currentTask.Start();
            return Task.CompletedTask;
        }

        public Task End()
        {
            _cancellationTokenSource.Cancel();
            return Task.CompletedTask;
        }

        private void HandleTask()
        {
            // 1. Fetch the next job from the repository.
            // 2. Get the assembly qualified name.
            // 3. Resolve the type and execute Handle.
            // JOB HANDLER.
        }
    }
}
