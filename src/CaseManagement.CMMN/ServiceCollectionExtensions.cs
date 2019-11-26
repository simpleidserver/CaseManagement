
using CaseManagement.CMMN;
using CaseManagement.CMMN.CaseInstance.CommandHandlers;
using CaseManagement.CMMN.CaseInstance.EventHandlers;
using CaseManagement.CMMN.CaseInstance.Processors;
using CaseManagement.CMMN.Persistence;
using CaseManagement.CMMN.Persistence.InMemory;
using CaseManagement.Workflow.Engine;
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
                .AddProcessors();
            return builder;
        }

        private static IServiceCollection AddInMemoryPersistence(this IServiceCollection services)
        {
            var definitions = new List<tDefinitions>();
            services.AddSingleton<ICMMNDefinitionsQueryRepository>(new InMemoryCMMNDefinitionsQueryRepository(definitions));
            return services;
        }

        private static IServiceCollection AddCommandHandlers(this IServiceCollection services)
        {
            services.AddTransient<ILaunchCaseInstanceCommandHandler, LaunchCaseInstanceCommandHandler>();
            services.AddTransient<ICreateCaseInstanceCommandHandler, CreateCaseInstanceCommandHandler>();
            services.AddTransient<IConfirmFormCommandHandler, ConfirmFormCommandHandler>();
            return services;
        }

        private static IServiceCollection AddEventHandlers(this IServiceCollection services)
        {
            services.AddTransient<ProcessFlowInstanceCreatedEventHandler>();
            services.AddTransient<ProcessFlowInstanceLaunchedEventHandler>();
            services.AddTransient<ProcessFlowInstanceFormConfirmedEventHandler>();
            return services;
        }

        private static IServiceCollection AddProcessors(this IServiceCollection services)
        {
            services.AddTransient<IProcessFlowElementProcessor, CMMNPlanItemProcessor>();
            return services;
        }
    }
}
