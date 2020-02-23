// Copyright (c) SimpleIdServer. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
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

namespace CaseManagement.Gateway.Website.Host
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
            services.AddMvc();
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
                        "http://localhost:60000"
                    },
                    ValidIssuers = new List<string>
                    {
                        "http://localhost:60000"
                    }
                };
            });
            services.AddAuthorization(policy =>
            {
                policy.AddPolicy("get_statistic", p => p.RequireRole("admin"));
                policy.AddPolicy("get_performance", p => p.RequireRole("admin"));       
                policy.AddPolicy("add_casefile", p => p.RequireRole("businessanalyst"));
                policy.AddPolicy("get_casefile", p => p.RequireRole("businessanalyst"));
                policy.AddPolicy("update_casefile", p => p.RequireRole("businessanalyst"));
                policy.AddPolicy("publish_casefile", p => p.RequireRole("businessanalyst"));
                policy.AddPolicy("get_caseplan", p => p.RequireRole("businessanalyst"));
                policy.AddPolicy("search_caseplaninstance", p => p.RequireRole("businessanalyst"));
                policy.AddPolicy("add_case_instance", p => p.RequireRole("businessanalyst"));
                policy.AddPolicy("launch_caseplaninstance", p => p.RequireRole("businessanalyst"));
                policy.AddPolicy("close_caseplaninstance", p => p.RequireRole("businessanalyst"));
                policy.AddPolicy("reactivate_caseplaninstance", p => p.RequireRole("businessanalyst"));
                policy.AddPolicy("resume_caseplaninstance", p => p.RequireRole("businessanalyst"));
                policy.AddPolicy("suspend_caseplaninstance", p => p.RequireRole("businessanalyst"));
                policy.AddPolicy("terminate_caseplaninstance", p => p.RequireRole("businessanalyst"));
                policy.AddPolicy("search_caseplaninstance", p => p.RequireRole("businessanalyst"));
                policy.AddPolicy("get_caseplaninstance", p => p.RequireRole("businessanalyst"));
                policy.AddPolicy("me_search_caseplaninstance", p => p.RequireRole("caseworker"));
                policy.AddPolicy("me_get_caseplaninstance", p => p.RequireRole("businessanalyst"));
            });
            services.AddCors(options => options.AddPolicy("AllowAll", p => p.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()));
            services.AddWebsiteGateway(opts =>
            {
                opts.ApiUrl = "http://localhost:54942";
                opts.TokenUrl = "http://localhost:60001/token";
                opts.ClientId = "websiteGateway";
                opts.ClientSecret = "websiteGatewaySecret";
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
            using (var rsa = RSA.Create())
            {
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
}