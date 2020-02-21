using CaseManagement.CMMN;
using CaseManagement.CMMN.CaseFile.CommandHandlers;
using CaseManagement.CMMN.CaseFile.EventHandlers;
using CaseManagement.CMMN.CasePlan.EventHandlers;
using CaseManagement.CMMN.CasePlanInstance.CommandHandlers;
using CaseManagement.CMMN.CasePlanInstance.EventHandlers;
using CaseManagement.CMMN.CasePlanInstance.Processors;
using CaseManagement.CMMN.CasePlanInstance.Processors.CaseFileItem;
using CaseManagement.CMMN.CasePlanInstance.Repositories;
using CaseManagement.CMMN.CaseProcess.CommandHandlers;
using CaseManagement.CMMN.CaseProcess.ProcessHandlers;
using CaseManagement.CMMN.CaseWorkerTask.EventHandlers;
using CaseManagement.CMMN.DailyStatistic.EventHandlers;
using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Domains.Events;
using CaseManagement.CMMN.Domains.FormInstance.Events;
using CaseManagement.CMMN.Infrastructures;
using CaseManagement.CMMN.Infrastructures.Bus;
using CaseManagement.CMMN.Infrastructures.Bus.InMemory;
using CaseManagement.CMMN.Infrastructures.Bus.LaunchCasePlanInstance;
using CaseManagement.CMMN.Infrastructures.Bus.ReactivateProcess;
using CaseManagement.CMMN.Infrastructures.Bus.Transition;
using CaseManagement.CMMN.Infrastructures.EvtStore;
using CaseManagement.CMMN.Infrastructures.EvtStore.InMemory;
using CaseManagement.CMMN.Infrastructures.Lock;
using CaseManagement.CMMN.Infrastructures.Lock.InMemory;
using CaseManagement.CMMN.Persistence;
using CaseManagement.CMMN.Persistence.InMemory;
using CaseManagement.CMMN.Roles.EventHandlers;
using Microsoft.Extensions.Options;
using NEventStore;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static ServerBuilder AddCMMNEngine(this IServiceCollection services)
        {
            var builder = new ServerBuilder(services);
            services.TryAddTransient<ICaseLaunchProcessCommandHandler, CaseLaunchProcessCommandHandler>();
            services.TryAddTransient<ICaseEngine, CaseEngine>()
                .AddCommon()
                .AddMessageHandlers()
                .AddProcessHandlers()
                .AddProcessors()
                .AddBus();
            return builder;
        }

        public static ServerBuilder AddCMMNEngine(this IServiceCollection services, Action<CMMNServerOptions> serverOptions)
        {
            Configure(services, serverOptions);
            return services.AddCMMNEngine();
        }

        public static ServerBuilder AddCMMNApi(this IServiceCollection services)
        {
            var builder = new ServerBuilder(services);
            services.AddCommon().AddCommandHandlers();
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
            services.TryAddTransient<IConfirmFormCommandHandler, ConfirmFormCommandHandler>();
            services.TryAddTransient<ICreateCaseInstanceCommandHandler, CreateCaseInstanceCommandHandler>();
            services.TryAddTransient<ILaunchCaseInstanceCommandHandler, LaunchCaseInstanceCommandHandler>();
            services.TryAddTransient<IReactivateCommandHandler, ReactivateCommandHandler>();
            services.TryAddTransient<IResumeCommandHandler, ResumeCommandHandler>();
            services.TryAddTransient<ISuspendCommandHandler, SuspendCommandHandler>();
            services.TryAddTransient<ITerminateCommandHandler, TerminateCommandHandler>();
            services.TryAddTransient<ICaseLaunchProcessCommandHandler, CaseLaunchProcessCommandHandler>();
            return services;
        }

        private static IServiceCollection AddCommon(this IServiceCollection services)
        {
            var wireup = Wireup.Init().UsingInMemoryPersistence().Build();
            services.TryAddSingleton<IStoreEvents>(wireup);
            services.TryAddSingleton<IRunningTaskPool>(new RunningTaskPool());
            services.TryAddSingleton<IMessageBroker, InMemoryMessageBroker>();
            services.TryAddTransient<ICommitAggregateHelper, CommitAggregateHelper>()
                .AddInMemoryPersistence();
            return services;
        }

        private static IServiceCollection AddBus(this IServiceCollection services)
        {
            services.TryAddSingleton<IDistributedLock, InMemoryDistributedLock>();
            services.TryAddTransient<IJob, PerformanceMonitoringJob>();
            services.TryAddTransient<IJob, InMemoryMessageBroker>();
            return services;
        }

        private static IServiceCollection AddInMemoryPersistence(this IServiceCollection services)
        {
            var definitions = new ConcurrentBag<CasePlanAggregate>();
            var caseProcesses = new List<ProcessAggregate>();
            var instances = new ConcurrentBag<CasePlanInstanceAggregate>();
            var roles = new List<RoleAggregate>();
            var formInstances = new ConcurrentBag<FormInstanceAggregate>();
            var files = new ConcurrentBag<CaseFileAggregate>();
            var forms = new List<FormAggregate>();
            var caseDailyStatistics = new ConcurrentBag<DailyStatisticAggregate>();
            var caseWorkerTasks = new ConcurrentBag<CaseWorkerTaskAggregate>();
            var performances = new ConcurrentBag<PerformanceAggregate>();
            services.TryAddSingleton<IFormCommandRepository>(new InMemoryFormCommandRepository(forms));
            services.TryAddSingleton<IFormQueryRepository>(new InMemoryFormQueryRepository(forms));
            services.TryAddSingleton<ICaseFileQueryRepository>(new InMemoryCaseFileQueryRepository(files));
            services.TryAddSingleton<IFormInstanceCommandRepository>(new InMemoryFormInstanceCommandRepository(formInstances));
            services.TryAddSingleton<IFormInstanceQueryRepository>(new InMemoryFormInstanceQueryRepository(formInstances));
            services.TryAddSingleton<ICasePlanCommandRepository>(new InMemoryCasePlanCommandRepository(definitions));
            services.TryAddSingleton<ICasePlanQueryRepository>(new InMemoryCasePlanQueryRepository(definitions));
            services.TryAddSingleton<ICasePlanInstanceQueryRepository>(new InMemoryCaseInstanceQueryRepository(instances));
            services.TryAddSingleton<ICasePlanInstanceCommandRepository>(new InMemoryCaseInstanceCommandRepository(instances));
            services.TryAddSingleton<IProcessQueryRepository>(new InMemoryProcessQueryRepository(caseProcesses));
            services.TryAddSingleton<ICaseWorkerTaskCommandRepository>(new InMemoryCaseWorkerTaskCommandRepository(caseWorkerTasks));
            services.TryAddSingleton<ICaseWorkerTaskQueryRepository>(new InMemoryCaseWorkerTaskQueryRepository(caseWorkerTasks));
            services.TryAddSingleton<IRoleQueryRepository>(new InMemoryRoleQueryRepository(roles));
            services.TryAddSingleton<IRoleCommandRepository>(new InMemoryRoleCommandRepository(roles));
            services.TryAddSingleton<IDailyStatisticCommandRepository>(new InMemoryDailyStatisticCommandRepository(caseDailyStatistics));
            services.TryAddSingleton<IStatisticQueryRepository>(new InMemoryDailyStatisticQueryRepository(caseDailyStatistics));
            services.TryAddSingleton<IPerformanceCommandRepository>(new InMemoryPerformanceCommandRepository(performances));
            services.TryAddSingleton<IPerformanceQueryRepository>(new InMemoryPerformanceQueryRepository(performances));
            services.TryAddSingleton<ICaseFileCommandRepository>(new InMemoryCaseFileCommandRepository(files));
            services.TryAddSingleton<ICaseFileItemRepository, InMemoryDirectoryCaseFileItemRepository>();
            services.TryAddTransient<IEventStoreRepository, InMemoryEventStoreRepository>();
            services.TryAddSingleton<IAggregateSnapshotStore, InMemoryAggregateSnapshotStore>();
            return services;
        }

        private static IServiceCollection AddMessageHandlers(this IServiceCollection services)
        {
            services.TryAddTransient<IMessageBrokerConsumerGeneric<CasePlanAddedEvent>, RoleEventHandler>();
            services.TryAddTransient<IMessageBrokerConsumerGeneric<TransitionCommand>, TransitionCommandHandler>();
            services.TryAddTransient<IMessageBrokerConsumerGeneric<ReactivateProcessCommand>, ReactivateProcessCommandHandler>();
            services.TryAddTransient<IMessageBrokerConsumerGeneric<LaunchCasePlanInstanceCommand>, LaunchCasePlanInstanceHandler>();
            services.TryAddTransient<IMessageBrokerConsumerGeneric<CaseFilePublishedEvent>, CasePlanEventHandler>();
            services.TryAddTransient<IMessageBrokerConsumerGeneric<CaseWorkerTaskAddedEvent>, CaseWorkerTaskHandler>();
            services.TryAddTransient<IMessageBrokerConsumerGeneric<CaseWorkerTaskConfirmedEvent>, CaseWorkerTaskHandler>();
            services.TryAddTransient<IMessageBrokerConsumerGeneric<CaseFileAddedEvent>, CaseFileHandler>();
            services.TryAddTransient<IMessageBrokerConsumerGeneric<CaseFileUpdatedEvent>, CaseFileHandler>();
            services.TryAddTransient<IMessageBrokerConsumerGeneric<CaseFilePublishedEvent>, CaseFileHandler>();
            services.TryAddTransient<IMessageBrokerConsumerGeneric<FormInstanceAddedEvent>, FormInstanceHandler>();
            services.TryAddTransient<IMessageBrokerConsumerGeneric<FormInstanceSubmittedEvent>, FormInstanceHandler>();
            services.TryAddTransient<IMessageBrokerConsumerGeneric<CaseElementTransitionRaisedEvent>, StatisticCasePlanInstanceHandler>();
            services.TryAddTransient<IMessageBrokerConsumerGeneric<FormInstanceAddedEvent>, StatisticFormInstanceHandler>();
            services.TryAddTransient<IMessageBrokerConsumerGeneric<FormInstanceSubmittedEvent>, StatisticFormInstanceHandler>();
            services.TryAddTransient<IMessageBrokerConsumerGeneric<CaseTransitionRaisedEvent>, StatisticCasePlanInstanceHandler>();
            services.TryAddTransient<IMessageBrokerConsumerGeneric<CaseElementCreatedEvent>, CasePlanInstanceHandler>();
            services.TryAddTransient<IMessageBrokerConsumerGeneric<CaseElementFinishedEvent>, CasePlanInstanceHandler>();
            services.TryAddTransient<IMessageBrokerConsumerGeneric<CaseElementStartedEvent>, CasePlanInstanceHandler>();
            services.TryAddTransient<IMessageBrokerConsumerGeneric<CaseElementTransitionRaisedEvent>, CasePlanInstanceHandler>();
            services.TryAddTransient<IMessageBrokerConsumerGeneric<CaseInstanceCreatedEvent>, CasePlanInstanceHandler>();
            services.TryAddTransient<IMessageBrokerConsumerGeneric<CaseInstanceVariableAddedEvent>, CasePlanInstanceHandler>();
            services.TryAddTransient<IMessageBrokerConsumerGeneric<CaseTransitionRaisedEvent>, CasePlanInstanceHandler>();
            return services;
        }

        private static IServiceCollection AddProcessors(this IServiceCollection services)
        {
            services.TryAddTransient<IProcessor, CMMNCaseFileItemProcessor>();
            services.TryAddTransient<IProcessor, CMMNHumanTaskProcessor>();
            services.TryAddTransient<IProcessor, CMMNProcessTaskProcessor>();
            services.TryAddTransient<IProcessor, CMMNMilestoneProcessor>();
            services.TryAddTransient<IProcessor, CMMNStageProcessor>();
            services.TryAddTransient<IProcessor, CMMNTaskProcessor>();
            services.TryAddTransient<IProcessor, CMMNTimerEventListenerProcessor>();
            services.TryAddTransient<ICaseFileItemListener, DirectoryCaseFileItemListener>();
            return services;
        }

        private static IServiceCollection AddProcessHandlers(this IServiceCollection services)
        {
            services.TryAddTransient<ICaseProcessHandler, CaseManagementCallbackProcessHandler>();
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
        }
    }
}