using CaseManagement.CMMN.Acceptance.Tests.Delegates;
using CaseManagement.CMMN.Acceptance.Tests.Middlewares;
using CaseManagement.CMMN.Domains;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CaseManagement.CMMN.Acceptance.Tests
{
    public class FakeStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCustomAuthentication(opts => { });
            services.AddAuthorization(policy =>
            {
                policy.AddPolicy("IsConnected", p => p.RequireAuthenticatedUser());
            });
            var builder = services.AddCMMN();
            var files = Directory.EnumerateFiles(Path.Combine(Directory.GetCurrentDirectory(), "Cmmns"), "*.cmmn").ToList();
            builder.AddDefinitions(files)
                .AddCaseProcesses(new List<ProcessAggregate>
                {
                    new CaseManagementProcessAggregate
                    {
                        Id = "longtask",
                        AssemblyQualifiedName = typeof(LongTask).AssemblyQualifiedName
                    },
                    new CaseManagementProcessAggregate
                    {
                        Id = "failtask",
                        AssemblyQualifiedName = typeof(FailTask).AssemblyQualifiedName
                    }
                });
            /*
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
                },
                new CaseManagementProcessAggregate
                {
                    Id = "wait",
                    AssemblyQualifiedName = typeof(WaitTaskDelegate).AssemblyQualifiedName
                },
                new CaseManagementProcessAggregate
                {
                    Id = "sendEmail",
                    AssemblyQualifiedName = typeof(SendEmailTaskDelegate).AssemblyQualifiedName
                },
                new CaseManagementProcessAggregate
                {
                    Id = "manualActivation",
                    AssemblyQualifiedName = typeof(ManualActivationTask).AssemblyQualifiedName
                },
                new CaseManagementProcessAggregate
                {
                    Id = "caseWithRepetitionRule_countClients",
                    AssemblyQualifiedName = typeof(CountClientsTask).AssemblyQualifiedName
                },
                new CaseManagementProcessAggregate
                {
                    Id = "caseWithRepetitionRule_sendEmail",
                    AssemblyQualifiedName = typeof(SendEmailToClientTask).AssemblyQualifiedName
                },
                new CaseManagementProcessAggregate
                {
                    Id = "caseWithCaseFileItemAndOneRepetitionRule_sendEmail",
                    AssemblyQualifiedName = typeof(Delegates.CaseWithCaseFileItemAndOneRepetitionRule.SendEmailTaskDelegate).AssemblyQualifiedName
                },
                new CaseManagementProcessAggregate
                {
                    Id = "caseWithMilestoneAndOneRepetitionRule_receiveTask",
                    AssemblyQualifiedName = typeof(Delegates.CaseWithMilestoneAndOneRepetitionRule.ReceiveTaskDelegate).AssemblyQualifiedName
                }
            })
            .AddForms(new List<FormAggregate>
            {
                new FormAggregate
                {
                    Id = "createMeetingForm",
                    Titles = new List<Translation>
                    {
                        new Translation("en", "Create meeting")
                    },
                    Elements = new List<FormElement>
                    {
                        new FormElement
                        {
                            Id = "name",
                            Names = new List<Translation>
                            {
                                new Translation("en", "Name")
                            },
                            Descriptions = new List<Translation>
                            {
                                new Translation("en", "Name of the meeting"),
                                new Translation("fr", "Intitulé de la réunion")
                            },
                            Type = FormElementTypes.TXT,
                            IsRequired = true
                        }
                    }
                }
            })
            .AddRoles(new List<RoleAggregate>
            {
                new RoleAggregate
                {
                    Id = "admin",
                    Name = "admin",
                    UserIds = new List<string>
                    {
                        "thabart"
                    }
                }
            });
            */
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseAuthentication();
            app.UseCMMN();
        }
    }

    public static class ServiceCollectionExtensions
    {
        public static AuthenticationBuilder AddCustomAuthentication(this AuthenticationBuilder authBuilder, Action<AuthenticationSchemeOptions> callback)
        {
            authBuilder.AddScheme<AuthenticationSchemeOptions, CustomAuthenticationHandler>(CookieAuthenticationDefaults.AuthenticationScheme, CookieAuthenticationDefaults.AuthenticationScheme, callback);
            return authBuilder;
        }
    }
}
