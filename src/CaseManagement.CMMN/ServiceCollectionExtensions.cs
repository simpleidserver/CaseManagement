using CaseManagement.CMMN;
using CaseManagement.CMMN.CaseInstance.CommandHandlers;
using CaseManagement.CMMN.CaseInstance.EventHandlers;
using CaseManagement.CMMN.CaseInstance.Processors;
using CaseManagement.CMMN.CaseInstance.Processors.CaseFileItem;
using CaseManagement.CMMN.CaseInstance.Repositories;
using CaseManagement.CMMN.CaseProcess.CommandHandlers;
using CaseManagement.CMMN.CaseProcess.ProcessHandlers;
using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Domains.CaseFile;
using CaseManagement.CMMN.Domains.Events;
using CaseManagement.CMMN.Infrastructures;
using CaseManagement.CMMN.Infrastructures.Bus;
using CaseManagement.CMMN.Infrastructures.Bus.ConfirmForm;
using CaseManagement.CMMN.Infrastructures.Bus.ConfirmTableItem;
using CaseManagement.CMMN.Infrastructures.Bus.ConsumeDomainEvent;
using CaseManagement.CMMN.Infrastructures.Bus.ConsumeTransitionEvent;
using CaseManagement.CMMN.Infrastructures.Bus.InMemory;
using CaseManagement.CMMN.Infrastructures.Bus.LaunchProcess;
using CaseManagement.CMMN.Infrastructures.Bus.ReactivateProcess;
using CaseManagement.CMMN.Infrastructures.EvtStore;
using CaseManagement.CMMN.Infrastructures.EvtStore.InMemory;
using CaseManagement.CMMN.Infrastructures.Lock;
using CaseManagement.CMMN.Infrastructures.Lock.InMemory;
using CaseManagement.CMMN.Persistence;
using CaseManagement.CMMN.Persistence.InMemory;
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
                .AddEventHandlers()
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
            services.TryAddTransient<IConfirmTableItemCommandHandler, ConfirmTableItemCommandHandler>();
            return services;
        }

        private static IServiceCollection AddCommon(this IServiceCollection services)
        {
            var wireup = Wireup.Init().UsingInMemoryPersistence().Build();
            services.TryAddSingleton<IStoreEvents>(wireup);
            services.TryAddSingleton<IRunningTaskPool>(new RunningTaskPool());
            services.TryAddSingleton<IQueueProvider>(new InMemoryQueueProvider());
            services.TryAddTransient<ICommitAggregateHelper, CommitAggregateHelper>()
                .AddInMemoryPersistence();
            return services;
        }

        private static IServiceCollection AddBus(this IServiceCollection services)
        {
            services.TryAddSingleton<IDistributedLock, InMemoryDistributedLock>();
            services.TryAddTransient<IMessageConsumer, PerformanceMonitoringService>();
            services.TryAddTransient<IMessageConsumer, LaunchProcessMessageConsumer>();
            services.TryAddTransient<IMessageConsumer, ReactivateProcessMessageConsumer>();
            services.TryAddTransient<IMessageConsumer, DomainEventMessageConsumer>();
            services.TryAddTransient<IMessageConsumer, TransitionEventMessageConsumer>();
            services.TryAddTransient<IMessageConsumer, ConfirmFormMessageConsumer>();
            services.TryAddTransient<IMessageConsumer, ConfirmTableItemMessageConsumer>();
            return services;
        }

        private static IServiceCollection AddInMemoryPersistence(this IServiceCollection services)
        {
            var definitions = new ConcurrentBag<CaseDefinition>();
            var caseProcesses = new List<ProcessAggregate>();
            var activations = new ConcurrentBag<CaseActivationAggregate>();
            var instances = new ConcurrentBag<CaseInstance>();
            var roles = new List<RoleAggregate>();
            var formInstances = new ConcurrentBag<FormInstanceAggregate>();
            var files = new List<CaseFileDefinitionAggregate>();
            var forms = new List<FormAggregate>();
            var caseDefinitionHistories = new ConcurrentBag<CaseDefinitionHistoryAggregate>();
            var caseDailyStatistics = new ConcurrentBag<DailyStatisticAggregate>();
            var performances = new ConcurrentBag<PerformanceStatisticAggregate>();
            services.TryAddSingleton<IFormCommandRepository>(new InMemoryFormCommandRepository(forms));
            services.TryAddSingleton<IFormQueryRepository>(new InMemoryFormQueryRepository(forms));
            services.TryAddSingleton<ICaseFileQueryRepository>(new InMemoryCaseFileQueryRepository(files));
            services.TryAddSingleton<IFormInstanceCommandRepository>(new InMemoryFormInstanceCommandRepository(formInstances));
            services.TryAddSingleton<IFormInstanceQueryRepository>(new InMemoryFormInstanceQueryRepository(formInstances));
            services.TryAddSingleton<ICaseDefinitionCommandRepository>(new InMemoryCaseDefinitionCommandRepository(definitions, caseDefinitionHistories));
            services.TryAddSingleton<ICaseDefinitionQueryRepository>(new InMemoryCaseDefinitionQueryRepository(definitions, caseDefinitionHistories));
            services.TryAddSingleton<ICaseInstanceQueryRepository>(new InMemoryCaseInstanceQueryRepository(instances));
            services.TryAddSingleton<ICaseInstanceCommandRepository>(new InMemoryCaseInstanceCommandRepository(instances));
            services.TryAddSingleton<IProcessQueryRepository>(new InMemoryProcessQueryRepository(caseProcesses));
            services.TryAddSingleton<IActivationCommandRepository>(new InMemoryActivationCommandRepository(activations));
            services.TryAddSingleton<IActivationQueryRepository>(new InMemoryActivationQueryRepository(activations));
            services.TryAddSingleton<IRoleQueryRepository>(new InMemoryRoleQueryRepository(roles));
            services.TryAddSingleton<IRoleCommandRepository>(new InMemoryRoleCommandRepository(roles));
            services.TryAddSingleton<IStatisticCommandRepository>(new InMemoryStatisticCommandRepository(caseDailyStatistics, performances));
            services.TryAddSingleton<IStatisticQueryRepository>(new InMemoryStatisticQueryRepository(caseDailyStatistics, performances));
            services.TryAddSingleton<ICaseFileItemRepository, InMemoryDirectoryCaseFileItemRepository>();
            services.TryAddTransient<IEventStoreRepository, InMemoryEventStoreRepository>();
            services.TryAddSingleton<IAggregateSnapshotStore, InMemoryAggregateSnapshotStore>();
            return services;
        }

        private static IServiceCollection AddEventHandlers(this IServiceCollection services)
        {
            services.TryAddTransient<IDomainEventHandler<CaseInstanceCreatedEvent>, CaseDefinitionHistoryHandler>();
            services.TryAddTransient<IDomainEventHandler<CaseElementCreatedEvent>, CaseDefinitionHistoryHandler>();
            services.TryAddTransient<IDomainEventHandler<CaseElementTransitionRaisedEvent>, CaseActivationHandler>();
            services.TryAddTransient<IDomainEventHandler<CaseElementInstanceFormCreatedEvent>, FormInstanceHandler>();
            services.TryAddTransient<IDomainEventHandler<CaseElementInstanceFormSubmittedEvent>, FormInstanceHandler>();
            services.TryAddTransient<IDomainEventHandler<CaseElementTransitionRaisedEvent>, StatisticHandler>();
            services.TryAddTransient<IDomainEventHandler<CaseElementInstanceFormCreatedEvent>, StatisticHandler>();
            services.TryAddTransient<IDomainEventHandler<CaseElementInstanceFormSubmittedEvent>, StatisticHandler>();
            services.TryAddTransient<IDomainEventHandler<CaseTransitionRaisedEvent>, StatisticHandler>();
            services.TryAddTransient<IDomainEventHandler<CaseElementCreatedEvent>, WorkflowInstanceHandler>();
            services.TryAddTransient<IDomainEventHandler<CaseElementFinishedEvent>, WorkflowInstanceHandler>();
            services.TryAddTransient<IDomainEventHandler<CaseElementInstanceFormCreatedEvent>, WorkflowInstanceHandler>();
            services.TryAddTransient<IDomainEventHandler<CaseElementInstanceFormSubmittedEvent>, WorkflowInstanceHandler>();
            services.TryAddTransient<IDomainEventHandler<CaseElementStartedEvent>, WorkflowInstanceHandler>();
            services.TryAddTransient<IDomainEventHandler<CaseElementTransitionRaisedEvent>, WorkflowInstanceHandler>();
            services.TryAddTransient<IDomainEventHandler<CaseInstanceCreatedEvent>, WorkflowInstanceHandler>();
            services.TryAddTransient<IDomainEventHandler<CaseInstanceVariableAddedEvent>, WorkflowInstanceHandler>();
            services.TryAddTransient<IDomainEventHandler<CaseTransitionRaisedEvent>, WorkflowInstanceHandler>();
            services.TryAddTransient<IDomainEventHandler<CaseElementPlanificationConfirmedEvent>, WorkflowInstanceHandler>();
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