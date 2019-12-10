using CaseManagement.Workflow.Domains;
using CaseManagement.Workflow.Engine;
using CaseManagement.Workflow.Infrastructure;
using CaseManagement.Workflow.Infrastructure.Bus;
using CaseManagement.Workflow.Infrastructure.Bus.ConsumeDomainEvent;
using CaseManagement.Workflow.Infrastructure.Bus.InMemory;
using CaseManagement.Workflow.Infrastructure.Bus.StopProcess;
using CaseManagement.Workflow.Infrastructure.EvtStore;
using CaseManagement.Workflow.Infrastructure.EvtStore.InMemory;
using CaseManagement.Workflow.Infrastructure.Lock;
using CaseManagement.Workflow.Infrastructure.Lock.InMemory;
using CaseManagement.Workflow.Infrastructure.Scheduler;
using CaseManagement.Workflow.Infrastructure.Scheduler.InMemory;
using CaseManagement.Workflow.Persistence;
using CaseManagement.Workflow.Persistence.InMemory;
using NEventStore;
using System.Collections.Generic;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddWorkflow(this IServiceCollection services)
        {
            var processFlowInstances = new List<ProcessFlowInstance>();
            var forms = new List<FormAggregate>();
            var formInstances = new List<FormInstanceAggregate>();
            var roles = new List<RoleAggregate>();
            services.AddMvc();
            services.AddHostedService<BusHostedService>();
            services.AddTransient<IWorkflowEngine, WorkflowEngine>();
            services.AddSingleton<IProcessFlowInstanceQueryRepository>(new InMemoryProcessFlowInstanceQueryRepository(processFlowInstances));
            services.AddSingleton<IProcessFlowInstanceCommandRepository>(new InMemoryProcessFlowInstanceCommandRepository(processFlowInstances));
            services.AddSingleton<IFormQueryRepository>(new InMemoryFormQueryRepository(forms));
            services.AddSingleton<IFormCommandRepository>(new InMemoryFormCommandRepository(forms));
            services.AddSingleton<IFormInstanceCommandRepository>(new InMemoryFormInstanceCommandRepository(formInstances));
            services.AddSingleton<IFormInstanceQueryRepository>(new InMemoryFormInstanceQueryRepository(formInstances));
            services.AddSingleton<IRoleCommandRepository>(new InMemoryRoleCommandRepository(roles));
            services.AddSingleton<IRoleQueryRepository>(new InMemoryRoleQueryRepository(roles));
            services.AddNEventStore()
                .AddEventStores()
                .AddInfrastructure()
                .AddSnapshotStore()
                .AddBus()
                .AddScheduler()
                .AddLock();
            return services;
        }

        private static IServiceCollection AddScheduler(this IServiceCollection services)
        {
            services.AddSingleton<IScheduleJobStore, InMemoryScheduleJobStore>();
            services.AddTransient<ISchedulerHost, SchedulerHost>();
            services.AddHostedService<SchedulerHostedService>();
            return services;
        }

        private static IServiceCollection AddNEventStore(this IServiceCollection services)
        {
            var wireup = Wireup.Init().UsingInMemoryPersistence() .Build();
            services.AddSingleton(wireup);
            return services;
        }

        private static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddTransient<ICommitAggregateHelper, CommitAggregateHelper>();
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
            services.AddTransient<IMessageConsumer, DomainEventMessageConsumer>();
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
