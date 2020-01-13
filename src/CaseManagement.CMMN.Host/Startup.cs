﻿// Copyright (c) SimpleIdServer. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Host.Delegates;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CaseManagement.CMMN.Host
{
    public class Startup
    {
        public Startup(IHostingEnvironment env) { }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie();
            services.AddAuthorization(policy =>
            {
                policy.AddPolicy("IsConnected", p => p.RequireAuthenticatedUser());
            });
            var builder = services.AddCMMN();
            services.AddCors(options => options.AddPolicy("AllowAll", p => p.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()));
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
                .AddStatistics(new ConcurrentBag<DailyStatisticAggregate>
                {
                    new DailyStatisticAggregate
                    {
                        DateTime = DateTime.UtcNow.AddDays(-8).Date,
                        NbActiveCases = 2,
                        NbClosedCases = 1,
                        NbCompletedCases = 10,
                        NbFailedCases = 11,
                        NbSuspendedCases = 12,
                        NbTerminatedCases = 20,
                        NbConfirmedActivation = 2,
                        NbCreatedActivation = 2,
                        NbConfirmedForm = 1,
                        NbCreatedForm = 1
                    },
                    new DailyStatisticAggregate
                    {
                        DateTime = DateTime.UtcNow.AddDays(-1).Date,
                        NbActiveCases = 10,
                        NbClosedCases = 10,
                        NbCompletedCases = 10,
                        NbFailedCases = 0,
                        NbSuspendedCases = 0,
                        NbTerminatedCases = 0,
                        NbConfirmedActivation = 10,
                        NbConfirmedForm = 2,
                        NbCreatedForm = 0,
                        NbCreatedActivation = 3
                    },
                    new DailyStatisticAggregate
                    {
                        DateTime = DateTime.UtcNow.Date,
                        NbActiveCases = 2,
                        NbConfirmedActivation = 1,
                        NbConfirmedForm = 1,
                        NbCreatedForm = 1,
                        NbCreatedActivation = 1
                    }
                });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseAuthentication();
            app.UseCors("AllowAll");
            app.UseCMMN();
        }
    }
}