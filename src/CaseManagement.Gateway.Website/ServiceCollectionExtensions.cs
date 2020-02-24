using CaseManagement.Gateway.Website;
using CaseManagement.Gateway.Website.CaseFile.CommandHandlers;
using CaseManagement.Gateway.Website.CaseFile.QueryHandlers;
using CaseManagement.Gateway.Website.CaseFile.Services;
using CaseManagement.Gateway.Website.CasePlanInstance.CommandHandlers;
using CaseManagement.Gateway.Website.CasePlanInstance.QueryHandlers;
using CaseManagement.Gateway.Website.CasePlanInstance.Services;
using CaseManagement.Gateway.Website.CasePlans.CommandHandlers;
using CaseManagement.Gateway.Website.CasePlans.QueryHandlers;
using CaseManagement.Gateway.Website.CasePlans.Services;
using CaseManagement.Gateway.Website.CaseWorkerTask.Services;
using CaseManagement.Gateway.Website.FormInstance.Services;
using CaseManagement.Gateway.Website.Performance.QueryHandlers;
using CaseManagement.Gateway.Website.Performance.Services;
using CaseManagement.Gateway.Website.Statistic.QueryHandlers;
using CaseManagement.Gateway.Website.Statistic.Services;
using CaseManagement.Gateway.Website.Token;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Extensions.Http;
using Polly.Timeout;
using System;
using System.Linq;
using System.Net.Http;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddWebsiteGateway(this IServiceCollection services, Action<ServerOptions> callback)
        {
            Configure(services, callback);
            services.AddStatistic()
                .AddPerformance()
                .AddCaseFile()
                .AddCasePlan()
                .AddFormInstance()
                .AddCaseWorkerTask()
                .AddCasePlanInstance()
                .AddToken();
            return services;
        }

        public static IServiceCollection AddStatistic(this IServiceCollection services)
        {
            services.TryAddTransient<IGetStatisticQueryHandler, GetStatisticQueryHandler>();
            services.TryAddTransient<ISearchStatisticQueryHandler, SearchStatisticQueryHandler>();
            services.AddHttpClient<IStatisticService, StatisticService>()
                 .AddPolicyHandler(GetRetryPolicy())
                 .AddPolicyHandler(GetBreakerCircuitPolicy());
            return services;
        }

        public static IServiceCollection AddPerformance(this IServiceCollection services)
        {
            services.TryAddTransient<IGetPerformanceQueryHandler, GetPerformanceQueryHandler>();
            services.TryAddTransient<ISearchPerformanceQueryHandler, SearchPerformanceQueryHandler>();
            services.AddHttpClient<IPerformanceService, PerformanceService>()
                 .AddPolicyHandler(GetRetryPolicy())
                 .AddPolicyHandler(GetBreakerCircuitPolicy());
            return services;
        }

        public static IServiceCollection AddCaseFile(this IServiceCollection services)
        {
            services.TryAddTransient<IGetCaseFileQueryHandler, GetCaseFileQueryHandler>();
            services.TryAddTransient<IAddCaseFileCommandHandler, AddCaseFileCommandHandler>();
            services.TryAddTransient<ISearchMyLatestCaseFileQueryHandler, SearchMyLatestCaseFileQueryHandler>();
            services.TryAddTransient<IUpdateCaseFileCommandHandler, UpdateCaseFileCommandHandler>();
            services.TryAddTransient<ISearchCaseFileHistoryQueryHandler, SearchCaseFileHistoryQueryHandler>();
            services.TryAddTransient<IPublishCaseFileCommandHandler, PublishCaseFileCommandHandler>();
            services.AddHttpClient<ICaseFileService, CaseFileService>()
                .AddPolicyHandler(GetRetryPolicy())
                .AddPolicyHandler(GetBreakerCircuitPolicy());
            return services;
        }

        public static IServiceCollection AddCasePlan(this IServiceCollection services)
        {
            services.TryAddTransient<ISearchCasePlanHistoryQueryHandler, SearchCasePlanHistoryQueryHandler>();
            services.TryAddTransient<ILaunchCasePlanInstanceCommandHandler, LaunchCasePlanInstanceCommandHandler>();
            services.TryAddTransient<ICloseCasePlanInstanceCommandHandler, CloseCasePlanInstanceCommandHandler>();
            services.TryAddTransient<IReactivateCasePlanInstanceCommandHandler, ReactivateCasePlanInstanceCommandHandler>();
            services.TryAddTransient<IResumeCasePlanInstanceCommandHandler, ResumeCasePlanInstanceCommandHandler>();
            services.TryAddTransient<ISuspendCasePlanInstanceCommandHandler, SuspendCasePlanInstanceCommandHandler>();
            services.TryAddTransient<ITerminateCasePlanInstanceCommandHandler, TerminateCasePlanInstanceCommandHandler>();
            services.TryAddTransient<ISearchMyLatestCasePlanQueryHandler, SearchMyLatestCasePlanQueryHandler>();
            services.TryAddTransient<CaseManagement.Gateway.Website.CasePlans.QueryHandlers.ISearchCasePlanInstanceQueryHandler, CaseManagement.Gateway.Website.CasePlans.QueryHandlers.SearchCasePlanInstanceQueryHandler>();
            services.TryAddTransient<ISearchFormInstanceQueryHandler, SearchFormInstanceQueryHandler>();
            services.TryAddTransient<ISearchCaseWorkerTaskQueryHandler, SearchCaseWorkerTaskQueryHandler>();
            services.TryAddTransient<IGetCasePlanQueryHandler, GetCasePlanQueryHandler>();
            services.AddHttpClient<ICasePlanService, CasePlanService>()
                .AddPolicyHandler(GetRetryPolicy())
                .AddPolicyHandler(GetBreakerCircuitPolicy());
            return services;
        }

        public static IServiceCollection AddCasePlanInstance(this IServiceCollection services)
        {
            services.TryAddTransient<IEnableCasePlanElementInstanceCommandHandler, EnableCasePlanElementInstanceCommandHandler>();
            services.TryAddTransient<IGetCasePlanInstanceQueryHandler, GetCasePlanInstanceQueryHandler>();
            services.TryAddTransient<IGetAssignedCasePlanInstanceQueryHandler, GetAssignedCasePlanInstanceQueryHandler>();
            services.TryAddTransient<ISearchAssignedCasePlanInstanceQueryHandler, SearchAssignedCasePlanInstanceQueryHandler>();
            services.TryAddTransient<CaseManagement.Gateway.Website.CasePlanInstance.QueryHandlers.ISearchCasePlanInstanceQueryHandler, CaseManagement.Gateway.Website.CasePlanInstance.QueryHandlers.SearchCasePlanInstanceQueryHandler>();
            services.AddHttpClient<ICasePlanInstanceService, CasePlanInstanceService>()
                .AddPolicyHandler(GetRetryPolicy())
                .AddPolicyHandler(GetBreakerCircuitPolicy());
            return services;
        }

        public static IServiceCollection AddFormInstance(this IServiceCollection services)
        {
            services.AddHttpClient<IFormInstanceService, FormInstanceService>()
                .AddPolicyHandler(GetRetryPolicy())
                .AddPolicyHandler(GetBreakerCircuitPolicy());
            return services;
        }

        public static IServiceCollection AddCaseWorkerTask(this IServiceCollection services)
        {
            services.AddHttpClient<ICaseWorkerTaskService, CaseWorkerTaskService>()
                .AddPolicyHandler(GetRetryPolicy())
                .AddPolicyHandler(GetBreakerCircuitPolicy());
            return services;
        }

        public static IServiceCollection AddToken(this IServiceCollection services)
        {
            services.TryAddSingleton<ITokenStore, TokenStore>();
            services.AddHttpClient<ITokenService, TokenService>()
                .AddPolicyHandler(GetRetryPolicy())
                .AddPolicyHandler(GetBreakerCircuitPolicy());
            return services;
        }

        private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            var retry = HttpPolicyExtensions
                .HandleTransientHttpError()
                .Or<TimeoutRejectedException>()
                .WaitAndRetryAsync(2, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
            var timeout = Policy.TimeoutAsync(3);
            return retry.WrapAsync(timeout);
        }

        private static IAsyncPolicy<HttpResponseMessage> GetBreakerCircuitPolicy()
        {
            var breaker = HttpPolicyExtensions
                .HandleTransientHttpError()
                .Or<TimeoutRejectedException>()
                .CircuitBreakerAsync(2, TimeSpan.FromSeconds(5));
            return breaker;
        }

        private static void Configure<T>(IServiceCollection services, Action<T> callback) where T : class, new()
        {
            services.TryAddSingleton<IOptions<T>, OptionsManager<T>>();
            services.TryAddScoped<IOptionsSnapshot<T>, OptionsManager<T>>();
            services.TryAddSingleton<IOptionsMonitor<T>, OptionsMonitor<T>>();
            services.TryAddTransient<IOptionsFactory<T>, OptionsFactory<T>>();
            services.TryAddSingleton<IConfigureOptions<T>>(new ConfigureNamedOptions<T>(Options.Options.DefaultName, callback));
        }

        public static IServiceCollection TryAddSingleton<TService>(this IServiceCollection services, TService instance) where TService : class
        {
            var service = services.FirstOrDefault(s => s.ServiceType == typeof(TService));
            if (service != null)
            {
                return services;
            }

            services.AddSingleton<TService>(instance);
            return services;
        }

        public static IServiceCollection TryAddSingleton<TService, TImplementation>(this IServiceCollection services)
            where TService : class
            where TImplementation : class, TService
        {
            var service = services.FirstOrDefault(s => s.ServiceType == typeof(TService) && s.ImplementationType == typeof(TImplementation));
            if (service != null)
            {
                return services;
            }

            services.AddSingleton<TService, TImplementation>();
            return services;
        }

        public static IServiceCollection TryAddScoped<TService, TImplementation>(this IServiceCollection services)
            where TService : class
            where TImplementation : class, TService
        {
            var service = services.FirstOrDefault(s => s.ServiceType == typeof(TService) && s.ImplementationType == typeof(TImplementation));
            if (service != null)
            {
                return services;
            }

            services.AddScoped<TService, TImplementation>();
            return services;
        }

        public static IServiceCollection TryAddTransient<TService, TImplementation>(this IServiceCollection services)
            where TService : class
            where TImplementation : class, TService
        {
            var service = services.FirstOrDefault(s => s.ServiceType == typeof(TService) && s.ImplementationType == typeof(TImplementation));
            if (service != null)
            {
                return services;
            }

            services.AddTransient<TService, TImplementation>();
            return services;
        }
    }
}