using CaseManagement.CMMN.Infrastructures;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.AspNetCore
{
    public class BusHostedService : IHostedService
    {
        private readonly IEnumerable<IJob> _jobs;

        public BusHostedService(IEnumerable<IJob> jobs)
        {
            _jobs = jobs;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            foreach(var messageConsumer in _jobs)
            {
                messageConsumer.Start();
            }

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            foreach(var messageConsumer in _jobs)
            {
                messageConsumer.Stop();
            }

            return Task.CompletedTask;
        }
    }
}