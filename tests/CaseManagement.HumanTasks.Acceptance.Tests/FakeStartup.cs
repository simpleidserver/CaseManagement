using CaseManagement.Common.Jobs.Persistence;
using CaseManagement.HumanTask.AspNetCore;
using CaseManagement.HumanTask.Builders;
using CaseManagement.HumanTask.Domains;
using CaseManagement.HumanTask.Infrastructure.Jobs;
using CaseManagement.HumanTasks.Acceptance.Tests.Middlewares;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;

namespace CaseManagement.HumanTasks.Acceptance.Tests
{
    public class FakeStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCustomAuthentication(opts => { });
            services.AddAuthorization(policy =>
            {
                policy.AddPolicy("Authenticated", p => p.RequireAuthenticatedUser());
            });
            services.AddMvc();
            services.AddLogging();
            var addClient = HumanTaskDefBuilder.New("addClient")
                .SetTaskInitiatorUserIdentifiers(new List<string> { "taskInitiator" })
                .SetPotentialOwnerUserIdentifiers(new List<string> { "administrator" })
                .AddDescription("en-US", "<b>firstname is $firstName$, isGoldenClient $isGoldenClient$</b>", "text/html")
                .AddPresentationParameter("firstName", ParameterTypes.STRING, "context.GetInput(\"firstName\")")
                .AddPresentationParameter("isGoldenClient", ParameterTypes.BOOL, "context.GetInput(\"isGoldenClient\")")
                .SetOperation(op =>
                {
                    op.AddParameter("firstName", ParameterTypes.STRING, true);
                    op.AddParameter("isGoldenClient", ParameterTypes.BOOL, false);
                })
                .Build();
            var noPotentialOwner = HumanTaskDefBuilder.New("noPotentialOwner")
                .SetTaskInitiatorUserIdentifiers(new List<string> { "taskInitiator" })
                .SetBusinessAdministratorUserIdentifiers(new List<string> { "businessAdmin" })
                .Build();
            var multiplePotentialOwners = HumanTaskDefBuilder.New("multiplePotentialOwners")
                .SetTaskInitiatorUserIdentifiers(new List<string> { "taskInitiator" })
                .SetPotentialOwnerUserIdentifiers(new List<string> { "administrator", "guest" })
                .Build();
            var startDeadLine = HumanTaskDefBuilder.New("startDeadLine")
                .SetOperation(op =>
                {
                    op.AddParameter("firstName", ParameterTypes.STRING, true);
                })
                .SetTaskInitiatorUserIdentifiers(new List<string> { "taskInitiator" })
                .SetPotentialOwnerUserIdentifiers(new List<string> { "administrator", "guest" })
                .AddStartDeadLine("sendReminder", (dl) =>
                {
                    dl.SetUntilExpression("P0Y0M0DT0H0M2S");
                    dl.AddEscalation(eb =>
                    {
                        eb.AddToPart("firstName", "context.GetInput(\"firstName\")");
                        eb.SetNotification("notification", nt =>
                        {
                            nt.SetOperation(op =>
                            {
                                op.AddParameter("firstName", ParameterTypes.STRING, true);
                            });
                            nt.SetRecipientUserIdentifiers(new List<string> { "guest" });
                            nt.AddName("en-US", "<b>firstName is $firstName$</b>");
                            nt.AddPresentationParameter("firstName", ParameterTypes.STRING, "context.GetInput(\"firstName\")");                        });
                    });
                })
                .Build();
            services.AddHostedService<HumanTaskJobServerHostedService>();
            services.AddHumanTasksApi();
            services.AddHumanTaskServer()
                .AddHumanTaskDefs(new List<HumanTaskDefinitionAggregate>
                {
                    addClient,
                    noPotentialOwner,
                    multiplePotentialOwners,
                    startDeadLine
                })
                .AddScheduledJobs(new List<ScheduleJob>
                {
                    ScheduleJob.New<ProcessActivationTimerJob>(1 * 1000),
                    ScheduleJob.New<ProcessDeadLinesJob>(1 * 1000)
                });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseCulture();
            app.UseAuthentication();
            app.UseMvc();
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
