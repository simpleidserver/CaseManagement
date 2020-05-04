using CaseManagement.AspNetWebApi.Infrastructures;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Web.Http;
using Unity;
using Unity.Lifetime;

namespace CaseManagement.AspNetWebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config, ServiceCollection serviceCollection)
        {
            var container = new UnityContainer(); 
            foreach (var d in serviceCollection)
            {
                Register(container, d);
            }

            container.RegisterInstance<IServiceProvider>(serviceCollection.BuildServiceProvider());
            config.DependencyResolver = new UnityResolver(container);
            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }

        internal static void Register(IUnityContainer container, ServiceDescriptor serviceDescriptor)
        {
            if (serviceDescriptor.ImplementationType != null)
            {
                var name = serviceDescriptor.ServiceType.IsGenericTypeDefinition ? UnityContainer.All : null;
                container.RegisterType(serviceDescriptor.ServiceType, serviceDescriptor.ImplementationType, name, (ITypeLifetimeManager)serviceDescriptor.GetLifetime());
            }
            else if (serviceDescriptor.ImplementationInstance != null)
            {
                container.RegisterInstance(serviceDescriptor.ServiceType, null, serviceDescriptor.ImplementationInstance, (IInstanceLifetimeManager)serviceDescriptor.GetLifetime());
            }
            else
            {
                throw new InvalidOperationException("Unsupported registration type");
            }
        }

        internal static LifetimeManager GetLifetime(this ServiceDescriptor serviceDescriptor)
        {
            switch (serviceDescriptor.Lifetime)
            {
                case ServiceLifetime.Scoped:
                    return new HierarchicalLifetimeManager();
                case ServiceLifetime.Singleton:
                    return new ContainerControlledLifetimeManager();
                case ServiceLifetime.Transient:
                    return new TransientLifetimeManager();
                default:
                    throw new NotImplementedException(
                        $"Unsupported lifetime manager type '{serviceDescriptor.Lifetime}'");
            }
        }
    }
}
