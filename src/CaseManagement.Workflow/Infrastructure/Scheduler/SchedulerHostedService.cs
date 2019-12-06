using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.Workflow.Infrastructure.Scheduler
{
    public class SchedulerHostedService : IHostedService
    {
        private readonly ISchedulerHost _schedulerHost;

        public SchedulerHostedService(ISchedulerHost schedulerHost)
        {
            _schedulerHost = schedulerHost;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _schedulerHost.Start();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _schedulerHost.Stop();
            return Task.CompletedTask;
        }
    }
}
