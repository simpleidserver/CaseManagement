using CaseManagement.Common.Jobs;
using CaseManagement.Common.Jobs.Persistence;
using CaseManagement.Common.Lock;
using CaseManagement.HumanTask;
using CaseManagement.HumanTask.Authorization;
using CaseManagement.HumanTask.Domains;
using CaseManagement.HumanTask.Infrastructure.Jobs;
using CaseManagement.HumanTask.Localization;
using CaseManagement.HumanTask.Parser;
using CaseManagement.HumanTask.Persistence;
using CaseManagement.HumanTask.Persistence.InMemory;
using MediatR;
using System.Collections.Concurrent;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static ServerBuilder AddHumanTasksApi(this IServiceCollection services)
        {
            services.AddCommon()
                .AddHumanTaskApiApplication();
            return new ServerBuilder(services);
        }

        public static ServerBuilder AddHumanTaskServer(this IServiceCollection services)
        {
            services.AddCommon()
                .AddHumanTaskServerApplication();
            return new ServerBuilder(services);
        }

        private static IServiceCollection AddHumanTaskApiApplication(this IServiceCollection services)
        {
            var logicalPeopleGroup = new ConcurrentBag<LogicalPeopleGroup>();
            services.AddTransient<IParameterParser, ParameterParser>();
            services.AddTransient<IAuthorizationHelper, AuthorizationHelper>();
            services.AddTransient<ITranslationHelper, TranslationHelper>();
            services.AddMediatR(typeof(IHumanTaskServer));
            services.TryAddSingleton<ILogicalPeopleGroupStore>(new InMemoryLogicalPeopleGroupStore(logicalPeopleGroup));
            return services;
        }

        private static IServiceCollection AddHumanTaskServerApplication(this IServiceCollection services)
        {
            var lstScheduledJobst = new ConcurrentBag<ScheduleJob>();
            services.TryAddSingleton<IDistributedLock, InMemoryDistributedLock>();
            services.TryAddSingleton<IScheduledJobStore>(new InMemoryScheduledJobStore(lstScheduledJobst));
            services.TryAddTransient<IHumanTaskServer, HumanTaskServer>();
            services.RegisterAllAssignableType<IJob>(typeof(ProcessActivationTimerJob).Assembly);
            return services;
        }

        private static IServiceCollection AddCommon(this IServiceCollection services)
        {
            var humanTaskDefs = new ConcurrentBag<HumanTaskDefinitionAggregate>();
            var humanTaskInstances = new ConcurrentBag<HumanTaskInstanceAggregate>();
            services.TryAddSingleton<IHumanTaskDefCommandRepository>(new HumanTaskDefCommandRepository(humanTaskDefs));
            services.TryAddSingleton<IHumanTaskDefQueryRepository>(new HumanTaskDefQueryRepository(humanTaskDefs));
            services.TryAddSingleton<IHumanTaskInstanceCommandRepository>(new HumanTaskInstanceCommandRepository(humanTaskInstances));
            services.TryAddSingleton<IHumanTaskInstanceQueryRepository>(new HumanTaskInstanceQueryRepository(humanTaskInstances));
            return services;
        }
    }
}
