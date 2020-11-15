using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.BPMN.AspNetCore
{
    public class BPMNJobServerHostedService : IHostedService
    {
        private readonly IProcessJobServer _processJobServer;

        public BPMNJobServerHostedService(IServiceScopeFactory serviceScopeFactory)
        {
            var scope = serviceScopeFactory.CreateScope();
            _processJobServer = scope.ServiceProvider.GetRequiredService<IProcessJobServer>();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _processJobServer.Start();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _processJobServer.Stop();
            return Task.CompletedTask;
        }
    }
}
