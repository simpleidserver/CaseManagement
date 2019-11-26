using System;
using System.Collections.Generic;

namespace CaseManagement.Workflow.Infrastructure.EvtBus
{
    public interface IEventBusSubscriptionsManager
    {
        bool IsEmpty { get; }
        event EventHandler<string> OnEventRemoved;
        void AddDynamicSubscription<TH>(string eventName) where TH : IDynamicDomainEventHandler;

        void AddSubscription<T, TH>()
           where T : DomainEvent
           where TH : IDomainEventHandler<T>;

        void RemoveSubscription<T, TH>() where TH : IDomainEventHandler<T> where T : DomainEvent;
        void RemoveDynamicSubscription<TH>(string eventName) where TH : IDynamicDomainEventHandler;

        bool HasSubscriptionsForEvent<T>() where T : DomainEvent;
        bool HasSubscriptionsForEvent(string eventName);
        Type GetEventTypeByName(string eventName);
        void Clear();
        IEnumerable<SubscriptionInfo> GetHandlersForEvent<T>() where T : DomainEvent;
        IEnumerable<SubscriptionInfo> GetHandlersForEvent(string eventName);
        string GetEventKey<T>();
    }
}
