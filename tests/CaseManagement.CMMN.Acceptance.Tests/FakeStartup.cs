using CaseManagement.CMMN.Acceptance.Tests.Middlewares;
using CaseManagement.CMMN.AspNetCore;
using CaseManagement.Common.Jobs.Persistence;
using CaseManagement.HumanTask;
using CaseManagement.HumanTask.Builders;
using CaseManagement.HumanTask.Domains;
using CaseManagement.HumanTask.Infrastructure.Jobs;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CaseManagement.CMMN.Acceptance.Tests
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
                // Case file
                policy.AddPolicy("add_casefile", p => p.RequireAuthenticatedUser());
                policy.AddPolicy("update_casefile", p => p.RequireAuthenticatedUser());
                policy.AddPolicy("publish_casefile", p => p.RequireAuthenticatedUser());
                policy.AddPolicy("get_casefile", p => p.RequireAuthenticatedUser());
                // Case plan instance
                policy.AddPolicy("search_caseplaninstance", p => p.RequireAuthenticatedUser());
                policy.AddPolicy("get_caseplaninstance", p => p.RequireAuthenticatedUser());
                policy.AddPolicy("add_caseplaninstance", p => p.RequireAuthenticatedUser());
                policy.AddPolicy("add_caseplaniteminstance", p => p.RequireAuthenticatedUser());
                policy.AddPolicy("launch_caseplaninstance", p => p.RequireAuthenticatedUser());
                policy.AddPolicy("close_caseplaninstance", p => p.RequireAuthenticatedUser());
                policy.AddPolicy("suspend_caseplaninstance", p => p.RequireAuthenticatedUser());
                policy.AddPolicy("reactivate_caseplaninstance", p => p.RequireAuthenticatedUser());
                policy.AddPolicy("resume_caseplaninstance", p => p.RequireAuthenticatedUser());
                policy.AddPolicy("terminate_caseplaninstance", p => p.RequireAuthenticatedUser());
                policy.AddPolicy("activate_caseplaninstance", p => p.RequireAuthenticatedUser());
                policy.AddPolicy("complete_caseplaninstance", p => p.RequireAuthenticatedUser());
                policy.AddPolicy("disable_caseplaninstance", p => p.RequireAuthenticatedUser());
                policy.AddPolicy("reenable_caseplaninstance", p => p.RequireAuthenticatedUser());
                // Case plan
                policy.AddPolicy("get_caseplan", p => p.RequireAuthenticatedUser());
                // Case worker task
                policy.AddPolicy("get_caseworkertasks", p => p.RequireAuthenticatedUser());
                policy.AddPolicy("Authenticated", p => p.RequireAuthenticatedUser());
                policy.AddPolicy(HumanTaskConstants.ScopeNames.CreateHumanTaskInstance, p => p.RequireAssertion(__ => true));
            });
            var emptyTask = HumanTaskDefBuilder.New("emptyTask")
                .SetTaskInitiatorUserIdentifiers(new List<string> { "thabart" })
                .SetPotentialOwnerUserIdentifiers(new List<string> { "thabart" })
                .AddInputOperationParameter("city", ParameterTypes.STRING, true)
                .AddOutputOperationParameter("firstname", ParameterTypes.STRING, true)
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
            services.AddMvc();
            services.AddHostedService<CMMNJobServerHostedService>();
            var files = Directory.EnumerateFiles(Path.Combine(Directory.GetCurrentDirectory(), "Cmmns"), "*.cmmn").ToList();
            services.AddCaseApi();
            services.AddCaseJobServer(callback: opt =>
            {
                opt.CallbackUrl = "http://localhost/case-plan-instances/{id}/complete/{eltId}";
                opt.WSHumanTaskAPI = "http://localhost";
                opt.OAuthTokenEndpoint = "http://localhost/token";
            }).AddDefinitions(files);
            services.AddSingleton<CaseManagement.Common.Factories.IHttpClientFactory>(httpClientFactory);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseAuthentication();
            app.Map("/token", b =>
            {
                b.Use(async (context, next) =>
                {
                    context.Response.ContentType = new System.Net.Http.Headers
                        .MediaTypeHeaderValue("application/json").ToString();
                    await context.Response.WriteAsync("{ \"id\": \"id.\" }", Encoding.UTF8);
                });
            });
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
