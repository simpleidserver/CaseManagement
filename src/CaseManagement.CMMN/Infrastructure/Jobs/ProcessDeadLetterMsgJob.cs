using CaseManagement.CMMN.Infrastructure.Bus;
using Microsoft.Extensions.Options;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Infrastructure.Jobs
{
    public class ProcessDeadLetterMsgJob : IJob
    {
        private readonly IMessageBroker _messageBroker;
        private CancellationTokenSource _cancellationTokenSource { get; set; }
        private Task _currentTask { get; set; }
        private CMMNServerOptions _options;

        public ProcessDeadLetterMsgJob(IMessageBroker messageBroker, IOptions<CMMNServerOptions> options)
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
                var deadLetters = await _messageBroker.DequeueDeadLetters(cancellationToken);
                foreach (var deadLetter in deadLetters)
                {
                    await _messageBroker.Queue(deadLetter.QueueName, deadLetter.Content, cancellationToken);
                }
            }
        }
    }
}
