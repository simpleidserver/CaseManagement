using CaseManagement.CMMN.Infrastructure.ExternalEvts;
using CaseManagement.CMMN.Persistence;
using CaseManagement.CMMN.Persistence.EF;
using CaseManagement.CMMN.Persistence.EF.Persistence;
using CaseManagement.Common.Bus;
using Microsoft.EntityFrameworkCore;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCaseManagementEFStore(this IServiceCollection services, Action<DbContextOptionsBuilder> optionsAction)
        {
            services.TryAddTransient<ICaseFileCommandRepository, CaseFileCommandRepository>();
            services.TryAddTransient<ICaseFileQueryRepository, CaseFileQueryRepository>();
            services.TryAddTransient<ICasePlanCommandRepository, CasePlanCommandRepository>();
            services.TryAddTransient<ICasePlanInstanceCommandRepository, CasePlanInstanceCommandRepository>();
            services.TryAddTransient<ICasePlanInstanceQueryRepository, CasePlanInstanceQueryRepository>();
            services.TryAddTransient<ICasePlanQueryRepository, CasePlanQueryRepository>();
            services.TryAddTransient<ICaseWorkerTaskCommandRepository, CaseWorkerTaskCommandRepository>();
            services.TryAddTransient<ICaseWorkerTaskQueryRepository, CaseWorkerTaskQueryRepository>();
            services.TryAddTransient<ISubscriberRepository, SubscriberRepository>();
            services.AddDbContext<CaseManagementDbContext>(optionsAction);
            return services;
        }

        public static IServiceCollection AddCaseManagementEFMessageBroker(this IServiceCollection services)
        {
            services.TryAddTransient<IMessageBrokerStore, MessageBrokerStore>();
            services.TryAddTransient<IMessageBroker, PersistedMessageBroker>();
            return services;
        }
    }
}
