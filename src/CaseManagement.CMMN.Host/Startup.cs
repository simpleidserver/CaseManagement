// Copyright (c) SimpleIdServer. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
using CaseManagement.CMMN.AspNetCore;
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
            var files = Directory.EnumerateFiles(Path.Combine(Directory.GetCurrentDirectory(), "Cmmns"), "*.cmmn").ToList();
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
            })
            .AddJwtBearer("OAuthScheme", options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = ExtractKey("oauth_puk.txt"),
                    ValidAudiences = new List<string>
                    {
                        "humanTaskClient"
                    },
                    ValidIssuers = new List<string>
                    {
                        "http://localhost:60001",
                        "http://simpleidserver.northeurope.cloudapp.azure.com/oauth"
                    }
                };
            }); ;
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
                policy.AddPolicy("complete_caseplaninstance", p => 
                {
                    p.AddAuthenticationSchemes("OAuthScheme");
                    p.RequireAssertion(_ =>
                    {
                        if (_.User == null || _.User.Claims == null  || !_.User.Claims.Any())
                        {
                            return false;
                        }

                        var cl = _.User.Claims.FirstOrDefault(_ => _.Type == "scope" && _.Value == "complete_humantask");
                        if (cl != null)
                        {
                            return true;
                        }

                        cl = _.User.Claims.FirstOrDefault(_ => _.Type == "sub");
                        if (cl != null)
                        {
                            return true;
                        }

                        return false;
                    });
                });
                // Case plan
                policy.AddPolicy("get_caseplan", p => p.RequireAuthenticatedUser());
                // Case worker task
                policy.AddPolicy("get_caseworkertasks", p => p.RequireAuthenticatedUser());
            });
            services.AddCors(options => options.AddPolicy("AllowAll", p => p.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()));
            services.AddHostedService<CMMNJobServerHostedService>();
            services.AddCaseApi();
            services.AddCaseJobServer(callback: opt =>
            {
                opt.CallbackUrl = "http://localhost:60005/case-plan-instances/{id}/complete/{eltId}";
                opt.WSHumanTaskAPI = "http://localhost:60006";
            }).AddDefinitions(files);
            services.AddSwaggerGen();
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            var pathBase = string.Empty;
            if (_configuration.GetChildren().Any(i => i.Key == "pathBase"))
            {
                pathBase = _configuration["pathBase"];
                app.UsePathBase(pathBase);
            }

            app.UseForwardedHeaders();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {				
                var edp = "/swagger/v1/swagger.json";
                if (!string.IsNullOrWhiteSpace(pathBase))
                {
                    edp = $"{pathBase}{edp}";
                }
				
                c.SwaggerEndpoint(edp, "CaseManagement API V1");
            });
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