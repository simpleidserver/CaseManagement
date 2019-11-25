using CaseManagement.Workflow.Domains;
using CaseManagement.Workflow.Engine;
using CaseManagement.Workflow.Persistence;
using CaseManagement.Workflow.Persistence.InMemory;
using System.Collections.Generic;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddWorkflow(this IServiceCollection services)
        {
            var processFlowInstances = new List<ProcessFlowInstance>();
            services.AddTransient<IWorkflowEngine, WorkflowEngine>();
            services.AddTransient<IProcessFlowElementProcessorFactory, ProcessFlowElementProcessorFactory>();
            services.AddSingleton<IProcessFlowInstanceQueryRepository>(new InMemoryProcessFlowInstanceQueryRepository(processFlowInstances));
            services.AddSingleton<IProcessFlowInstanceCommandRepository>(new InMemoryProcessFlowInstanceCommandRepository(processFlowInstances));
            return services;
        }
    }
}
