using CaseManagement.Common.Jobs.Persistence;
using CaseManagement.HumanTask.AspNetCore;
using CaseManagement.HumanTask.AspNetCore.Apis;
using CaseManagement.HumanTask.Builders;
using CaseManagement.HumanTask.Domains;
using CaseManagement.HumanTask.Infrastructure.Jobs;
using CaseManagement.HumanTasks.Acceptance.Tests.Middlewares;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

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
                policy.AddPolicy("create_humantaskinstance", p => p.RequireAuthenticatedUser());
            });
            services
                .AddMvc(opts => opts.EnableEndpointRouting = false)
                .AddApplicationPart(typeof(HumanTaskDefsController).Assembly)
                .AddNewtonsoftJson();
            services.AddLogging();
            var emptyTask = HumanTaskDefBuilder.New("emptyTask")
                .SetTaskInitiatorUserIdentifiers(new List<string> { "taskInitiator" })
                .SetPotentialOwnerUserIdentifiers(new List<string> { "administrator" })
                .Build();
            var addClient = HumanTaskDefBuilder.New("addClient")
                .SetTaskInitiatorUserIdentifiers(new List<string> { "taskInitiator" })
                .SetPotentialOwnerUserIdentifiers(new List<string> { "administrator" })
                .AddDescription("en-US", "<b>firstname is $firstName$, isGoldenClient $isGoldenClient$</b>", "text/html")
                .AddPresentationParameter("firstName", ParameterTypes.STRING, "context.GetInput(\"firstName\")")
                .AddPresentationParameter("isGoldenClient", ParameterTypes.BOOL, "context.GetInput(\"isGoldenClient\")")
                .AddInputOperationParameter("firstName", ParameterTypes.STRING, true)
                .AddInputOperationParameter("isGoldenClient", ParameterTypes.BOOL, false)
                .AddOutputOperationParameter("wage", ParameterTypes.INT, true)
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
                .AddInputOperationParameter("firstName", ParameterTypes.STRING, true)
                .SetTaskInitiatorUserIdentifiers(new List<string> { "taskInitiator" })
                .SetPotentialOwnerUserIdentifiers(new List<string> { "administrator", "guest" })
                .AddStartDeadLine("sendReminder", (dl) =>
                {
                    dl.SetUntilExpression("P0Y0M0DT0H0M2S");
                    dl.AddEscalation(eb =>
                    {
                        eb.AddToPart("firstName", "context.GetInput(\"firstName\")");
                        eb.SetNotification("firstNotification");
                    });
                })
                .Build();     
            var completionDeadLine = HumanTaskDefBuilder.New("completionDeadLine")
                .AddInputOperationParameter("firstName", ParameterTypes.STRING, true)
                .SetTaskInitiatorUserIdentifiers(new List<string> { "taskInitiator" })
                .SetPotentialOwnerUserIdentifiers(new List<string> { "administrator", "guest" })
                .AddCompletionDeadLine("sendReminder", (dl) =>
                {
                    dl.SetUntilExpression("P0Y0M0DT0H0M2S");
                    dl.AddEscalation(eb =>
                    {
                        eb.AddToPart("firstName", "context.GetInput(\"firstName\")");
                        eb.SetNotification("secondNotification");
                    });
                })
                .Build();
            var compositeTask = HumanTaskDefBuilder.New("compositeTask")
                .SetTaskInitiatorUserIdentifiers(new List<string> { "taskInitiator" })
                .SetPotentialOwnerUserIdentifiers(new List<string> { "administrator" })
                .AddInputOperationParameter("firstName", ParameterTypes.STRING, true)
                .AddInputOperationParameter("isGoldenClient", ParameterTypes.BOOL, false)
                .SetParallelComposition(InstantiationPatterns.AUTOMATIC, cb =>
                {
                    cb.AddSubTask("addClient", new List<ToPart>
                    {
                        new ToPart { Expression = "context.GetInput(\"firstName\")", Name = "firstName" },
                        new ToPart { Expression = "context.GetInput(\"isGoldenClient\")", Name = "isGoldenClient" }
                    });
                })
                .Build();
            var compositeTaskWithAutomaticCompletionBehavior = HumanTaskDefBuilder.New("compositeTaskWithAutomaticCompletionBehavior")
                .SetTaskInitiatorUserIdentifiers(new List<string> { "taskInitiator" })
                .SetPotentialOwnerUserIdentifiers(new List<string> { "administrator" })
                .AddInputOperationParameter("firstName", ParameterTypes.STRING, true)
                .AddInputOperationParameter("isGoldenClient", ParameterTypes.BOOL, false)
                .SetParallelComposition(InstantiationPatterns.AUTOMATIC, cb =>
                {
                    cb.AddSubTask("addClient", new List<ToPart>
                    {
                        new ToPart { Expression = "context.GetInput(\"firstName\")", Name = "firstName" },
                        new ToPart { Expression = "context.GetInput(\"isGoldenClient\")", Name = "isGoldenClient" }
                    });
                    cb.AddSubTask("emptyTask", new List<ToPart>());
                })
                .SetAutomaticCompletion(cb =>
                {
                    cb.AddCompletion("context.GetIntOutput(\"wage\", \"addClient\") > 2", new List<Copy>
                    {
                        new Copy
                        {
                            From = "context.GetIntOutput(\"wage\", \"addClient\")",
                            To = "wage"
                        }
                    });
                })
                .Build();
            var firstNotification = NotificationDefBuilder.New("firstNotification")
                .AddInputOperationParameter("firstName", ParameterTypes.STRING, true)
                .SetRecipientUserIdentifiers(new List<string> { "guest" })
                .AddName("en-US", "<b>firstName is $firstName$</b>")
                .AddPresentationParameter("firstName", ParameterTypes.STRING, "context.GetInput(\"firstName\")")
                .Build();
            var secondNotification = NotificationDefBuilder.New("secondNotification")
                .AddInputOperationParameter("firstName", ParameterTypes.STRING, true)
                .SetRecipientUserIdentifiers(new List<string> { "guest" })
                .AddName("en-US", "<b>firstName is $firstName$</b>")
                .AddPresentationParameter("firstName", ParameterTypes.STRING, "context.GetInput(\"firstName\")")
                .Build();
            services.AddHostedService<HumanTaskJobServerHostedService>();
            services.AddHumanTasksApi();
            services.AddHumanTaskServer()
                .AddHumanTaskDefs(new List<HumanTaskDefinitionAggregate>
                {
                    addClient,
                    noPotentialOwner,
                    multiplePotentialOwners,
                    startDeadLine,
                    compositeTask,
                    completionDeadLine,
                    compositeTaskWithAutomaticCompletionBehavior,
                    emptyTask
                })
                .AddNotificationDefs(new List<NotificationDefinitionAggregate>
                {
                    firstNotification,
                    secondNotification
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
