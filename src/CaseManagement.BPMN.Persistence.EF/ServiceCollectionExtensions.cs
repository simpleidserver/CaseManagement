using CaseManagement.BPMN.Persistence;
using CaseManagement.BPMN.Persistence.EF;
using CaseManagement.BPMN.Persistence.EF.Persistence;
using Microsoft.EntityFrameworkCore;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBPMNStoreEF(this IServiceCollection services, Action<DbContextOptionsBuilder> optionsAction = null)
        {
            services.AddTransient<IProcessInstanceCommandRepository, ProcessInstanceCommandRepository>();
            services.AddTransient<IProcessInstanceQueryRepository, ProcessInstanceQueryRepository>();
            services.AddTransient<IProcessFileCommandRepository, ProcessFileCommandRepository>();
            services.AddTransient<IProcessFileQueryRepository, ProcessFileQueryRepository>();
            services.AddDbContext<BPMNDbContext>(optionsAction);
            return services;
        }
    }
}