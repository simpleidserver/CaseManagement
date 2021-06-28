// Copyright (c) SimpleIdServer. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Parser;
using CaseManagement.CMMN.Persistence.EF;
using CaseManagement.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using NEventStore;
using NEventStore.Persistence.Sql.SqlDialects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading;

namespace CaseManagement.CMMN.SqlServer.Host
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
            var connectionString = _configuration.GetConnectionString("db");
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
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
                        "https://simpleidserver.northeurope.cloudapp.azure.com/openid"
                    },
                    ValidIssuers = new List<string>
                    {
                        "http://localhost:60000",
                        "https://simpleidserver.northeurope.cloudapp.azure.com/openid"
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
                        "https://simpleidserver.northeurope.cloudapp.azure.com/oauth"
                    }
                };
            });
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
                policy.AddPolicy("disable_caseplaninstance", p => p.RequireAuthenticatedUser());
                policy.AddPolicy("reenable_caseplaninstance", p => p.RequireAuthenticatedUser());
                policy.AddPolicy("complete_caseplaninstance", p =>
                {
                    p.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme, "OAuthScheme");
                    p.RequireAssertion(_ =>
                    {
                        if (_.User == null || _.User.Claims == null || !_.User.Claims.Any())
                        {
                            return false;
                        }

                        var cl = _.User.Claims.FirstOrDefault(_ => _.Type == "scope" && _.Value == "complete_humantask");
                        if (cl != null)
                        {
                            return true;
                        }

                        cl = _.User.Claims.FirstOrDefault(_ => _.Type == "sub" || _.Type == ClaimTypes.NameIdentifier);
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
            services.AddCaseApi(callback: opt =>
            {
                opt.CallbackUrl = "http://localhost:60005/case-plan-instances/{id}/complete/{eltId}";
                opt.WSHumanTaskAPI = "http://localhost:60006";
            });

            var wireup = Wireup.Init()
                .UsingSqlPersistence(System.Data.SqlClient.SqlClientFactory.Instance, connectionString)
                .WithDialect(new MsSqlDialect())
                .UsingBinarySerialization()
                .Build();
            services.AddSingleton(wireup);
            services.AddCaseManagementEFStore(opts =>
            {
                opts.UseSqlServer(connectionString, o => o.MigrationsAssembly(migrationsAssembly));
            });
            services.AddDistributedLockSQLServer(opts =>
            {
                opts.ConnectionString = connectionString;
            });
            services.AddSwaggerGen();
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            InitializeDatabase(app);
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

        private void InitializeDatabase(IApplicationBuilder app)
        {
            var pathLst = Directory.EnumerateFiles(Path.Combine(Directory.GetCurrentDirectory(), "Cmmns"), "*.cmmn").ToList();
            using (var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                using (var context = scope.ServiceProvider.GetService<CaseManagementDbContext>())
                {
                    context.Database.Migrate();
                    if (context.CasePlans.Any())
                    {
                        return;
                    }

                    var commitAggregateHelper = (ICommitAggregateHelper)scope.ServiceProvider.GetService(typeof(ICommitAggregateHelper));
                    foreach (var path in pathLst)
                    {
                        var cmmnTxt = File.ReadAllText(path);
                        var name = Path.GetFileName(path);
                        var caseFile = CaseFileAggregate.New(name, name, 0, cmmnTxt);
                        var tDefinitions = CMMNParser.ParseWSDL(cmmnTxt);
                        var newCaseFile = caseFile.Publish();
                        commitAggregateHelper.Commit(caseFile, CaseFileAggregate.GetStreamName(caseFile.AggregateId), CancellationToken.None).Wait();
                        commitAggregateHelper.Commit(newCaseFile, CaseFileAggregate.GetStreamName(newCaseFile.AggregateId), CancellationToken.None).Wait();
                    }

                    context.SaveChanges();
                }
            }
        }
    }
}