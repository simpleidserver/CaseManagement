using CaseManagement.BPMN.Acceptance.Tests.Middlewares;
using CaseManagement.BPMN.Infrastructure.Jobs;
using CaseManagement.Common.Jobs.Persistence;
using CaseManagement.HumanTask;
using CaseManagement.HumanTask.AspNetCore;
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

namespace CaseManagement.BPMN.Acceptance.Tests
{
    public class FakeStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            var files = Directory.EnumerateFiles(Path.Combine(Directory.GetCurrentDirectory(), "Bpmns"), "*.bpmn").ToList();
            var sp = services.BuildServiceProvider();
            var httpClientFactory = sp.GetRequiredService<CaseManagement.Common.Factories.IHttpClientFactory>();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCustomAuthentication(opts => { });
            services.AddAuthorization(_ =>
            {
                _.AddPolicy("Authenticated", p => p.RequireAuthenticatedUser());
                _.AddPolicy(HumanTaskConstants.ScopeNames.CreateHumanTaskInstance, p => p.RequireAssertion(__ => true));
            });
            services.AddMvc();
            services.AddLogging();
            var emptyTask = HumanTaskDefBuilder.New("emptyTask")
                .SetTaskInitiatorUserIdentifiers(new List<string> { "thabart" })
                .SetPotentialOwnerUserIdentifiers(new List<string> { "thabart" })
                .Build();
            var dressAppropriateForm = HumanTaskDefBuilder.New("dressAppropriateForm")
                .SetTaskInitiatorUserIdentifiers(new List<string> { "thabart" })
                .SetPotentialOwnerUserIdentifiers(new List<string> { "thabart" })
                .AddInputOperationParameter("degree", ParameterTypes.STRING, true)
                .AddInputOperationParameter("city", ParameterTypes.STRING, true)
                .Build();
            var takeTemperatureForm = HumanTaskDefBuilder.New("temperatureForm")
                .SetTaskInitiatorUserIdentifiers(new List<string> { "thabart" })
                .SetPotentialOwnerUserIdentifiers(new List<string> { "thabart" })
                .AddOutputOperationParameter("degree", ParameterTypes.INT, true)
                .Build();
            services.AddHumanTasksApi();
            services.AddHumanTaskServer()
                .AddHumanTaskDefs(new List<HumanTaskDefinitionAggregate>
                {
                    emptyTask,
                    dressAppropriateForm,
                    takeTemperatureForm
                })
                .AddScheduledJobs(new List<ScheduleJob>
                {
                    ScheduleJob.New<ProcessActivationTimerJob>(1 * 1000),
                    ScheduleJob.New<ProcessDeadLinesJob>(1 * 1000)
                });
            services.AddProcessJobServer(callbackOpts: o =>
            {
                o.ApplicationAssembly = typeof(ProcessInstanceJob).Assembly;
            }, callbackServerOpts: o =>
            {
                o.WSHumanTaskAPI = "http://localhost";
                o.OAuthTokenEndpoint = "http://localhost/token";
                o.CallbackUrl = "http://localhost/processinstances/{id}/complete/{eltId}";
            })
            .AddProcessFiles(files);
            services.AddHostedService<HumanTaskJobServerHostedService>();
            services.AddSingleton<CaseManagement.Common.Factories.IHttpClientFactory>(httpClientFactory);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseCulture();
            app.Map("/token", b =>
            {
                b.Use(async (context, next) =>
                {
                    context.Response.ContentType = new System.Net.Http.Headers
                        .MediaTypeHeaderValue("application/json").ToString();
                    await context.Response.WriteAsync("{ \"id\": \"id.\" }", Encoding.UTF8);
                });
            });
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
