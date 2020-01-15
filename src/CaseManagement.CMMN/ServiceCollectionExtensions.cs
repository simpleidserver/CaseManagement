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
using CaseManagement.CMMN.Infrastructures.Bus.ConfirmForm;
using CaseManagement.CMMN.Infrastructures.Bus.ConsumeDomainEvent;
using CaseManagement.CMMN.Infrastructures.Bus.ConsumeTransitionEvent;
using CaseManagement.CMMN.Infrastructures.Bus.LaunchProcess;
using CaseManagement.CMMN.Infrastructures.Bus.ReactivateProcess;
using CaseManagement.CMMN.Persistence;
using CaseManagement.CMMN.Persistence.InMemory;
using CaseManagement.Workflow.Infrastructure;
using CaseManagement.Workflow.Infrastructure.Bus;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static ServerBuilder AddCMMN(this IServiceCollection services)
        {
            var builder = new ServerBuilder(services);
            services.AddWorkflow()
                .AddInfrastructure()
                .AddInMemoryPersistence()
                .AddCommandHandlers()
                .AddEventHandlers()
                .AddProcessHandlers()
                .AddProcessors()
                .AddCaseFileItemRepositories()
                .AddBus();
            return builder;
        }

        public static ServerBuilder AddCMMN(this IServiceCollection services, Action<CMMNServerOptions> serverOptions)
        {
            services.Configure(serverOptions);
            return services.AddCMMN();
        }


        private static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddTransient<ICommitAggregateHelper, CommitAggregateHelper>();
            services.AddTransient<ICaseEngine, CaseEngine>();
            return services;
        }

        private static IServiceCollection AddBus(this IServiceCollection services)
        {
            services.AddTransient<IMessageConsumer, PerformanceMonitoringService>();
            services.AddTransient<IMessageConsumer, LaunchProcessMessageConsumer>();
            services.AddTransient<IMessageConsumer, ReactivateProcessMessageConsumer>();
            services.AddTransient<IMessageConsumer, DomainEventMessageConsumer>();
            services.AddTransient<IMessageConsumer, TransitionEventMessageConsumer>();
            services.AddTransient<IMessageConsumer, ConfirmFormMessageConsumer>();
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
            services.AddSingleton<IFormCommandRepository>(new InMemoryFormCommandRepository(forms));
            services.AddSingleton<IFormQueryRepository>(new InMemoryFormQueryRepository(forms));
            services.AddSingleton<ICaseFileQueryRepository>(new InMemoryCaseFileQueryRepository(files));
            services.AddSingleton<IFormInstanceCommandRepository>(new InMemoryFormInstanceCommandRepository(formInstances));
            services.AddSingleton<IFormInstanceQueryRepository>(new InMemoryFormInstanceQueryRepository(formInstances));
            services.AddSingleton<ICaseDefinitionCommandRepository>(new InMemoryCaseDefinitionCommandRepository(definitions, caseDefinitionHistories));
            services.AddSingleton<ICaseDefinitionQueryRepository>(new InMemoryCaseDefinitionQueryRepository(definitions, caseDefinitionHistories));
            services.AddSingleton<ICaseInstanceQueryRepository>(new InMemoryCaseInstanceQueryRepository(instances));
            services.AddSingleton<ICaseInstanceCommandRepository>(new InMemoryCaseInstanceCommandRepository(instances));
            services.AddSingleton<IProcessQueryRepository>(new InMemoryProcessQueryRepository(caseProcesses));
            services.AddSingleton<IActivationCommandRepository>(new InMemoryActivationCommandRepository(activations));
            services.AddSingleton<IActivationQueryRepository>(new InMemoryActivationQueryRepository(activations));
            services.AddSingleton<IRoleQueryRepository>(new InMemoryRoleQueryRepository(roles));
            services.AddSingleton<IRoleCommandRepository>(new InMemoryRoleCommandRepository(roles));
            services.AddSingleton<IStatisticCommandRepository>(new InMemoryStatisticCommandRepository(caseDailyStatistics, performances));
            services.AddSingleton<IStatisticQueryRepository>(new InMemoryStatisticQueryRepository(caseDailyStatistics, performances));
            return services;
        }

        private static IServiceCollection AddCommandHandlers(this IServiceCollection services)
        {
            services.AddTransient<ICloseCommandHandler, CloseCommandHandler>();
            services.AddTransient<IReactivateCommandHandler, ReactivateCommandHandler>();
            services.AddTransient<IResumeCommandHandler, ResumeCommandHandler>();
            services.AddTransient<ILaunchCaseInstanceCommandHandler, LaunchCaseInstanceCommandHandler>();
            services.AddTransient<ICreateCaseInstanceCommandHandler, CreateCaseInstanceCommandHandler>();
            services.AddTransient<IConfirmFormCommandHandler, ConfirmFormCommandHandler>();
            services.AddTransient<ICaseLaunchProcessCommandHandler, CaseLaunchProcessCommandHandler>();
            services.AddTransient<IStopCaseInstanceCommandHandler, StopCaseInstanceCommandHandler>();
            services.AddTransient<IActivateCommandHandler, ActivateCommandHandler>();
            services.AddTransient<ITerminateCommandHandler, TerminateCommandHandler>();
            services.AddTransient<ISuspendCommandHandler, SuspendCommandHandler>();
            return services;
        }

        private static IServiceCollection AddEventHandlers(this IServiceCollection services)
        {
            services.AddTransient<IDomainEventHandler<CaseInstanceCreatedEvent>, CaseDefinitionHistoryHandler>();
            services.AddTransient<IDomainEventHandler<CaseElementCreatedEvent>, CaseDefinitionHistoryHandler>();
            services.AddTransient<IDomainEventHandler<CaseElementTransitionRaisedEvent>, CaseActivationHandler>();
            services.AddTransient<IDomainEventHandler<CaseElementInstanceFormCreatedEvent>, FormInstanceHandler>();
            services.AddTransient<IDomainEventHandler<CaseElementInstanceFormSubmittedEvent>, FormInstanceHandler>();
            services.AddTransient<IDomainEventHandler<CaseElementTransitionRaisedEvent>, StatisticHandler>();
            services.AddTransient<IDomainEventHandler<CaseElementInstanceFormCreatedEvent>, StatisticHandler>();
            services.AddTransient<IDomainEventHandler<CaseElementInstanceFormSubmittedEvent>, StatisticHandler>();
            services.AddTransient<IDomainEventHandler<CaseTransitionRaisedEvent>, StatisticHandler>();
            services.AddTransient<IDomainEventHandler<CaseElementCreatedEvent>, WorkflowInstanceHandler>();
            services.AddTransient<IDomainEventHandler<CaseElementFinishedEvent>, WorkflowInstanceHandler>();
            services.AddTransient<IDomainEventHandler<CaseElementInstanceFormCreatedEvent>, WorkflowInstanceHandler>();
            services.AddTransient<IDomainEventHandler<CaseElementInstanceFormSubmittedEvent>, WorkflowInstanceHandler>();
            services.AddTransient<IDomainEventHandler<CaseElementStartedEvent>, WorkflowInstanceHandler>();
            services.AddTransient<IDomainEventHandler<CaseElementTransitionRaisedEvent>, WorkflowInstanceHandler>();
            services.AddTransient<IDomainEventHandler<CaseInstanceCreatedEvent>, WorkflowInstanceHandler>();
            services.AddTransient<IDomainEventHandler<CaseInstanceVariableAddedEvent>, WorkflowInstanceHandler>();
            services.AddTransient<IDomainEventHandler<CaseTransitionRaisedEvent>, WorkflowInstanceHandler>();
            return services;
        }

        private static IServiceCollection AddProcessors(this IServiceCollection services)
        {
            services.AddTransient<IProcessor, CMMNCaseFileItemProcessor>();
            services.AddTransient<IProcessor, CMMNHumanTaskProcessor>();
            services.AddTransient<IProcessor, CMMNProcessTaskProcessor>();
            services.AddTransient<IProcessor, CMMNMilestoneProcessor>();
            services.AddTransient<IProcessor, CMMNStageProcessor>();
            services.AddTransient<IProcessor, CMMNTaskProcessor>();
            services.AddTransient<IProcessor, CMMNTimerEventListenerProcessor>();
            services.AddTransient<ICaseFileItemListener, DirectoryCaseFileItemListener>();
            return services;
        }

        private static IServiceCollection AddCaseFileItemRepositories(this IServiceCollection services)
        {
            services.AddTransient<ICaseFileItemRepository, InMemoryDirectoryCaseFileItemRepository>();
            return services;
        }

        private static IServiceCollection AddProcessHandlers(this IServiceCollection services)
        {
            services.AddTransient<ICaseProcessHandler, CaseManagementCallbackProcessHandler>();
            return services;
        }
    }
}
