using CaseManagement.CMMN;
using CaseManagement.CMMN.CasePlanInstance.Processors;
using CaseManagement.CMMN.CasePlanInstance.Processors.FileItem;
using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Infrastructure;
using CaseManagement.CMMN.Infrastructure.Bus;
using CaseManagement.CMMN.Infrastructure.EvtStore;
using CaseManagement.CMMN.Infrastructure.ExternalEvts;
using CaseManagement.CMMN.Infrastructure.Lock;
using CaseManagement.CMMN.Infrastructures.DomainEvts;
using CaseManagement.CMMN.Persistence;
using CaseManagement.CMMN.Persistence.InMemory;
using Microsoft.Extensions.Options;
using NEventStore;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static ServerBuilder AddCaseJobServer(this IServiceCollection services, Action<CMMNServerOptions> callback = null)
        {
            if (callback == null)
            {
                Configure<CMMNServerOptions>(services, (o) => { });
            }
            else
            {
                Configure(services, callback);
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
                Configure<CMMNServerOptions>(services, (o) => { });
            }
            else
            {
                Configure(services, callback);
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
            services.RegisterAllAssignableType<IJob>();
            services.RegisterAllAssignableType(typeof(IDomainEvtConsumerGeneric<>));
            services.RegisterAllAssignableType(typeof(IProcessor<>));
            services.RegisterAllAssignableType(typeof(ICaseFileItemStore));
            return services;
        }

        private static IServiceCollection AddCaseApiApplication(this IServiceCollection services)
        {
            return services;
        }

        #region Additional features

        public static IServiceCollection TryAddTransient<TService, TImplementation>(this IServiceCollection services) 
            where TService : class
            where TImplementation : class, TService
        {
            var service = services.FirstOrDefault(s => s.ServiceType == typeof(TService) && s.ImplementationType == typeof(TImplementation));  
            if (service != null)
            {
                return services;
            }

            services.AddTransient<TService, TImplementation>();
            return services;
        }

        public static IServiceCollection TryAddSingleton<TService, TImplementation>(this IServiceCollection services)
            where TService : class
            where TImplementation : class, TService
        {
            var service = services.FirstOrDefault(s => s.ServiceType == typeof(TService) && s.ImplementationType == typeof(TImplementation));
            if (service != null)
            {
                return services;
            }

            services.AddSingleton<TService, TImplementation>();
            return services;
        }

        public static IServiceCollection TryAddScoped<TService, TImplementation>(this IServiceCollection services)
            where TService : class
            where TImplementation : class, TService
        {
            var service = services.FirstOrDefault(s => s.ServiceType == typeof(TService) && s.ImplementationType == typeof(TImplementation));
            if (service != null)
            {
                return services;
            }

            services.AddScoped<TService, TImplementation>();
            return services;
        }

        public static IServiceCollection TryAddSingleton<TService>(this IServiceCollection services, TService instance) where TService : class
        {
            var service = services.FirstOrDefault(s => s.ServiceType == typeof(TService));
            if (service != null)
            {
                return services;
            }

            services.AddSingleton<TService>(instance);
            return services;
        }

        public static IServiceCollection TryUpdateSingleton<TService>(this IServiceCollection services, TService instance) where TService : class
        {
            var service = services.FirstOrDefault(s => s.ServiceType == typeof(TService));
            if (service != null)
            {
                services.Remove(service);
            }

            services.AddSingleton<TService>(instance);
            return services;
        }

        private static void Configure<T>(IServiceCollection services, Action<T> callback) where T : class, new()
        {
            services.TryAddSingleton<IOptions<T>, OptionsManager<T>>();
            services.TryAddScoped<IOptionsSnapshot<T>, OptionsManager<T>>();
            services.TryAddSingleton<IOptionsMonitor<T>, OptionsMonitor<T>>();
            services.TryAddTransient<IOptionsFactory<T>, OptionsFactory<T>>();
            services.TryAddSingleton<IConfigureOptions<T>>(new ConfigureNamedOptions<T>(Options.Options.DefaultName, callback));
            services.AddSingleton<IPostConfigureOptions<T>>(new PostConfigureOptions<T>(Options.Options.DefaultName, callback));
        }

        public static IServiceCollection RegisterAllAssignableType<T>(this IServiceCollection services)
        {
            return services.RegisterAllAssignableType(typeof(T));
        }

        public static IServiceCollection RegisterAllAssignableType(this IServiceCollection services, Type type)
        {
            var assm = typeof(ServiceCollectionExtensions).Assembly;
            var types = assm.GetTypes().Where(p => type.IsAssignableFrom(p) || IsAssignableToGenericType(p, type));
            var addTransientMethod = typeof(ServiceCollectionServiceExtensions).GetMethods().FirstOrDefault(m =>
                m.Name == "AddTransient" &&
                m.IsGenericMethod == true &&
                m.GetGenericArguments().Count() == 2);
            foreach (var t in types)
            {
                if (t.IsInterface || t.IsAbstract)
                {
                    continue;
                }

                if (type.IsGenericTypeDefinition)
                {
                    var genericArgs = GetGenericArgs(t, type);
                    foreach(var args in genericArgs)
                    {
                        var genericType = type.MakeGenericType(args);
                        var method = addTransientMethod.MakeGenericMethod(new[] { genericType, t });
                        method.Invoke(services, new[] { services });
                    }
                }
                else
                {
                    var method = addTransientMethod.MakeGenericMethod(new[] { type, t });
                    method.Invoke(services, new[] { services });
                }

            }

            return services;
        }

        private static bool IsAssignableToGenericType(Type givenType, Type genericType)
        {
            return GetGenericArgs(givenType, genericType).Any();
        }

        private static ICollection<Type[]> GetGenericArgs(Type givenType, Type genericType)
        {
            var result = new List<Type[]>();
            var interfaceTypes = givenType.GetInterfaces();
            foreach (var it in interfaceTypes)
            {
                if (it.IsGenericType && it.GetGenericTypeDefinition() == genericType)
                {
                    result.Add(it.GetGenericArguments());
                }
            }

            return result;
        }

        #endregion
    }
}