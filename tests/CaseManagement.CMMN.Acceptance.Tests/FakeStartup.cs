using CaseManagement.Workflow.Domains;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
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
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseCMMN();
        }
    }
}
