using CaseManagement.CMMN;
using CaseManagement.CMMN.CaseInstance.CommandHandlers;
using CaseManagement.CMMN.CaseInstance.EventHandlers;
using CaseManagement.CMMN.CaseInstance.Processors;
using CaseManagement.CMMN.CaseInstance.Processors.CaseFileItem;
using CaseManagement.CMMN.CaseInstance.Repositories;
using CaseManagement.CMMN.CaseProcess.CommandHandlers;
using CaseManagement.CMMN.CaseProcess.ProcessHandlers;
using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Domains.Events;
using CaseManagement.CMMN.Infrastructures;
using CaseManagement.CMMN.Infrastructures.Bus.ConsumeCMMNTransitionEvent;
using CaseManagement.CMMN.Infrastructures.Bus.ConsumeDomainEvent;
using CaseManagement.CMMN.Infrastructures.Bus.LaunchProcess;
using CaseManagement.CMMN.Persistence;
using CaseManagement.CMMN.Persistence.InMemory;
using CaseManagement.Workflow.Infrastructure;
using CaseManagement.Workflow.Infrastructure.Bus;
using System.Collections.Generic;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static CMMNServerBuilder AddCMMN(this IServiceCollection services)
        {
            var builder = new CMMNServerBuilder(services);
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

        private static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddTransient<ICommitAggregateHelper, CommitAggregateHelper>();
            services.AddTransient<ICMMNWorkflowEngine, CMMNWorkflowEngine>();
            return services;
        }

        private static IServiceCollection AddBus(this IServiceCollection services)
        {
            services.AddTransient<IMessageConsumer, CMMNLaunchProcessMessageConsumer>();
            services.AddTransient<IMessageConsumer, ReactivateProcessMessageConsumer>();
            services.AddTransient<IMessageConsumer, DomainEventMessageConsumer>();
            services.AddTransient<IMessageConsumer, CMMNTransitionEventMessageConsumer>();
            return services;
        }

        private static IServiceCollection AddInMemoryPersistence(this IServiceCollection services)
        {
            var definitions = new List<CMMNWorkflowDefinition>();
            var caseProcesses = new List<ProcessAggregate>();
            var activations = new List<CaseActivationAggregate>();
            var instances = new List<CMMNWorkflowInstance>();
            services.AddSingleton<ICMMNWorkflowDefinitionQueryRepository>(new InMemoryCMMNWorkflowDefinitionQueryRepository(definitions));
            services.AddSingleton<ICMMNWorkflowInstanceQueryRepository>(new InMemoryCMMNWorkflowInstanceQueryRepository(instances));
            services.AddSingleton<ICMMNWorkflowInstanceCommandRepository>(new InMemoryCMMNWorkflowInstanceCommandRepository(instances));
            services.AddSingleton<IProcessQueryRepository>(new InMemoryProcessQueryRepository(caseProcesses));
            services.AddSingleton<ICMMNActivationCommandRepository>(new InMemoryCMMNActivationCommandRepository(activations));
            return services;
        }

        private static IServiceCollection AddCommandHandlers(this IServiceCollection services)
        {
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
            services.AddTransient<IDomainEventHandler<CMMNWorkflowElementCreatedEvent>, CMMNWorkflowElementCreatedEventHandler>();
            services.AddTransient<IDomainEventHandler<CMMNWorkflowElementFinishedEvent>, CMMNWorkflowElementFinishedEventHandler>();
            services.AddTransient<IDomainEventHandler<CMMNWorkflowElementInstanceFormCreatedEvent>, CMMNWorkflowElementInstanceFormCreatedEventHandler>();
            services.AddTransient<IDomainEventHandler<CMMNWorkflowElementInstanceFormSubmittedEvent>, CMMNWorkflowElementInstanceFormSubmittedEventHandler>();
            services.AddTransient<IDomainEventHandler<CMMNWorkflowElementStartedEvent>, CMMNWorkflowElementStartedEventHandler>();
            services.AddTransient<IDomainEventHandler<CMMNWorkflowElementTransitionRaisedEvent>, CMMNWorkflowElementTransitionRaisedEventHandler>();
            services.AddTransient<IDomainEventHandler<CMMNWorkflowInstanceCreatedEvent>, CMMNWorkflowInstanceCreatedEventHandler>();
            services.AddTransient<IDomainEventHandler<CMMNWorkflowInstanceVariableAddedEvent>, CMMNWorkflowInstanceVariableAddedEventHandler>();
            services.AddTransient<IDomainEventHandler<CMMNWorkflowTransitionRaisedEvent>, CMMNWorkflowTransitionRaisedEventHandler>();
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
