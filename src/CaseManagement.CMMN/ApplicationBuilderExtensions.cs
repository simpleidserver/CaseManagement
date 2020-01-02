namespace Microsoft.AspNetCore.Builder
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseCMMN(this IApplicationBuilder appBuilder)
        {
            appBuilder.UseMvc();
            return appBuilder;
        }
    }
}
