using CaseManagement.CMMN;
using CaseManagement.CMMN.CaseInstance.CommandHandlers;
using CaseManagement.CMMN.CaseInstance.Processors;
using CaseManagement.CMMN.CaseInstance.Repositories;
using CaseManagement.CMMN.CaseInstance.Watchers;
using CaseManagement.CMMN.CaseProcess.CommandHandlers;
using CaseManagement.CMMN.CaseProcess.ProcessHandlers;
using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Domains.CaseInstance.Events;
using CaseManagement.CMMN.Infrastructures.Bus.LaunchProcess;
using CaseManagement.CMMN.Persistence;
using CaseManagement.CMMN.Persistence.InMemory;
using CaseManagement.Workflow.Infrastructure;
using CaseManagement.Workflow.Infrastructure.Bus;
using Hangfire;
using Hangfire.MemoryStorage;
using System.Collections.Generic;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static CMMNServerBuilder AddCMMN(this IServiceCollection services)
        {
            var builder = new CMMNServerBuilder(services);
            services.AddWorkflow()
                .AddInMemoryPersistence()
                .AddCommandHandlers()
                .AddEventHandlers()
                .AddProcessHandlers()
                .AddProcessors()
                .AddWatchers()
                .AddCaseFileItemRepositories()
                .AddBus();
            services.AddHangfire((act) =>
            {
                var inMemory = GlobalConfiguration.Configuration.UseMemoryStorage();
                act.UseSimpleAssemblyNameTypeSerializer()
                    .UseRecommendedSerializerSettings()
                    .UseStorage(inMemory);
            });
            services.AddHangfireServer();
            return builder;
        }

        public static IServiceCollection AddBus(this IServiceCollection services)
        {
            services.AddTransient<IMessageConsumer, CMMNLaunchProcessMessageConsumer>();
            return services;
        }

        private static IServiceCollection AddInMemoryPersistence(this IServiceCollection services)
        {
            var definitions = new List<tDefinitions>();
            var caseProcesses = new List<ProcessAggregate>();
            var activations = new List<CaseActivationAggregate>();
            services.AddSingleton<ICMMNDefinitionsQueryRepository>(new InMemoryCMMNDefinitionsQueryRepository(definitions));
            services.AddSingleton<IProcessQueryRepository>(new InMemoryProcessQueryRepository(caseProcesses));
            services.AddSingleton<ICMMNActivationCommandRepository>(new InMemoryCMMNActivationCommandRepository(activations));
            return services;
        }

        private static IServiceCollection AddCommandHandlers(this IServiceCollection services)
        {
            services.AddTransient<ILaunchCaseInstanceCommandHandler, LaunchCaseInstanceCommandHandler>();
            // services.AddTransient<ICreateCaseInstanceCommandHandler, CreateCaseInstanceCommandHandler>();
            services.AddTransient<IConfirmFormCommandHandler, ConfirmFormCommandHandler>();
            services.AddTransient<ICaseLaunchProcessCommandHandler, CaseLaunchProcessCommandHandler>();
            services.AddTransient<IStopCaseInstanceCommandHandler, StopCaseInstanceCommandHandler>();
            services.AddTransient<IActivateCommandHandler, ActivateCommandHandler>();
            services.AddTransient<ITerminateCommandHandler, TerminateCommandHandler>();
            return services;
        }

        private static IServiceCollection AddEventHandlers(this IServiceCollection services)
        {
            /*
            services.AddTransient<IDomainEventHandler<CMMNProcessInstanceCreatedEvent>, CMMNProcessInstanceCreatedEventHandler>();
            services.AddTransient<IDomainEventHandler<CMMNCaseFileItemCreatedEvent>, CMMNCaseFileItemCreatedEventHandler>();
            services.AddTransient<IDomainEventHandler<CMMNManualStartCreated>, CMMNManualStartCreatedHandler>();
            */
            return services;
        }

        private static IServiceCollection AddProcessors(this IServiceCollection services)
        {
            /*
            services.AddTransient<IProcessFlowElementProcessor, CMMNHumanTaskProcessor>();
            services.AddTransient<IProcessFlowElementProcessor, CMMNProcessTaskProcessor>();
            services.AddTransient<IProcessFlowElementProcessor, CMMNTaskProcessor>();
            services.AddTransient<IProcessFlowElementProcessor, CMMNTimerEventListenerProcessor>();
            services.AddTransient<IProcessFlowElementProcessor, CMMNMilestoneProcessor>();
            services.AddTransient<IProcessFlowElementProcessor, CMMNCaseFileItemProcessor>();
            services.AddTransient<IProcessorHelper, ProcessorHelper>();
            */
            return services;
        }

        private static IServiceCollection AddCaseFileItemRepositories(this IServiceCollection services)
        {
            // services.AddTransient<ICaseFileItemRepository, DirectoryCaseFileItemRepository>();
            services.AddTransient<ICaseFileItemRepositoryFactory, CaseFileItemRepositoryFactory>();
            return services;
        }

        private static IServiceCollection AddWatchers(this IServiceCollection services)
        {
            /*
            services.AddTransient<ITimerEventWatcher, TimerEventWatcher>();
            services.AddTransient<IDomainEventWatcher, DomainEventWatcher>();
            */
            return services;
        }

        private static IServiceCollection AddProcessHandlers(this IServiceCollection services)
        {
            services.AddTransient<ICaseProcessHandler, CaseManagementCallbackProcessHandler>();
            return services;
        }
    }
}
