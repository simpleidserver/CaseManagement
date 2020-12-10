// Copyright (c) SimpleIdServer. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
using CaseManagement.BPMN.AspNetCore;
using CaseManagement.BPMN.Domains;
using CaseManagement.BPMN.Persistence.EF;
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
using NEventStore.Persistence.Sql;
using NEventStore.Persistence.Sql.SqlDialects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Threading;

namespace CaseManagement.BPMN.SqlServer.Host
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
                        "http://simpleidserver.northeurope.cloudapp.azure.com/openid"
                    },
                    ValidIssuers = new List<string>
                    {
                        "http://localhost:60000",
                        "http://simpleidserver.northeurope.cloudapp.azure.com/openid"
                    }
                };
            });
            services.AddAuthorization(_ => _.AddDefaultBPMNAuthorizationPolicy());
            services.AddCors(options => options.AddPolicy("AllowAll", p => p.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()));
            services.AddHostedService<BPMNJobServerHostedService>();
            services.AddProcessJobServer(callbackServerOpts: opts =>
            {
                opts.WSHumanTaskAPI = "http://localhost:60006";
                opts.CallbackUrl = "http://localhost:60007/processinstances/{id}/complete/{eltId}";
            });
            services.AddBPMNStoreEF(opts =>
            {
                opts.UseSqlServer(_configuration.GetConnectionString("db"), o => o.MigrationsAssembly(migrationsAssembly));
            });

            var factory = new NetStandardConnectionFactory(SqlClientFactory.Instance, _configuration.GetConnectionString("db"));
            var wireup = Wireup.Init()
                .UsingSqlPersistence(factory)
                .WithDialect(new SqlDialect())
                .InitializeStorageEngine()
                .UsingBinarySerialization()
                .Build();
            services.AddSingleton<IStoreEvents>(wireup);
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

            InitializeDatabase(app);
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
                Modulus = Convert.FromBase64String(dic["n"].ToString()),
                Exponent = Convert.FromBase64String(dic["e"].ToString())
            };
            rsa.ImportParameters(rsaParameters);
            return new RsaSecurityKey(rsa);
        }

        private void InitializeDatabase(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                using (var context = scope.ServiceProvider.GetService<BPMNDbContext>())
                {
                    context.Database.Migrate();
                    if (context.ProcessFiles.Any())
                    {
                        return;
                    }

                    var commitAggregateHelper = scope.ServiceProvider.GetService<ICommitAggregateHelper>();
                    var pathLst = Directory.EnumerateFiles(Path.Combine(_env.ContentRootPath, "Bpmns"), "*.bpmn").ToList();
                    foreach(var path in pathLst)
                    {
                        var bpmnTxt = File.ReadAllText(path);
                        var name = Path.GetFileName(path);
                        var processFile = ProcessFileAggregate.New(name, name, name, 0, bpmnTxt);
                        commitAggregateHelper.Commit(processFile, processFile.GetStreamName(), CancellationToken.None).Wait();
                    }
                }
            }
        }
    }
}