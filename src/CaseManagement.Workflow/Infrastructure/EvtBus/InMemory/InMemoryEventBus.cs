using Microsoft.Extensions.DependencyInjection;
using System;

namespace CaseManagement.Workflow.Infrastructure.EvtBus.InMemory
{
    public class InMemoryEventBus : IEventBus
    {
        private readonly IEventBusSubscriptionsManager _subsManager;
        private readonly IServiceProvider _serviceProvider;

        public InMemoryEventBus(IEventBusSubscriptionsManager subsManager, IServiceProvider serviceProvider)
        {
            _subsManager = subsManager;
            _serviceProvider = serviceProvider;
        }

        public void Publish(DomainEvent @event)
        {
            var eventName = @event.GetType().Name;
            using (var scope = _serviceProvider.CreateScope())
            {
                var subscriptions = _subsManager.GetHandlersForEvent(eventName);
                foreach (var subscription in subscriptions)
                {
                    var handler = _serviceProvider.GetService(subscription.HandlerType);
                    var eventType = _subsManager.GetEventTypeByName(eventName);
                    var concreteType = typeof(IDomainEventHandler<>).MakeGenericType(eventType);
                    concreteType.GetMethod("Handle").Invoke(handler, new object[] { @event });
                }
            }
        }

        public void Subscribe<T, TH>()
            where T : DomainEvent
            where TH : IDomainEventHandler<T>
        {
            var eventName = _subsManager.GetEventKey<T>();
            _subsManager.AddSubscription<T, TH>();
        }

        public void SubscribeDynamic<TH>(string eventName) where TH : IDynamicDomainEventHandler
        {
            _subsManager.AddDynamicSubscription<TH>(eventName);
        }

        public void Unsubscribe<T, TH>()
            where T : DomainEvent
            where TH : IDomainEventHandler<T>
        {
            var eventName = _subsManager.GetEventKey<T>();
            _subsManager.RemoveSubscription<T, TH>();
        }

        public void UnsubscribeDynamic<TH>(string eventName) where TH : IDynamicDomainEventHandler
        {
            _subsManager.RemoveDynamicSubscription<TH>(eventName);
        }
    }
}
