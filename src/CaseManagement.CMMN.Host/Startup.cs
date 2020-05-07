// Copyright (c) SimpleIdServer. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
using CaseManagement.CMMN.AspNetCore;
using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Host.Delegates;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace CaseManagement.CMMN.Host
{
    public class Startup
    {
        private readonly IHostingEnvironment _env;
        private readonly IConfiguration _configuration;

        public Startup(IHostingEnvironment env, IConfiguration configuration) 
        {
            _env = env;
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(opts => opts.EnableEndpointRouting = false).AddNewtonsoftJson();
            services.AddAuthentication(options =>
            {
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = ExtractKey("oauth_puk.txt"),
                    ValidAudiences = new List<string>
                    {
                        "websiteGateway"
                    },
                    ValidIssuers = new List<string>
                    {
                        "http://localhost:60001"
                    }
                };
            })
            .AddJwtBearer("IdentityServer", options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = ExtractKey("openid_puk.txt"),
                    ValidAudiences = new List<string>
                    {
                        "http://localhost:60000",
                        "http://simpleidserver.northeurope.cloudapp.azure.com/openid"
                    },
                    ValidIssuers = new List<string>
                    {
                        "http://localhost:60000",
                        "http://simpleidserver.northeurope.cloudapp.azure.com/openid"
                    }
                };
            });
            services.AddAuthorization(policy =>
            {
                // Case file
                policy.AddPolicy("me_add_casefile", p =>
                {
                    p.AuthenticationSchemes.Clear();
                    p.AuthenticationSchemes.Add("IdentityServer");
                    p.RequireAuthenticatedUser();
                });
                policy.AddPolicy("add_casefile", p => p.RequireClaim("scope", "add_casefile"));
                policy.AddPolicy("me_update_casefile", p =>
                {
                    p.AuthenticationSchemes.Clear();
                    p.AuthenticationSchemes.Add("IdentityServer");
                    p.RequireAuthenticatedUser();
                });
                policy.AddPolicy("update_casefile", p => p.RequireClaim("scope", "update_casefile"));
                policy.AddPolicy("me_publish_casefile", p =>
                {
                    p.AuthenticationSchemes.Clear();
                    p.AuthenticationSchemes.Add("IdentityServer");
                    p.RequireAuthenticatedUser();
                });
                policy.AddPolicy("publish_casefile", p => p.RequireClaim("scope", "publish_casefile"));
                policy.AddPolicy("me_get_casefile", p =>
                {
                    p.AuthenticationSchemes.Clear();
                    p.AuthenticationSchemes.Add("IdentityServer");
                    p.RequireAuthenticatedUser();
                });
                policy.AddPolicy("get_casefile", p => p.RequireClaim("scope", "get_casefile"));
                // Form instances
                policy.AddPolicy("get_forminstances", p => p.RequireClaim("scope", "get_forminstances"));
                // Form
                policy.AddPolicy("get_form", p => p.RequireClaim("scope", "get_form"));
                policy.AddPolicy("search_form", p => p.RequireClaim("scope", "search_form"));
                // Case plan instance
                policy.AddPolicy("search_caseplaninstance", p => p.RequireClaim("scope", "search_caseplaninstance"));
                policy.AddPolicy("me_search_caseplaninstance", p =>
                {
                    p.AuthenticationSchemes.Clear();
                    p.AuthenticationSchemes.Add("IdentityServer");
                    p.RequireAuthenticatedUser();
                });
                policy.AddPolicy("me_get_caseplaninstance", p =>
                {
                    p.AuthenticationSchemes.Clear();
                    p.AuthenticationSchemes.Add("IdentityServer");
                    p.RequireAuthenticatedUser();
                });
                policy.AddPolicy("get_caseplaninstance", p => p.RequireClaim("scope", "get_caseplaninstance"));
                policy.AddPolicy("get_casefileitems", p => p.RequireClaim("scope", "get_casefileitems"));
                policy.AddPolicy("me_add_caseplaninstance", p =>
                {
                    p.AuthenticationSchemes.Clear();
                    p.AuthenticationSchemes.Add("IdentityServer");
                    p.RequireAuthenticatedUser();
                });
                policy.AddPolicy("add_caseplaninstance", p => p.RequireClaim("scope", "add_caseplaninstance"));
                policy.AddPolicy("add_caseplaniteminstance", p => p.RequireClaim("scope", "add_caseplaniteminstance"));
                policy.AddPolicy("me_launch_caseplaninstance", p =>
                {
                    p.AuthenticationSchemes.Clear();
                    p.AuthenticationSchemes.Add("IdentityServer");
                    p.RequireAuthenticatedUser();
                });
                policy.AddPolicy("launch_caseplaninstance", p => p.RequireClaim("scope", "launch_caseplaninstance"));
                policy.AddPolicy("me_close_caseplaninstance", p =>
                {
                    p.AuthenticationSchemes.Clear();
                    p.AuthenticationSchemes.Add("IdentityServer");
                    p.RequireAuthenticatedUser();
                });
                policy.AddPolicy("close_caseplaninstance", p => p.RequireClaim("scope", "close_caseplaninstance"));
                policy.AddPolicy("me_suspend_caseplaninstance", p =>
                {
                    p.AuthenticationSchemes.Clear();
                    p.AuthenticationSchemes.Add("IdentityServer");
                    p.RequireAuthenticatedUser();
                });
                policy.AddPolicy("suspend_caseplaninstance", p => p.RequireClaim("scope", "suspend_caseplaninstance"));
                policy.AddPolicy("me_reactivate_caseplaninstance", p =>
                {
                    p.AuthenticationSchemes.Clear();
                    p.AuthenticationSchemes.Add("IdentityServer");
                    p.RequireAuthenticatedUser();
                });
                policy.AddPolicy("reactivate_caseplaninstance", p => p.RequireClaim("scope", "reactivate_caseplaninstance"));
                policy.AddPolicy("me_resume_caseplaninstance", p =>
                {
                    p.AuthenticationSchemes.Clear();
                    p.AuthenticationSchemes.Add("IdentityServer");
                    p.RequireAuthenticatedUser();
                });
                policy.AddPolicy("resume_caseplaninstance", p => p.RequireClaim("scope", "resume_caseplaninstance"));
                policy.AddPolicy("me_terminate_caseplaninstance", p =>
                {
                    p.AuthenticationSchemes.Clear();
                    p.AuthenticationSchemes.Add("IdentityServer");
                    p.RequireAuthenticatedUser();
                });
                policy.AddPolicy("terminate_caseplaninstance", p => p.RequireClaim("scope", "terminate_caseplaninstance"));
                policy.AddPolicy("me_confirm_caseplaninstance", p =>
                {
                    p.AuthenticationSchemes.Clear();
                    p.AuthenticationSchemes.Add("IdentityServer");
                    p.RequireAuthenticatedUser();
                });
                policy.AddPolicy("confirm_caseplaninstance", p => p.RequireClaim("scope", "confirm_caseplaninstance"));
                policy.AddPolicy("me_activate_caseplaninstance", p =>
                {
                    p.AuthenticationSchemes.Clear();
                    p.AuthenticationSchemes.Add("IdentityServer");
                    p.RequireAuthenticatedUser();
                });
                policy.AddPolicy("activate_caseplaninstance", p => p.RequireClaim("scope", "activate_caseplaninstance"));
                // Case plan
                policy.AddPolicy("me_get_caseplan", p =>
                {
                    p.AuthenticationSchemes.Clear();
                    p.AuthenticationSchemes.Add("IdentityServer");
                    p.RequireAuthenticatedUser();
                });
                policy.AddPolicy("get_caseplan", p => p.RequireClaim("scope", "get_caseplan"));
                // Case worker task
                policy.AddPolicy("get_caseworkertasks", p => p.RequireClaim("scope", "get_caseworkertasks"));
                // Performance
                policy.AddPolicy("get_performance", p => p.RequireClaim("scope", "get_performance"));
                // Statistic
                policy.AddPolicy("get_statistic", p => p.RequireClaim("scope", "get_statistic"));
                // Role
                policy.AddPolicy("get_role", p => p.RequireClaim("scope", "get_role"));
                policy.AddPolicy("search_role", p => p.RequireClaim("scope", "search_role"));
                policy.AddPolicy("add_role", p => p.RequireClaim("scope", "add_role"));
                policy.AddPolicy("delete_role", p => p.RequireClaim("scope", "delete_role"));
                policy.AddPolicy("update_role", p => p.RequireClaim("scope", "update_role"));
            });
            services.AddCors(options => options.AddPolicy("AllowAll", p => p.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()));
            services.AddHostedService<BusHostedService>();
            services.AddCMMNApi();
            services.AddCMMNEngine()
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
                .AddRoles(new List<RoleAggregate>
                {
                    new RoleAggregate
                    {
                        Id = "caseworker",
                        UserIds = new List<string>
                        {
                            "caseworker"
                        },
                        CreateDateTime = DateTime.UtcNow,
                        Version = 0
                    }
                })
                .AddForms(new List<FormAggregate>
                {
                    new FormAggregate
                    {
                        Id = FormAggregate.BuildIdentifier("form", 0),
                        Titles = new List<Translation>
                        {
                            new Translation("fr", "Mettre à jour le client"),
                            new Translation("en", "Update the client")
                        },
                        FormId ="form",
                        Elements = new List<FormElement>
                        {
                            new FormElement
                            {
                                Id = "name",
                                Type = FormElementTypes.TXT,
                                Names = new List<Translation>
                                {
                                    new Translation("fr", "Nom"),
                                    new Translation("en", "Name")
                                },
                                Descriptions = new List<Translation>
                                {
                                    new Translation("fr", "Nom du client"),
                                    new Translation("en", "Name of the client")
                                }
                            }
                        }
                    }
                });
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (_configuration.GetChildren().Any(i => i.Key == "pathBase"))
            {
                app.UsePathBase(_configuration["pathBase"]);
            }

            app.UseForwardedHeaders();
            app.UseAuthentication();
            app.UseCors("AllowAll");
            app.UseMvc();
        }

        private RsaSecurityKey ExtractKey(string fileName)
        {
            var json = File.ReadAllText(Path.Combine(_env.ContentRootPath, fileName));
            var dic = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            var rsa = RSA.Create();
            var rsaParameters = new RSAParameters
            {
                Modulus = Convert.FromBase64String(dic["n"].ToString()),
                Exponent = Convert.FromBase64String(dic["e"].ToString())
            };
            rsa.ImportParameters(rsaParameters);
            return new RsaSecurityKey(rsa);
        }
    }
}