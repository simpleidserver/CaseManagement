using CaseManagement.CMMN;
using CaseManagement.CMMN.AspNetCore;
using CaseManagement.CMMN.CaseProcess.CommandHandlers;
using CaseManagement.CMMN.Infrastructures;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static ServerBuilder AddCMMNApi(this IServiceCollection services)
        {
            var builder = new ServerBuilder(services);
            services.AddCommon()
                .AddCommandHandlers();
            return builder;
        }

        public static ServerBuilder AddCMMNApi(this IServiceCollection services, Action<CMMNServerOptions> serverOptions)
        {
            services.Configure(serverOptions);
            return services.AddCMMNApi();
        }

        public static ServerBuilder AddCMMNEngine(this IServiceCollection services)
        {
            var builder = new ServerBuilder(services);
            services.AddTransient<ICaseLaunchProcessCommandHandler, CaseLaunchProcessCommandHandler>();
            services.AddHostedService<BusHostedService>();
            services.AddTransient<ICaseEngine, CaseEngine>()
                .AddCommon()
                .AddEventHandlers()
                .AddProcessHandlers()
                .AddProcessors()
                .AddBus();
            return builder;
        }

        public static ServerBuilder AddCMMNEngine(this IServiceCollection services, Action<CMMNServerOptions> serverOptions)
        {
            services.Configure(serverOptions);
            return services.AddCMMNEngine();
        }
    }
}
