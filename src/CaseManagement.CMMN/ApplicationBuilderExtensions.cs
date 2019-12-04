using Hangfire;

namespace Microsoft.AspNetCore.Builder
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseCMMN(this IApplicationBuilder appBuilder)
        {
            appBuilder.UseHangfireServer();
            appBuilder.UseMvc();
            return appBuilder;
        }
    }
}
