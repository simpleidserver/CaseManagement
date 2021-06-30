// Copyright (c) SimpleIdServer. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
using CaseManagement.BPMN.Domains;
using CaseManagement.BPMN.Host.Delegates;
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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace CaseManagement.BPMN.Host
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
            var files = Directory.EnumerateFiles(Path.Combine(Directory.GetCurrentDirectory(), "Bpmns"), "*.bpmn").ToList();
            services
                .AddMvc(opts => opts.EnableEndpointRouting = false)
                .AddNewtonsoftJson();
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
                        "https://localhost:60000",
                        "https://simpleidserver.northeurope.cloudapp.azure.com/openid"
                    },
                    ValidIssuers = new List<string>
                    {
                        "https://localhost:60000",
                        "https://simpleidserver.northeurope.cloudapp.azure.com/openid"
                    }
                };
            });
            services.AddAuthorization(_ => _.AddDefaultBPMNAuthorizationPolicy());
            services.AddCors(options => options.AddPolicy("AllowAll", p => p.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()));
            services.AddProcessJobServer(callbackServerOpts: opts =>
            {
                opts.WSHumanTaskAPI = "http://localhost:60006";
                opts.CallbackUrl = "http://localhost:60007/processinstances/{id}/complete/{eltId}";
            }).AddProcessFiles(files).AddDelegateConfigurations(GetDelegateConfigurations());
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
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "BPMN API V1");
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
                Modulus = Base64DecodeBytes(dic["n"].ToString()),
                Exponent = Base64DecodeBytes(dic["e"].ToString())
            };
            rsa.ImportParameters(rsaParameters);
            return new RsaSecurityKey(rsa);
        }

        private static byte[] Base64DecodeBytes(string base64EncodedData)
        {
            var s = base64EncodedData
                .Trim()
                .Replace(" ", "+")
                .Replace('-', '+')
                .Replace('_', '/');
            switch (s.Length % 4)
            {
                case 0:
                    return Convert.FromBase64String(s);
                case 2:
                    s += "==";
                    goto case 0;
                case 3:
                    s += "=";
                    goto case 0;
                default:
                    throw new InvalidOperationException("Illegal base64url string!");
            }
        }

        private static ConcurrentBag<DelegateConfigurationAggregate> GetDelegateConfigurations()
        {
            var getWeatherInformationDelegate = DelegateConfigurationAggregate.Create("GetWeatherInformationDelegate", typeof(GetWeatherInformationDelegate).FullName);
            getWeatherInformationDelegate.AddDisplayName("fr", "Récupérer météo");
            getWeatherInformationDelegate.AddDescription("fr", "Récupérer les informations sur la météo");
            getWeatherInformationDelegate.AddDisplayName("en", "Get meteo");
            getWeatherInformationDelegate.AddDescription("en", "Get informations about meteo");

            var sendEmailDelegate = DelegateConfigurationAggregate.Create("SendEmailDelegate", typeof(SendEmailDelegate).FullName);
            sendEmailDelegate.AddDisplayName("fr", "Envoyer un email");
            sendEmailDelegate.AddDisplayName("en", "Send email");
            sendEmailDelegate.AddRecord("httpBody", "Please click on this link to update the password");
            sendEmailDelegate.AddRecord("subject", "Update password");
            sendEmailDelegate.AddRecord("fromEmail", "");
            sendEmailDelegate.AddRecord("smtpHost", "");
            sendEmailDelegate.AddRecord("smtpPort", "");
            sendEmailDelegate.AddRecord("smtpUserName", "");
            sendEmailDelegate.AddRecord("smtpPassword", "");
            sendEmailDelegate.AddRecord("smtpEnableSsl", "");

            var updateUserPasswordDelegate = DelegateConfigurationAggregate.Create("UpdateUserPasswordDelegate", typeof(UpdateUserPasswordDelegate).FullName);
            updateUserPasswordDelegate.AddDisplayName("fr", "Mettre à jour le mot de passe");
            updateUserPasswordDelegate.AddDisplayName("en", "Update password");
            updateUserPasswordDelegate.AddRecord("clientId", "");
            updateUserPasswordDelegate.AddRecord("clientSecret", "");
            updateUserPasswordDelegate.AddRecord("tokenUrl", "https://localhost:60000/token");
            updateUserPasswordDelegate.AddRecord("userUrl", "");
            updateUserPasswordDelegate.AddRecord("scope", "update_password");

            return new ConcurrentBag<DelegateConfigurationAggregate>
            {
                getWeatherInformationDelegate,
                sendEmailDelegate,
                updateUserPasswordDelegate
            };
        }
    }
}