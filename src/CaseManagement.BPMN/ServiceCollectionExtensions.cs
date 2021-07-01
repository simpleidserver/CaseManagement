using CaseManagement.BPMN;
using CaseManagement.BPMN.Domains;
using CaseManagement.BPMN.EventHandlers;
using CaseManagement.BPMN.Persistence;
using CaseManagement.BPMN.Persistence.InMemory;
using CaseManagement.BPMN.ProcessInstance.Processors;
using CaseManagement.BPMN.ProcessInstance.Processors.Activities.Handlers;
using CaseManagement.Common;
using CaseManagement.Common.Factories;
using CaseManagement.Common.Processors;
using MassTransit;
using MassTransit.ExtensionsDependencyInjectionIntegration;
using MediatR;
using System;
using System.Collections.Concurrent;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static ServerBuilder AddProcessJobServer(
            this IServiceCollection services,
            Action<CommonOptions> callbackOpts = null,
            Action<BPMNServerOptions> callbackServerOpts = null,
            Action<IServiceCollectionBusConfigurator> configureMassTransit = null)
        {
            if (callbackOpts == null)
            {
                services.Configure<CommonOptions>((o) =>
                {
                    o.ApplicationAssembly = typeof(ProcessInstanceAggregate).Assembly;
                });
            }
            else
            {
                services.Configure(callbackOpts);
            }

            if (callbackServerOpts == null)
            {
                services.Configure<BPMNServerOptions>((o) => { });
            }
            else
            {
                services.Configure(callbackServerOpts);
            }

            if (configureMassTransit == null)
            {
                var schedulerEdp = new Uri("queue:scheduler");
                services.AddMassTransit(x =>
                {
                    x.AddMessageScheduler(schedulerEdp);
                    x.AddConsumer<ProcessInstanceRestartedEventConsumer>();
                    x.UsingInMemory((context, cfg) =>
                    {
                        cfg.ConfigureEndpoints(context);
                    });
                });
            }
            else
            {
                services.AddMassTransit(configureMassTransit);
            }

            services.AddCommon()
                .AddProcessServerApplication()
                .AddProcessApiApplication();
            return new ServerBuilder(services);
        }

        private static IServiceCollection AddProcessApiApplication(this IServiceCollection services)
        {
            services.AddMediatR(typeof(ProcessInstanceAggregate));
            var processFiles = new ConcurrentBag<ProcessFileAggregate>();
            services.AddSingleton<IProcessFileCommandRepository>(new InMemoryProcessFileCommandRepository(processFiles));
            services.AddSingleton<IProcessFileQueryRepository>(new InMemoryProcessFileQueryRepository(processFiles));
            return services;
        }

        private static IServiceCollection AddProcessServerApplication(this IServiceCollection services)
        {
            var instances = new ConcurrentBag<ProcessInstanceAggregate>();
            services.TryAddTransient<IProcessorFactory, ProcessorFactory>();
            services.TryAddTransient<IProcessInstanceProcessor, ProcessInstanceProcessor>();
            services.TryAddSingleton<IProcessInstanceQueryRepository>(new InMemoryProcessInstanceQueryRepository(instances));
            services.TryAddSingleton<IProcessInstanceCommandRepository>(new InMemoryProcessInstanceCommandRepository(instances));
            services.RegisterAllAssignableType(typeof(IProcessor<,,>), typeof(BPMNConstants).Assembly);
            services.RegisterAllAssignableType(typeof(IServiceTaskHandler), typeof(IServiceTaskHandler).Assembly);
            services.RegisterAllAssignableType(typeof(IUserServerTaskHandler), typeof(IUserServerTaskHandler).Assembly);
            foreach(var assm in AppDomain.CurrentDomain.GetAssemblies())
            {
                services.RegisterAllAssignableType(typeof(IDelegateHandler), assm, true);
            }

            return services;
        }

        private static IServiceCollection AddCommon(this IServiceCollection services)
        {
            services.TryAddTransient<IHttpClientFactory, HttpClientFactory>();
            return services;
        }
    }
}
