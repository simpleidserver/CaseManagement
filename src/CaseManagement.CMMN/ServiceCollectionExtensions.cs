using CaseManagement.CMMN;
using CaseManagement.CMMN.CaseFile;
using CaseManagement.CMMN.CaseFile.CommandHandlers;
using CaseManagement.CMMN.CaseFile.EventHandlers;
using CaseManagement.CMMN.CasePlan;
using CaseManagement.CMMN.CasePlanInstance;
using CaseManagement.CMMN.CasePlanInstance.CommandHandlers;
using CaseManagement.CMMN.CasePlanInstance.Processors;
using CaseManagement.CMMN.CasePlanInstance.Processors.Steps;
using CaseManagement.CMMN.CasePlanInstance.Repositories;
using CaseManagement.CMMN.CaseWorkerTask;
using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Domains.CasePlanInstance;
using CaseManagement.CMMN.Domains.Events;
using CaseManagement.CMMN.Infrastructures;
using CaseManagement.CMMN.Infrastructures.BackgroundTasks;
using CaseManagement.CMMN.Infrastructures.Bus;
using CaseManagement.CMMN.Infrastructures.DomainEvts;
using CaseManagement.CMMN.Infrastructures.EvtStore;
using CaseManagement.CMMN.Infrastructures.ExternalEvts;
using CaseManagement.CMMN.Infrastructures.Jobs;
using CaseManagement.CMMN.Infrastructures.Lock;
using CaseManagement.CMMN.Persistence;
using CaseManagement.CMMN.Persistence.InMemory;
using Microsoft.Extensions.Options;
using NEventStore;
using System;
using System.Collections.Concurrent;
using System.Linq;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static ServerBuilder AddCaseJobServer(this IServiceCollection services, Action<CMMNServerOptions> callback = null)
        {
            if (callback == null)
            {
                Configure<CMMNServerOptions>(services, (o) => { });
            }
            else
            {
                Configure<CMMNServerOptions>(services, callback);
            }

            var builder = new ServerBuilder(services);
            services.AddTransient<ICaseJobServer, CaseJobServer>();
            services.AddCommon()
                .AddBus()
                .AddServices()
                .AddMessageHandlers()
                .AddProcessors();
            return builder;
        }

        public static ServerBuilder AddCMMNApi(this IServiceCollection services)
        {
            var builder = new ServerBuilder(services);
            services.AddCommon()
                .AddServices()
                .AddCommandHandlers();
            return builder;
        }

        public static ServerBuilder AddCMMNApi(this IServiceCollection services, Action<CMMNServerOptions> serverOptions)
        {
            Configure(services, serverOptions);
            return services.AddCMMNApi();
        }

        private static IServiceCollection AddCommandHandlers(this IServiceCollection services)
        {
            services.TryAddTransient<IUpdateCaseFileCommandHandler, UpdateCaseFileCommandHandler>();
            services.TryAddTransient<IAddCaseFileCommandHandler, AddCaseFileCommandHandler>();
            services.TryAddTransient<IPublishCaseFileCommandHandler, PublishCaseFileCommandHandler>();
            services.TryAddTransient<IActivateCommandHandler, ActivateCommandHandler>();
            services.TryAddTransient<ICloseCommandHandler, CloseCommandHandler>();
            services.TryAddTransient<ICreateCaseInstanceCommandHandler, CreateCaseInstanceCommandHandler>();
            services.TryAddTransient<ILaunchCaseInstanceCommandHandler, LaunchCaseInstanceCommandHandler>();
            services.TryAddTransient<IReactivateCommandHandler, ReactivateCommandHandler>();
            services.TryAddTransient<IResumeCommandHandler, ResumeCommandHandler>();
            services.TryAddTransient<ISuspendCommandHandler, SuspendCommandHandler>();
            services.TryAddTransient<ITerminateCommandHandler, TerminateCommandHandler>();
            return services;
        }

        private static IServiceCollection AddCommon(this IServiceCollection services)
        {
            var wireup = Wireup.Init().UsingInMemoryPersistence().Build();
            services.TryAddSingleton<IStoreEvents>(wireup);
            services.TryAddSingleton<IMessageBroker, InMemoryMessageBroker>();
            services.TryAddTransient<ICommitAggregateHelper, CommitAggregateHelper>()
                .AddInMemoryPersistence();
            return services;
        }

        private static IServiceCollection AddBus(this IServiceCollection services)
        {
            services.TryAddSingleton<IDistributedLock, InMemoryDistributedLock>();
            services.TryAddSingleton<ISubscriberRepository, InMemorySubscriberRepository>();
            services.TryAddTransient<IJob, ProcessCasePlanInstanceJob>();
            services.TryAddTransient<IJob, ProcessDomainEventsJob>();
            services.TryAddTransient<IJob, ProcessExternalEventsJob>();
            return services;
        }

        private static IServiceCollection AddInMemoryPersistence(this IServiceCollection services)
        {
            var definitions = new ConcurrentBag<CasePlanAggregate>();
            var instances = new ConcurrentBag<CasePlanInstanceAggregate>();
            var files = new ConcurrentBag<CaseFileAggregate>();
            var caseWorkerTasks = new ConcurrentBag<CaseWorkerTaskAggregate>();
            services.TryAddSingleton<ICaseFileQueryRepository>(new InMemoryCaseFileQueryRepository(files));
            services.TryAddSingleton<ICasePlanCommandRepository>(new InMemoryCasePlanCommandRepository(definitions));
            services.TryAddSingleton<ICasePlanQueryRepository>(new InMemoryCasePlanQueryRepository(definitions));
            services.TryAddSingleton<ICasePlanInstanceQueryRepository>(new InMemoryCaseInstanceQueryRepository(instances));
            services.TryAddSingleton<ICasePlanInstanceCommandRepository>(new InMemoryCaseInstanceCommandRepository(instances));
            services.TryAddSingleton<ICaseWorkerTaskCommandRepository>(new InMemoryCaseWorkerTaskCommandRepository(caseWorkerTasks));
            services.TryAddSingleton<ICaseWorkerTaskQueryRepository>(new InMemoryCaseWorkerTaskQueryRepository(caseWorkerTasks));
            services.TryAddSingleton<ICaseFileCommandRepository>(new InMemoryCaseFileCommandRepository(files));
            services.TryAddSingleton<ICaseFileItemRepository, InMemoryDirectoryCaseFileItemRepository>();
            services.TryAddTransient<IEventStoreRepository, InMemoryEventStoreRepository>();
            services.TryAddSingleton<IAggregateSnapshotStore, InMemoryAggregateSnapshotStore>();
            return services;
        }

        private static IServiceCollection AddMessageHandlers(this IServiceCollection services)
        {
            services.TryAddTransient<IDomainEvtConsumerGeneric<CaseFileAddedEvent>, CaseFileHandler>();
            services.TryAddTransient<IDomainEvtConsumerGeneric<CaseFileUpdatedEvent>, CaseFileHandler>();
            services.TryAddTransient<IDomainEvtConsumerGeneric<CaseFilePublishedEvent>, CaseFileHandler>();
            return services;
        }

        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.TryAddTransient<ICaseWorkerTaskService, CaseWorkerTaskService>();
            services.TryAddTransient<ICasePlanInstanceService, CasePlanInstanceService>();
            services.TryAddTransient<ICaseFileService, CaseFileService>();
            services.TryAddTransient<ICasePlanService, CasePlanService>();
            return services;
        }

        private static IServiceCollection AddProcessors(this IServiceCollection services)
        {
            services.AddTransient<ICasePlanInstanceProcessor, CasePlanInstanceProcessor>();
            services.AddTransient<IProcessorFactory, ProcessorFactory>();
            services.AddTransient<BaseProcessor<StageElementInstance>, StageProcessor>();
            services.AddTransient<BaseProcessor<EmptyTaskElementInstance>, EmptyTaskProcessor>();
            services.AddTransient<BaseProcessor<HumanTaskElementInstance>, HumanTaskProcessor>();
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

        public static IServiceCollection TryUpdateSingleton<TService>(this IServiceCollection services, TService instance) where TService : class
        {
            var service = services.FirstOrDefault(s => s.ServiceType == typeof(TService));
            if (service != null)
            {
                services.Remove(service);
            }

            services.AddSingleton<TService>(instance);
            return services;
        }

        private static void Configure<T>(IServiceCollection services, Action<T> callback) where T : class, new()
        {
            services.TryAddSingleton<IOptions<T>, OptionsManager<T>>();
            services.TryAddScoped<IOptionsSnapshot<T>, OptionsManager<T>>();
            services.TryAddSingleton<IOptionsMonitor<T>, OptionsMonitor<T>>();
            services.TryAddTransient<IOptionsFactory<T>, OptionsFactory<T>>();
            services.TryAddSingleton<IConfigureOptions<T>>(new ConfigureNamedOptions<T>(Options.Options.DefaultName, callback));
            services.AddSingleton<IPostConfigureOptions<T>>(new PostConfigureOptions<T>(Options.Options.DefaultName, callback));
        }
    }
}