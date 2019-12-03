using CaseManagement.CMMN;
using CaseManagement.CMMN.CaseInstance.CommandHandlers;
using CaseManagement.CMMN.CaseInstance.EventHandlers;
using CaseManagement.CMMN.CaseInstance.Processors;
using CaseManagement.CMMN.CaseProcess.CommandHandlers;
using CaseManagement.CMMN.CaseProcess.ProcessHandlers;
using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Persistence;
using CaseManagement.CMMN.Persistence.InMemory;
using CaseManagement.Workflow.Engine;
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
                .AddProcessors();
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

        private static IServiceCollection AddInMemoryPersistence(this IServiceCollection services)
        {
            var definitions = new List<tDefinitions>();
            var caseProcesses = new List<ProcessAggregate>();
            services.AddSingleton<ICMMNDefinitionsQueryRepository>(new InMemoryCMMNDefinitionsQueryRepository(definitions));
            services.AddSingleton<IProcessQueryRepository>(new InMemoryProcessQueryRepository(caseProcesses));
            return services;
        }

        private static IServiceCollection AddCommandHandlers(this IServiceCollection services)
        {
            services.AddTransient<ILaunchCaseInstanceCommandHandler, LaunchCaseInstanceCommandHandler>();
            services.AddTransient<ICreateCaseInstanceCommandHandler, CreateCaseInstanceCommandHandler>();
            services.AddTransient<IConfirmFormCommandHandler, ConfirmFormCommandHandler>();
            services.AddTransient<ICaseLaunchProcessCommandHandler, CaseLaunchProcessCommandHandler>();
            return services;
        }

        private static IServiceCollection AddEventHandlers(this IServiceCollection services)
        {
            services.AddTransient<ProcessFlowInstanceCreatedEventHandler>();
            services.AddTransient<ProcessFlowInstanceLaunchedEventHandler>();
            services.AddTransient<ProcessFlowInstanceFormConfirmedEventHandler>();
            services.AddTransient<ProcessFlowElementLaunchedEventHandler>();
            return services;
        }

        private static IServiceCollection AddProcessors(this IServiceCollection services)
        {
            services.AddTransient<IProcessFlowElementProcessor, CMMNPlanItemProcessor>();
            services.AddTransient<ICMMNPlanItemDefinitionProcessor, CMMNHumanTaskProcessor>();
            services.AddTransient<ICMMNPlanItemDefinitionProcessor, CMMNProcessTaskProcessor>();
            services.AddTransient<ICMMNPlanItemDefinitionProcessor, CMMNTaskProcessor>();
            services.AddTransient<ICMMNPlanItemDefinitionProcessor, CMMNTimerEventListenerProcessor>();
            return services;
        }

        private static IServiceCollection AddProcessHandlers(this IServiceCollection services)
        {
            services.AddTransient<ICaseProcessHandler, CaseManagementCallbackProcessHandler>();
            return services;
        }
    }
}
