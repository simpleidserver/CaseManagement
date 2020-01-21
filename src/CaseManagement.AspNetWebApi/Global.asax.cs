using CaseManagement.CMMN;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Linq;
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
            var files = Directory.EnumerateFiles(Path.Combine(System.Web.HttpContext.Current.Server.MapPath("."), "Cmmns"), "*.cmmn").ToList();
            serviceCollection.AddCMMNApi(opts => { })
                .AddDefinitions(files);
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
            _jobServer.Stop();
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
