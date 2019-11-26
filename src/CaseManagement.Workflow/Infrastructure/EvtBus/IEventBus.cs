namespace CaseManagement.Workflow.Infrastructure.EvtBus
{
    public interface IEventBus
    {
        void Publish(DomainEvent @event);

        void Subscribe<T, TH>()
            where T : DomainEvent
            where TH : IDomainEventHandler<T>;

        void SubscribeDynamic<TH>(string eventName)
            where TH : IDynamicDomainEventHandler;

        void UnsubscribeDynamic<TH>(string eventName)
            where TH : IDynamicDomainEventHandler;

        void Unsubscribe<T, TH>()
            where TH : IDomainEventHandler<T>
            where T : DomainEvent;
    }
}
