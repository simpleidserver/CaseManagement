using CaseManagement.CMMN.Infrastructures.Bus;
using CaseManagement.MessageBroker.MassTransit;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMassTransitMessageBroker(this IServiceCollection services, Action<MessageBrokerOptions> options)
        {
            services.AddOptions<MessageBrokerOptions>().Configure(options);
            services.AddSingleton<IMessageBroker, MassTransitMessageBroker>();
            return services;
        }
    }
}