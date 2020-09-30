using CaseManagement.Common.Middlewares;

namespace Microsoft.AspNetCore.Builder
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseCulture(this IApplicationBuilder app)
        {
            return app.UseMiddleware<CultureMiddleware>();
        }
    }
}
