using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection TryAddTransient<TService, TImplementation>(this IServiceCollection services)
            where TService : class
            where TImplementation : class, TService
        {
            var service = services.FirstOrDefault(s => s.ServiceType == typeof(TService) && s.ImplementationType == typeof(TImplementation));
            if (service != null)
            {
                return services;
            }

            services.AddTransient<TService, TImplementation>();
            return services;
        }

        public static IServiceCollection TryAddSingleton<TService, TImplementation>(this IServiceCollection services)
            where TService : class
            where TImplementation : class, TService
        {
            var service = services.FirstOrDefault(s => s.ServiceType == typeof(TService) && s.ImplementationType == typeof(TImplementation));
            if (service != null)
            {
                return services;
            }

            services.AddSingleton<TService, TImplementation>();
            return services;
        }

        public static IServiceCollection TryAddScoped<TService, TImplementation>(this IServiceCollection services)
            where TService : class
            where TImplementation : class, TService
        {
            var service = services.FirstOrDefault(s => s.ServiceType == typeof(TService) && s.ImplementationType == typeof(TImplementation));
            if (service != null)
            {
                return services;
            }

            services.AddScoped<TService, TImplementation>();
            return services;
        }

        public static IServiceCollection TryAddSingleton<TService>(this IServiceCollection services, TService instance) where TService : class
        {
            var service = services.FirstOrDefault(s => s.ServiceType == typeof(TService));
            if (service != null)
            {
                return services;
            }

            services.AddSingleton<TService>(instance);
            return services;
        }

        public static IServiceCollection TryUpdateSingleton<TService>(this IServiceCollection services, TService instance) where TService : class
        {
            var service = services.FirstOrDefault(s => s.ServiceType == typeof(TService));
            if (service != null)
            {
                services.Remove(service);
            }

            services.AddSingleton<TService>(instance);
            return services;
        }

        public static IServiceCollection RegisterAllAssignableType<T>(this IServiceCollection services, Assembly assm)
        {
            return services.RegisterAllAssignableType(typeof(T), assm);
        }

        public static IServiceCollection RegisterAllAssignableType(this IServiceCollection services, Type type, Assembly assm, bool registerClass = false)
        {
            var types = assm.GetTypes().Where(p => type.IsAssignableFrom(p) || IsAssignableToGenericType(p, type));
            var addTransientMethod = typeof(ServiceCollectionServiceExtensions).GetMethods().FirstOrDefault(m =>
                m.Name == "AddTransient" &&
                m.IsGenericMethod == true &&
                m.GetGenericArguments().Count() == 2);
            var addTransientMethodClass = typeof(ServiceCollectionServiceExtensions).GetMethods().FirstOrDefault(m =>
                m.Name == "AddTransient" &&
                m.IsGenericMethod == false &&
                m.GetParameters().Count() == 2);
            foreach (var t in types)
            {
                if (t.IsInterface || t.IsAbstract)
                {
                    continue;
                }

                if (type.IsGenericTypeDefinition)
                {
                    var genericArgs = GetGenericArgs(t, type);
                    foreach (var args in genericArgs)
                    {
                        var genericType = type.MakeGenericType(args);
                        var method = addTransientMethod.MakeGenericMethod(new[] { genericType, t });
                        method.Invoke(services, new[] { services });
                        if (registerClass)
                        {
                            addTransientMethodClass.Invoke(services, new object[] { services, t });
                        }
                    }
                }
                else
                {
                    var method = addTransientMethod.MakeGenericMethod(new[] { type, t });
                    method.Invoke(services, new[] { services });
                    if (registerClass)
                    {
                        addTransientMethodClass.Invoke(services, new object[] { services, t });
                    }
                }
            }

            return services;
        }

        private static bool IsAssignableToGenericType(Type givenType, Type genericType)
        {
            return GetGenericArgs(givenType, genericType).Any();
        }

        private static ICollection<Type[]> GetGenericArgs(Type givenType, Type genericType)
        {
            var result = new List<Type[]>();
            var interfaceTypes = givenType.GetInterfaces();
            foreach (var it in interfaceTypes)
            {
                if (it.IsGenericType && it.GetGenericTypeDefinition() == genericType)
                {
                    result.Add(it.GetGenericArguments());
                }
            }

            return result;
        }
    }
}
