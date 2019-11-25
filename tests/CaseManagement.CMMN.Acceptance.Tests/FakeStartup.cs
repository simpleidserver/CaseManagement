using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.IO;

namespace CaseManagement.CMMN.Acceptance.Tests
{
    public class FakeStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            var builder = services.AddCMMN();
            builder.AddDefinitions(c =>
            {
                foreach (var file in Directory.EnumerateFiles(Path.Combine(Directory.GetCurrentDirectory(), "Cmmns"), "*.cmmn"))
                {
                    c.ImportDefinition(file);
                }
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseCMMN();
        }
    }
}
