using CaseManagement.Workflow.Infrastructure.Bus;
using CaseManagement.Workflow.Infrastructure.Bus.InMemory;
using CaseManagement.Workflow.Infrastructure.Bus.StopProcess;
using CaseManagement.Workflow.Infrastructure.EvtStore;
using CaseManagement.Workflow.Infrastructure.EvtStore.InMemory;
using CaseManagement.Workflow.Infrastructure.Lock;
using CaseManagement.Workflow.Infrastructure.Lock.InMemory;
using NEventStore;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddWorkflow(this IServiceCollection services)
        {
            services.AddMvc();
            services.AddHostedService<BusHostedService>();
            services.AddNEventStore()
                .AddEventStores()
                .AddSnapshotStore()
                .AddBus()
                .AddLock();
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
            services.AddTransient<IEventStoreRepository, InMemoryEventStoreRepository>();
            return services;
        }

        private static IServiceCollection AddSnapshotStore(this IServiceCollection services)
        {
            services.AddSingleton<IAggregateSnapshotStore, InMemoryAggregateSnapshotStore>();
            return services;
        }

        private static IServiceCollection AddBus(this IServiceCollection services)
        {
            services.AddSingleton<IRunningTaskPool, RunningTaskPool>();
            services.AddSingleton<IQueueProvider, InMemoryQueueProvider>();
            services.AddTransient<IMessageConsumer, StopProcessMessageConsumer>();
            return services;
        }

        private static IServiceCollection AddLock(this IServiceCollection services)
        {
            services.AddSingleton<IDistributedLock, InMemoryDistributedLock>();
            return services;
        }
    }
}
