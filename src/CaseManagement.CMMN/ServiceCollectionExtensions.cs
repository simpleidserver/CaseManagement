
using CaseManagement.CMMN;
using CaseManagement.CMMN.CaseInstance.Handlers;
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
            services.AddMvc();
            services.AddWorkflow()
                .AddInMemoryPersistence()
                .AddHandlers();
            return builder;
        }

        private static IServiceCollection AddInMemoryPersistence(this IServiceCollection services)
        {
            var definitions = new List<tDefinitions>();
            services.AddSingleton<ICMMNDefinitionsQueryRepository>(new InMemoryCMMNDefinitionsQueryRepository(definitions));
            return services;
        }

        private static IServiceCollection AddHandlers(this IServiceCollection services)
        {
            services.AddTransient<ILaunchCaseInstanceCommandHandler, LaunchCaseInstanceCommandHandler>();
            services.AddTransient<ICreateCaseInstanceCommandHandler, CreateCaseInstanceCommandHandler>();
            return services;
        }

        private static IServiceCollection AddProcessors(this IServiceCollection services)
        {
            services.AddTransient<IProcessFlowElementProcessor, CMMNPlanItemProcessor>();
            return services;
        }
    }
}
