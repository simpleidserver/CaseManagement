using CaseManagement.BPMN;
using CaseManagement.BPMN.Domains;
using CaseManagement.BPMN.Persistence;
using CaseManagement.BPMN.Persistence.InMemory;
using CaseManagement.BPMN.ProcessInstance.Processors;
using CaseManagement.Common;
using CaseManagement.Common.Bus;
using CaseManagement.Common.Domains;
using CaseManagement.Common.EvtStore;
using CaseManagement.Common.Jobs;
using CaseManagement.Common.Lock;
using CaseManagement.Common.Processors;
using NEventStore;
using System;
using System.Collections.Concurrent;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static void AddProcessJobServer(this IServiceCollection services, Action<CommonOptions> callbackOpts = null)
        {
            if (callbackOpts == null)
            {
                services.Configure<CommonOptions>((o) =>
                {
                    o.ApplicationAssembly = typeof(IProcessJobServer).Assembly;
                });
            }
            else
            {
                services.Configure(callbackOpts);
            }

            services.AddCommon()
                .AddProcessServerApplication();
        }

        private static IServiceCollection AddCommon(this IServiceCollection services)
        {
            var wireup = Wireup.Init().UsingInMemoryPersistence().Build();
            services.TryAddSingleton<IStoreEvents>(wireup);
            services.TryAddSingleton<IAggregateSnapshotStore, InMemoryAggregateSnapshotStore>();
            services.TryAddSingleton<IEventStoreRepository, InMemoryEventStoreRepository>();
            services.TryAddSingleton<IMessageBroker, InMemoryMessageBroker>();
            services.TryAddTransient<ICommitAggregateHelper, CommitAggregateHelper>();
            return services;
        }

        private static IServiceCollection AddProcessServerApplication(this IServiceCollection services)
        {
            var instances = new ConcurrentBag<ProcessInstanceAggregate>();
            services.TryAddSingleton<IDistributedLock, InMemoryDistributedLock>();
            services.TryAddTransient<IProcessJobServer, ProcessJobServer>();
            services.TryAddTransient<IProcessorFactory, ProcessorFactory>();
            services.TryAddTransient<IProcessInstanceProcessor, ProcessInstanceProcessor>();
            services.TryAddSingleton<IProcessInstanceQueryRepository>(new InMemoryProcessInstanceQueryRepository(instances));
            services.TryAddSingleton<IProcessInstanceCommandRepository>(new InMemoryProcessInstanceCommandRepository(instances));
            services.RegisterAllAssignableType<IJob>(typeof(IProcessJobServer).Assembly);
            services.RegisterAllAssignableType<IJob>(typeof(ICommitAggregateHelper).Assembly);
            services.RegisterAllAssignableType(typeof(IDomainEvtConsumerGeneric<>), typeof(IProcessJobServer).Assembly);
            services.RegisterAllAssignableType(typeof(IProcessor<,>), typeof(IProcessJobServer).Assembly);
            return services;
        }
    }
}
