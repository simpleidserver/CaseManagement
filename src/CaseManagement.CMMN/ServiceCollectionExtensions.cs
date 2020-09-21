using CaseManagement.CMMN;
using CaseManagement.CMMN.CasePlanInstance.Processors;
using CaseManagement.CMMN.CasePlanInstance.Processors.FileItem;
using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Infrastructure.ExternalEvts;
using CaseManagement.CMMN.Persistence;
using CaseManagement.CMMN.Persistence.InMemory;
using CaseManagement.Common;
using CaseManagement.Common.Bus;
using CaseManagement.Common.Domains;
using CaseManagement.Common.EvtStore;
using CaseManagement.Common.Jobs;
using CaseManagement.Common.Lock;
using CaseManagement.Common.Processors;
using MediatR;
using NEventStore;
using System;
using System.Collections.Concurrent;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static ServerBuilder AddCaseJobServer(this IServiceCollection services, Action<CMMNServerOptions> callback = null, Action<CommonOptions> callbackOpts = null)
        {
            if (callback == null)
            {
                services.Configure<CMMNServerOptions>((o) => { });
            }
            else
            {
                services.Configure(callback);
            }

            if (callbackOpts == null)
            {
                services.Configure<CommonOptions>((o) => 
                {
                    o.ApplicationAssembly = typeof(ICaseJobServer).Assembly;
                });
            }
            else
            {
                services.Configure(callbackOpts);
            }

            var builder = new ServerBuilder(services);
            services.AddCommon()
                .AddCaseJobServerApplication();
            return builder;
        }

        public static ServerBuilder AddCaseApi(this IServiceCollection services, Action<CMMNServerOptions> callback = null)
        {
            if (callback == null)
            {
                services.Configure<CMMNServerOptions>((o) => { });
            }
            else
            {
                services.Configure(callback);
            }

            var builder = new ServerBuilder(services);
            services.AddCommon()
                .AddCaseApiApplication();
            return builder;
        }

        private static IServiceCollection AddCommon(this IServiceCollection services)
        {
            var definitions = new ConcurrentBag<CasePlanAggregate>();
            var instances = new ConcurrentBag<CasePlanInstanceAggregate>();
            var files = new ConcurrentBag<CaseFileAggregate>();
            var caseWorkerTasks = new ConcurrentBag<CaseWorkerTaskAggregate>();
            var wireup = Wireup.Init().UsingInMemoryPersistence().Build();
            services.TryAddSingleton<IStoreEvents>(wireup);
            services.TryAddSingleton<IAggregateSnapshotStore, InMemoryAggregateSnapshotStore>();
            services.TryAddSingleton<IEventStoreRepository, InMemoryEventStoreRepository>();
            services.TryAddSingleton<IMessageBroker, InMemoryMessageBroker>();
            services.TryAddTransient<ICommitAggregateHelper, CommitAggregateHelper>();
            services.TryAddSingleton<ICaseFileCommandRepository>(new InMemoryCaseFileCommandRepository(files));
            services.TryAddSingleton<ICaseFileQueryRepository>(new InMemoryCaseFileQueryRepository(files));
            services.TryAddSingleton<ICasePlanInstanceCommandRepository>(new InMemoryCaseInstanceCommandRepository(instances));
            services.TryAddSingleton<ICasePlanInstanceQueryRepository>(new InMemoryCaseInstanceQueryRepository(instances));
            services.TryAddSingleton<ICasePlanCommandRepository>(new InMemoryCasePlanCommandRepository(definitions));
            services.TryAddSingleton<ICasePlanQueryRepository>(new InMemoryCasePlanQueryRepository(definitions));
            services.TryAddSingleton<ICaseWorkerTaskCommandRepository>(new InMemoryCaseWorkerTaskCommandRepository(caseWorkerTasks));
            services.TryAddSingleton<ICaseWorkerTaskQueryRepository>(new InMemoryCaseWorkerTaskQueryRepository(caseWorkerTasks));
            return services;
        }

        private static IServiceCollection AddCaseJobServerApplication(this IServiceCollection services)
        {
            services.TryAddSingleton<ISubscriberRepository, InMemorySubscriberRepository>();
            services.TryAddSingleton<IDistributedLock, InMemoryDistributedLock>();
            services.TryAddTransient<ICaseJobServer, CaseJobServer>();
            services.TryAddTransient<IProcessorFactory, ProcessorFactory>();
            services.TryAddTransient<ICasePlanInstanceProcessor, CasePlanInstanceProcessor>();
            services.RegisterAllAssignableType<IJob>(typeof(ICaseJobServer).Assembly);
            services.RegisterAllAssignableType<IJob>(typeof(ICommitAggregateHelper).Assembly);
            services.RegisterAllAssignableType(typeof(IDomainEvtConsumerGeneric<>), typeof(ICaseJobServer).Assembly);
            services.RegisterAllAssignableType(typeof(IProcessor<,>), typeof(ICaseJobServer).Assembly);
            services.RegisterAllAssignableType(typeof(ICaseFileItemStore), typeof(ICaseJobServer).Assembly);
            return services;
        }

        private static IServiceCollection AddCaseApiApplication(this IServiceCollection services)
        {
            services.AddMediatR(typeof(ICaseJobServer));
            return services;
        }
    }
}