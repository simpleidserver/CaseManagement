﻿using CaseManagement.CMMN.Acceptance.Tests.Delegates;
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
                policy.AddPolicy("IsConnected", p => p.RequireAuthenticatedUser());
                policy.AddPolicy("get_statistic", p => p.RequireAuthenticatedUser());
                policy.AddPolicy("get_performance", p => p.RequireAuthenticatedUser());
                policy.AddPolicy("get_caseplan", p => p.RequireAuthenticatedUser());
                policy.AddPolicy("add_casefile", p => p.RequireAuthenticatedUser());
                policy.AddPolicy("update_casefile", p => p.RequireAuthenticatedUser());
                policy.AddPolicy("publish_casefile", p => p.RequireAuthenticatedUser());
                policy.AddPolicy("add_case_instance", p => p.RequireAuthenticatedUser());
                policy.AddPolicy("launch_case_intance", p => p.RequireAuthenticatedUser());
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
                        Id = "form",
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
                        Name = "admin",
                        UserIds = new List<string>
                        {
                            "thabart"
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
