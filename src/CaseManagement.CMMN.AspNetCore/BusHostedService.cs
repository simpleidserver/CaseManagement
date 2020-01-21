using CaseManagement.CMMN.Infrastructures.Bus;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.AspNetCore
{
    public class BusHostedService : IHostedService
    {
        private readonly IEnumerable<IMessageConsumer> _messageConsumers;

        public BusHostedService(IEnumerable<IMessageConsumer> messageConsumers)
        {
            _messageConsumers = messageConsumers;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            foreach(var messageConsumer in _messageConsumers)
            {
                messageConsumer.Start();
            }

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            foreach(var messageConsumer in _messageConsumers)
            {
                messageConsumer.Stop();
            }

            return Task.CompletedTask;
        }
    }
}