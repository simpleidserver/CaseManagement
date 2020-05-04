using CaseManagement.CMMN;
using CaseManagement.CMMN.Domains;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace CaseManagement.AspNetWebApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        private CaseJobServer _jobServer;

        protected void Application_Start()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddCMMNApi(opts => { })
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
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(config => WebApiConfig.Register(config, serviceCollection));
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            _jobServer = new CaseJobServer(serviceCollection, opts => {  });
            _jobServer.Start();
        }

        protected void Application_End()
        {
            if (_jobServer != null)
            {
                _jobServer.Stop();
            }
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            if (HttpContext.Current.Request.HttpMethod == "OPTIONS")
            {
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.SuppressContent = true;
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
        }
    }
}
