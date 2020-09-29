using CaseManagement.HumanTask.AspNetCore;
using CaseManagement.HumanTask.Builders;
using CaseManagement.HumanTask.Domains;
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
            });
            services.AddMvc();
            services.AddLogging();
            var addClient = HumanTaskDefBuilder.New("addClient")
                .SetTaskInitiatorUserIdentifiers(new List<string> { "taskInitiator" })
                .SetPotentialOwnerUserIdentifiers(new List<string> { "administrator" })
                .SetOperation(op =>
                {
                    op.AddParameter("firstName", ParameterTypes.STRING, true);
                    op.AddParameter("isGoldenClient", ParameterTypes.BOOL, false);
                })
                .Build();
            services.AddHostedService<HumanTaskJobServerHostedService>();
            services.AddHumanTasksApi();
            services.AddHumanTaskServer()
                .AddHumanTaskDefs(new List<HumanTaskDefinitionAggregate>
                {
                    addClient
                });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
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
