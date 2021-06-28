using CaseManagement.CMMN;
using CaseManagement.CMMN.CasePlanInstance.Processors;
using CaseManagement.CMMN.CasePlanInstance.Processors.FileItem;
using CaseManagement.CMMN.CasePlanInstance.Processors.Handlers;
using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.EventHandlers;
using CaseManagement.CMMN.Persistence;
using CaseManagement.CMMN.Persistence.InMemory;
using CaseManagement.Common.Factories;
using MassTransit;
using MassTransit.ExtensionsDependencyInjectionIntegration;
using MediatR;
using System;
using System.Collections.Concurrent;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static ServerBuilder AddCaseApi(this IServiceCollection services, Action<CMMNServerOptions> callback = null, Action<IServiceCollectionBusConfigurator> configureMassTransit = null)
        {
            if (callback == null)
            {
                services.Configure<CMMNServerOptions>((o) => { });
            }
            else
            {
                services.Configure(callback);
            }

            if (configureMassTransit == null)
            {
                var schedulerEdp = new Uri("queue:scheduler");
                services.AddMassTransit(x =>
                {
                    x.AddMessageScheduler(schedulerEdp);
                    x.AddConsumer<CaseElementOccuredEventConsumer>();
                    x.AddConsumer<CaseInstanceWorkerTaskAddedEventConsumer>();
                    x.AddConsumer<CaseInstanceWorkerTaskRemovedEventConsumer>();
                    x.AddConsumer<CasePlanInstanceCreatedEventConsumer>();
                    x.AddConsumer<CasePublishedEventConsumer>();
                    x.UsingInMemory((context, cfg) =>
                    {
                        cfg.UseInMemoryScheduler("scheduler");
                        cfg.ConfigureEndpoints(context);
                    });
                });
            }
            else
            {
                services.AddMassTransit(configureMassTransit);
            }

            var builder = new ServerBuilder(services);
            services.AddCaseApiApplication();
            return builder;
        }

        private static IServiceCollection AddCaseApiApplication(this IServiceCollection services)
        {
            var definitions = new ConcurrentBag<CasePlanAggregate>();
            var instances = new ConcurrentBag<CasePlanInstanceAggregate>();
            var files = new ConcurrentBag<CaseFileAggregate>();
            var caseWorkerTasks = new ConcurrentBag<CaseWorkerTaskAggregate>();
            services.AddMediatR(typeof(CMMNExecutionContext));
            services.TryAddTransient<IHttpClientFactory, HttpClientFactory>();
            services.TryAddSingleton<ICaseFileCommandRepository>(new InMemoryCaseFileCommandRepository(files));
            services.TryAddSingleton<ICaseFileQueryRepository>(new InMemoryCaseFileQueryRepository(files));
            services.TryAddSingleton<ICasePlanInstanceCommandRepository>(new InMemoryCaseInstanceCommandRepository(instances));
            services.TryAddSingleton<ICasePlanInstanceQueryRepository>(new InMemoryCaseInstanceQueryRepository(instances));
            services.TryAddSingleton<ICasePlanCommandRepository>(new InMemoryCasePlanCommandRepository(definitions));
            services.TryAddSingleton<ICasePlanQueryRepository>(new InMemoryCasePlanQueryRepository(definitions));
            services.TryAddSingleton<ICaseWorkerTaskCommandRepository>(new InMemoryCaseWorkerTaskCommandRepository(caseWorkerTasks));
            services.TryAddSingleton<ICaseWorkerTaskQueryRepository>(new InMemoryCaseWorkerTaskQueryRepository(caseWorkerTasks));
            services.TryAddSingleton<ISubscriberRepository>(new InMemorySubscriberRepository());
            services.TryAddTransient<ICasePlanInstanceProcessor, CasePlanInstanceProcessor>();
            services.TryAddTransient<ICMMNProcessorFactory, CMMNProcessorFactory>();
            services.RegisterAllAssignableType(typeof(ICaseFileItemStore), typeof(ICaseFileItemStore).Assembly);
            services.RegisterAllAssignableType(typeof(ICaseEltInstanceProcessor), typeof(ICaseEltInstanceProcessor).Assembly);
            services.RegisterAllAssignableType(typeof(IHumanTaskHandler), typeof(IHumanTaskHandler).Assembly);
            return services;
        }

    }
}