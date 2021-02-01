using CaseManagement.HumanTask.Persistence;
using CaseManagement.HumanTask.Persistence.EF;
using CaseManagement.HumanTask.Persistence.EF.Persistence;
using Microsoft.EntityFrameworkCore;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddHumanTaskStoreEF(this IServiceCollection services, Action<DbContextOptionsBuilder> optionsAction = null)
        {
            services.AddTransient<IHumanTaskDefCommandRepository, HumanTaskDefCommandRepository>();
            services.AddTransient<IHumanTaskDefQueryRepository, HumanTaskDefQueryRepository>();
            services.AddTransient<IHumanTaskInstanceCommandRepository, HumanTaskInstanceCommandRepository>();
            services.AddTransient<IHumanTaskInstanceQueryRepository, HumanTaskInstanceQueryRepository>();
            services.AddTransient<INotificationInstanceCommandRepository, NotificationInstanceCommandRepository>();
            services.AddTransient<INotificationInstanceQueryRepository, NotificationInstanceQueryRepository>();
            services.AddTransient<INotificationDefCommandRepository, NotificationDefCommandRepository>();
            services.AddTransient<INotificationDefQueryRepository, NotificationDefQueryRepository>();
            services.AddDbContext<HumanTaskDBContext>(optionsAction);
            return services;
        }
    }
}
