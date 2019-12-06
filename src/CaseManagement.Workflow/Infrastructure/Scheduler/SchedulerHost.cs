using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.Workflow.Infrastructure.Scheduler
{
    public class SchedulerHost : ISchedulerHost
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IScheduleJobStore _scheduleJobStore;
        private CancellationTokenSource _cancellationTokenSource;
        private Task _currentTask;

        public SchedulerHost(IServiceProvider serviceProvider, IScheduleJobStore scheduleJobStore)
        {
            _serviceProvider = serviceProvider;
            _scheduleJobStore = scheduleJobStore;
        }

        public Task Start()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _currentTask = new Task(HandleTask, TaskCreationOptions.LongRunning);
            _currentTask.Start();
            return Task.CompletedTask;
        }

        public Task Stop()
        {
            _cancellationTokenSource.Cancel();
            return Task.CompletedTask;
        }

        private async void HandleTask()
        {
            while (!_cancellationTokenSource.IsCancellationRequested)
            {
                var job = await _scheduleJobStore.TakeNextJob();
                if (job == null)
                {
                    continue;
                }

                var handlerMessageType = Type.GetType(job.AssemblyQualifiedName);
                var handlerMessage = JsonConvert.DeserializeObject(job.Message, handlerMessageType);
                var concreteType = typeof(IScheduleJobHandler<>).MakeGenericType(handlerMessageType);
                var messageHandler = _serviceProvider.GetService(concreteType);
                concreteType.GetMethod("Handle").Invoke(messageHandler, new object[] { handlerMessage, _cancellationTokenSource.Token });
            }
        }
    }
}
