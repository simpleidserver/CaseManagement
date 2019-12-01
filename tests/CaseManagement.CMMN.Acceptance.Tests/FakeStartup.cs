using CaseManagement.CMMN.Domains;
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
            .AddCaseProcesses(new List<ProcessAggregate>
            {
                new CaseManagementProcessAggregate
                {
                    Id = "countBikes",
                    AssemblyQualifiedName = "CaseManagement.CMMN.Acceptance.Tests.Delegates.CountBikesTaskDelegate, CaseManagement.CMMN.Acceptance.Tests, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
                },
                new CaseManagementProcessAggregate
                {
                    Id = "countCars",
                    AssemblyQualifiedName = "CaseManagement.CMMN.Acceptance.Tests.Delegates.CountCarsTaskDelegate, CaseManagement.CMMN.Acceptance.Tests, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
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
