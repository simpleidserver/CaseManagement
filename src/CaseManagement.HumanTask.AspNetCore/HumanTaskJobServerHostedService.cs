using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.HumanTask.AspNetCore
{
    public class HumanTaskJobServerHostedService : IHostedService
    {
        private readonly IHumanTaskServer _humanTaskServer;

        public HumanTaskJobServerHostedService(IServiceScopeFactory serviceScopeFactory)
        {
            var scope = serviceScopeFactory.CreateScope();
            _humanTaskServer = scope.ServiceProvider.GetRequiredService<IHumanTaskServer>();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _humanTaskServer.Start();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _humanTaskServer.Stop();
            return Task.CompletedTask;
        }
    }
}
