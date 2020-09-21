using CaseManagement.Common.Lock;
using CaseManagement.Common.SqlServer;
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
