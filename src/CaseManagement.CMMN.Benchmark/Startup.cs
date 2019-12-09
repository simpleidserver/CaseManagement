using CaseManagement.Workflow.Domains;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IO;

namespace CaseManagement.CMMN.Benchmark
{
    public class Startup
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
            })
            .AddForms(new List<Form>
            {
                new Form
                {
                    Id = "createMeetingForm",
                    Elements = new List<FormElement>
                    {
                        new FormElement
                        {
                            Id = "name",
                            Type = FormElementTypes.TXT
                        }
                    }
                }
            });
            services.AddLogging(b =>
            {
                b.AddFilter("Hangfire", LogLevel.Error);
                b.AddFilter("Microsoft", LogLevel.Error);
                b.AddFilter("System", LogLevel.Error);
                b.AddFilter("CaseManagement", LogLevel.Error);   
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseCMMN();
        }
    }
}
