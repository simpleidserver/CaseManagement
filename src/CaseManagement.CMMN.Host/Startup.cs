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
                policy.AddPolicy("add_casefile", p => p.RequireClaim("scope", "add_casefile"));
                policy.AddPolicy("update_casefile", p => p.RequireClaim("scope", "update_casefile"));
                policy.AddPolicy("publish_casefile", p => p.RequireClaim("scope", "publish_casefile"));
                policy.AddPolicy("get_casefile", p => p.RequireClaim("scope", "get_casefile"));
                // Case plan instance
                policy.AddPolicy("search_caseplaninstance", p => p.RequireClaim("scope", "search_caseplaninstance"));
                policy.AddPolicy("get_caseplaninstance", p => p.RequireClaim("scope", "get_caseplaninstance"));
                policy.AddPolicy("add_caseplaninstance", p => p.RequireClaim("scope", "add_caseplaninstance"));
                policy.AddPolicy("add_caseplaniteminstance", p => p.RequireClaim("scope", "add_caseplaniteminstance"));
                policy.AddPolicy("launch_caseplaninstance", p => p.RequireClaim("scope", "launch_caseplaninstance"));
                policy.AddPolicy("close_caseplaninstance", p => p.RequireClaim("scope", "close_caseplaninstance"));
                policy.AddPolicy("suspend_caseplaninstance", p => p.RequireClaim("scope", "suspend_caseplaninstance"));
                policy.AddPolicy("reactivate_caseplaninstance", p => p.RequireClaim("scope", "reactivate_caseplaninstance"));
                policy.AddPolicy("resume_caseplaninstance", p => p.RequireClaim("scope", "resume_caseplaninstance"));
                policy.AddPolicy("terminate_caseplaninstance", p => p.RequireClaim("scope", "terminate_caseplaninstance"));
                policy.AddPolicy("activate_caseplaninstance", p => p.RequireClaim("scope", "activate_caseplaninstance"));
                // Case plan
                policy.AddPolicy("get_caseplan", p => p.RequireClaim("scope", "get_caseplan"));
                // Case worker task
                policy.AddPolicy("get_caseworkertasks", p => p.RequireClaim("scope", "get_caseworkertasks"));
            });
            services.AddCors(options => options.AddPolicy("AllowAll", p => p.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()));
            services.AddHostedService<CMMNJobServerHostedService>();
            services.AddCaseApi();
            services.AddCaseJobServer();
            services.AddSwaggerGen();
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
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "CaseManagement API V1");
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