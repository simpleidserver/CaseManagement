using CaseManagement.Workflow.Domains;
using CaseManagement.Workflow.Engine;
using CaseManagement.Workflow.Infrastructure.EvtBus;
using CaseManagement.Workflow.Infrastructure.EvtBus.InMemory;
using CaseManagement.Workflow.Infrastructure.EvtStore;
using CaseManagement.Workflow.Infrastructure.EvtStore.InMemory;
using CaseManagement.Workflow.Infrastructure.Services;
using CaseManagement.Workflow.Persistence;
using CaseManagement.Workflow.Persistence.InMemory;
using NEventStore;
using System.Collections.Generic;
using static CaseManagement.Workflow.Infrastructure.EvtStore.InMemory.InMemoryAggregateSnapshotStore;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddWorkflow(this IServiceCollection services)
        {
            var processFlowInstances = new List<ProcessFlowInstance>();
            var forms = new List<Form>();
            services.AddMvc();
            services.AddHostedService<QueuedHostedService>();
            services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();
            services.AddTransient<IWorkflowEngine, WorkflowEngine>();
            services.AddTransient<IProcessFlowElementProcessorFactory, ProcessFlowElementProcessorFactory>();
            services.AddSingleton<IProcessFlowInstanceQueryRepository>(new InMemoryProcessFlowInstanceQueryRepository(processFlowInstances));
            services.AddSingleton<IProcessFlowInstanceCommandRepository>(new InMemoryProcessFlowInstanceCommandRepository(processFlowInstances));
            services.AddSingleton<IFormQueryRepository>(new InMemoryFormQueryRepository(forms));
            services.AddSingleton<IFormCommandRepository>(new InMemoryFormCommandRepository(forms));
            services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();
            services.AddSingleton<IEventBus, InMemoryEventBus>();
            services.AddNEventStore()
                .AddEventStores()
                .AddSnapshotStore();
            return services;
        }

        private static IServiceCollection AddNEventStore(this IServiceCollection services)
        {
            var wireup = Wireup.Init().UsingInMemoryPersistence() .Build();
            services.AddSingleton(wireup);
            return services;
        }

        private static IServiceCollection AddEventStores(this IServiceCollection services)
        {
            services.AddTransient<IEventStoreRepository<ProcessFlowInstance>, EventStoreRepository<ProcessFlowInstance>>();
            return services;
        }
        private static IServiceCollection AddSnapshotStore(this IServiceCollection services)
        {
            services.AddSingleton<IAggregateSnapshotStore<ProcessFlowInstance>, AggregateSnapshotStore<ProcessFlowInstance>>();
            return services;
        }
    }
}
