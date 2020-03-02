using CaseManagement.CMMN.Acceptance.Tests.Delegates;
using CaseManagement.CMMN.Acceptance.Tests.Middlewares;
using CaseManagement.CMMN.AspNetCore;
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
                // Case file
                policy.AddPolicy("me_add_casefile", p => p.RequireAuthenticatedUser());
                policy.AddPolicy("add_casefile", p => p.RequireAuthenticatedUser());
                policy.AddPolicy("me_update_casefile", p => p.RequireAuthenticatedUser());
                policy.AddPolicy("update_casefile", p => p.RequireAuthenticatedUser());
                policy.AddPolicy("me_publish_casefile", p => p.RequireAuthenticatedUser());
                policy.AddPolicy("publish_casefile", p => p.RequireAuthenticatedUser());
                policy.AddPolicy("me_get_casefile", p => p.RequireAuthenticatedUser());
                policy.AddPolicy("get_casefile", p => p.RequireAuthenticatedUser());
                // Form
                policy.AddPolicy("get_form", p => p.RequireAuthenticatedUser());
                policy.AddPolicy("search_form", p => p.RequireAuthenticatedUser());
                // Form instances
                policy.AddPolicy("get_forminstances", p => p.RequireAuthenticatedUser());
                // Case plan instance
                policy.AddPolicy("search_caseplaninstance", p => p.RequireAuthenticatedUser());
                policy.AddPolicy("me_search_caseplaninstance", p => p.RequireAuthenticatedUser());
                policy.AddPolicy("me_get_caseplaninstance", p => p.RequireAuthenticatedUser());
                policy.AddPolicy("get_caseplaninstance", p => p.RequireAuthenticatedUser());
                policy.AddPolicy("get_casefileitems", p => p.RequireAuthenticatedUser());
                policy.AddPolicy("me_add_caseplaninstance", p => p.RequireAuthenticatedUser());
                policy.AddPolicy("add_caseplaninstance", p => p.RequireAuthenticatedUser());
                policy.AddPolicy("add_caseplaniteminstance", p => p.RequireAuthenticatedUser());
                policy.AddPolicy("me_launch_caseplaninstance", p => p.RequireAuthenticatedUser());
                policy.AddPolicy("launch_caseplaninstance", p => p.RequireAuthenticatedUser());
                policy.AddPolicy("me_close_caseplaninstance", p => p.RequireAuthenticatedUser());
                policy.AddPolicy("close_caseplaninstance", p => p.RequireAuthenticatedUser());
                policy.AddPolicy("me_suspend_caseplaninstance", p => p.RequireAuthenticatedUser());
                policy.AddPolicy("suspend_caseplaninstance", p => p.RequireAuthenticatedUser());
                policy.AddPolicy("me_reactivate_caseplaninstance", p => p.RequireAuthenticatedUser());
                policy.AddPolicy("reactivate_caseplaninstance", p => p.RequireAuthenticatedUser());
                policy.AddPolicy("me_resume_caseplaninstance", p => p.RequireAuthenticatedUser());
                policy.AddPolicy("resume_caseplaninstance", p => p.RequireAuthenticatedUser());
                policy.AddPolicy("me_terminate_caseplaninstance", p => p.RequireAuthenticatedUser());
                policy.AddPolicy("terminate_caseplaninstance", p => p.RequireAuthenticatedUser());
                policy.AddPolicy("me_confirm_caseplaninstance", p => p.RequireAuthenticatedUser());
                policy.AddPolicy("confirm_caseplaninstance", p => p.RequireAuthenticatedUser());
                policy.AddPolicy("me_activate_caseplaninstance", p => p.RequireAuthenticatedUser());
                policy.AddPolicy("activate_caseplaninstance", p => p.RequireAuthenticatedUser());
                // Case plan
                policy.AddPolicy("me_get_caseplan", p => p.RequireAuthenticatedUser());
                policy.AddPolicy("get_caseplan", p => p.RequireAuthenticatedUser());
                // Case worker task
                policy.AddPolicy("get_caseworkertasks", p => p.RequireAuthenticatedUser());
                // Performance
                policy.AddPolicy("get_performance", p => p.RequireAuthenticatedUser());
                // Statistic
                policy.AddPolicy("get_statistic", p => p.RequireAuthenticatedUser());
                // Role
                policy.AddPolicy("get_role", p => p.RequireAuthenticatedUser());
                policy.AddPolicy("search_role", p => p.RequireAuthenticatedUser());
                policy.AddPolicy("add_role", p => p.RequireAuthenticatedUser());
                policy.AddPolicy("delete_role", p => p.RequireAuthenticatedUser());
                policy.AddPolicy("update_role", p => p.RequireAuthenticatedUser());
            });
            services.AddMvc();
            services.AddHostedService<BusHostedService>();
            services.AddCMMNApi();
            var files = Directory.EnumerateFiles(Path.Combine(Directory.GetCurrentDirectory(), "Cmmns"), "*.cmmn").ToList();
            services.AddCMMNEngine()
                .AddDefinitions(files, "thabart")
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
                    },
                    new CaseManagementProcessAggregate
                    {
                        Id = "incrementtask",
                        AssemblyQualifiedName = typeof(IncrementTask).AssemblyQualifiedName
                    }
                })
                .AddForms(new List<FormAggregate>
                {
                    new FormAggregate
                    {
                        Id = FormAggregate.BuildIdentifier("form", 0),
                        FormId = "form",
                        Version = 0,
                        Elements = new List<FormElement>
                        {
                            new FormElement
                            {
                                Id = "name",
                                Type = FormElementTypes.TXT
                            }
                        }
                    }
                })
                .AddRoles(new List<RoleAggregate>
                {
                    new RoleAggregate
                    {
                        Id = "admin",
                        UserIds = new List<string>
                        {
                            "thabart"
                        }
                    },
                    new RoleAggregate
                    {
                        Id = "caseworker",
                        UserIds = new List<string>
                        {
                            "caseworker"
                        }
                    }
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
