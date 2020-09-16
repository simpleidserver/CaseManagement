using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.AspNetCore
{
    public class CMMNJobServerHostedService : IHostedService
    {
        private readonly ICaseJobServer _caseJobServer;

        public CMMNJobServerHostedService(IServiceScopeFactory serviceScopeFactory)
        {
            var scope = serviceScopeFactory.CreateScope();
            _caseJobServer = scope.ServiceProvider.GetRequiredService<ICaseJobServer>();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _caseJobServer.Start();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _caseJobServer.Stop();
            return Task.CompletedTask;
        }
    }
}
