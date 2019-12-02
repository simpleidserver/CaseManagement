using CaseManagement.CMMN.Acceptance.Tests.Delegates;
using CaseManagement.CMMN.Acceptance.Tests.Delegates.CaseWithProcessTask;
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
                    AssemblyQualifiedName = typeof(CountBikesTaskDelegate).AssemblyQualifiedName
                },
                new CaseManagementProcessAggregate
                {
                    Id = "countCars",
                    AssemblyQualifiedName = typeof(CountCarsTaskDelegate).AssemblyQualifiedName
                },
                new CaseManagementProcessAggregate
                {
                    Id = "firstTestProcess",
                    AssemblyQualifiedName = typeof(SetVariableTaskDelegate).AssemblyQualifiedName
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
                            Type = FormElementTypes.TXT,
                            IsRequired = true
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
