using CaseManagement.CMMN.Acceptance.Tests.Middlewares;
using CaseManagement.CMMN.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
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
                // Case plan
                policy.AddPolicy("get_caseplan", p => p.RequireAuthenticatedUser());
                // Case worker task
                policy.AddPolicy("get_caseworkertasks", p => p.RequireAuthenticatedUser());
            });
            services.AddMvc();
            services.AddHostedService<CMMNJobServerHostedService>();
            var files = Directory.EnumerateFiles(Path.Combine(Directory.GetCurrentDirectory(), "Cmmns"), "*.cmmn").ToList();
            services.AddCaseApi();
            services.AddCaseJobServer().AddDefinitions(files);
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
