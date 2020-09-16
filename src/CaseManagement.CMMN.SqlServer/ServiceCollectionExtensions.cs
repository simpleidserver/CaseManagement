using CaseManagement.CMMN.Infrastructure.Lock;
using CaseManagement.CMMN.SqlServer;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDistributedLockSQLServer(this IServiceCollection services, Action<SqlDistributedLockOptions> callback)
        {
            services.TryAddTransient<IDistributedLock, SqlDistributedLock>();
            services.Configure(callback);
            return services;
        }
    }
}
