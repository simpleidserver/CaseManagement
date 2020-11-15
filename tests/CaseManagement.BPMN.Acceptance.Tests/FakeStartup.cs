using CaseManagement.BPMN.Acceptance.Tests.Middlewares;
using CaseManagement.BPMN.Builders;
using CaseManagement.BPMN.Domains;
using CaseManagement.Common.Jobs.Persistence;
using CaseManagement.HumanTask.AspNetCore;
using CaseManagement.HumanTask.Builders;
using CaseManagement.HumanTask.Domains;
using CaseManagement.HumanTask.Infrastructure.Jobs;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace CaseManagement.BPMN.Acceptance.Tests
{
    public class FakeStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            var sp = services.BuildServiceProvider();
            var httpClientFactory = sp.GetRequiredService<CaseManagement.Common.Factories.IHttpClientFactory>();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCustomAuthentication(opts => { });
            services.AddAuthorization(policy =>
            {
                policy.AddPolicy("Authenticated", p => p.RequireAuthenticatedUser());
            });
            services.AddMvc();
            services.AddLogging();
            var emptyTask = HumanTaskDefBuilder.New("emptyTask")
                .SetTaskInitiatorUserIdentifiers(new List<string> { "thabart" })
                .SetPotentialOwnerUserIdentifiers(new List<string> { "thabart" })
                .AddInputOperationParameter("flowNodeInstanceId", ParameterTypes.STRING, true)
                .AddInputOperationParameter("flowNodeElementInstanceId", ParameterTypes.STRING, true)
                .AddCallbackOperation("http://localhost/processinstances/{id}/statetransitions")
                .Build();
            var processInstance = ProcessInstanceBuilder.New("1", "processId", "processFile")
                .AddStartEvent("1", "evt")
                .AddUserTask("2", "userTask", (cb) =>
                {
                    cb.SetWebservice("emptyTask");
                })
                .AddSequenceFlow("seq1", "sequence", "1", "2")
                .Build();
            services.AddHumanTasksApi();
            services.AddHumanTaskServer()
                .AddHumanTaskDefs(new List<HumanTaskDefinitionAggregate>
                {
                    emptyTask
                })
                .AddScheduledJobs(new List<ScheduleJob>
                {
                    ScheduleJob.New<ProcessActivationTimerJob>(1 * 1000),
                    ScheduleJob.New<ProcessDeadLinesJob>(1 * 1000)
                });
            services.AddProcessJobServer(callbackServerOpts: o =>
            {
                o.WSHumanTaskAPI = "http://localhost";
            })
            .AddProcessInstances(new ConcurrentBag<ProcessInstanceAggregate>
            {
                processInstance
            });
            services.AddHostedService<HumanTaskJobServerHostedService>();
            services.AddSingleton<CaseManagement.Common.Factories.IHttpClientFactory>(httpClientFactory);
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
