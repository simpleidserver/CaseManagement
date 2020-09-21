using CaseManagement.Common.Bus;
using Microsoft.Extensions.Options;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.Common.Jobs
{
    public class ProcessScheduleMsgJob : IJob
    {
        private readonly IMessageBroker _messageBroker;
        private CancellationTokenSource _cancellationTokenSource { get; set; }
        private Task _currentTask { get; set; }
        private CommonOptions _options;

        public ProcessScheduleMsgJob(IMessageBroker messageBroker, IOptions<CommonOptions> options)
        {
            _messageBroker = messageBroker;
            _options = options.Value;
        }

        public Task Start()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _currentTask = new Task(Handle, TaskCreationOptions.LongRunning);
            _currentTask.Start();
            return Task.CompletedTask;
        }

        public Task Stop()
        {
            _cancellationTokenSource.Cancel();
            return _currentTask;
        }

        private async void Handle()
        {
            var cancellationToken = _cancellationTokenSource.Token;
            while (!_cancellationTokenSource.IsCancellationRequested)
            {
                Thread.Sleep(_options.BlockThreadMS);
                var scheduleMessages = await _messageBroker.DequeueScheduledMessage(cancellationToken);
                foreach (var scheduleMessage in scheduleMessages)
                {
                    await _messageBroker.Queue(scheduleMessage.QueueName, scheduleMessage.SerializedContent, cancellationToken);
                }
            }
        }
    }
}
