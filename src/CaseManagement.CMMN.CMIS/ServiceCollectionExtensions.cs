using CaseManagement.CMMN.CaseInstance.Repositories;
using CaseManagement.CMMN.CMIS;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCMIS(this IServiceCollection services)
        {
            // services.AddTransient<ICaseFileItemRepository, CMISDirectoryCaseFileItemRepository>();
            services.AddSingleton<ICMISSessionFactory, CMISSessionFactory>();
            return services;
        }
    }
}
