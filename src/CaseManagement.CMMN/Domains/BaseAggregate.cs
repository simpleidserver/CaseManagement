using System;
using System.Collections.Concurrent;

namespace CaseManagement.CMMN.Infrastructures
{
    public abstract class BaseAggregate : ICloneable
    {
        public BaseAggregate()
        {
            DomainEvents = new ConcurrentBag<DomainEvent>();
        }

        public string Id { get; set; }
        public int Version { get; set; }
        public ConcurrentBag<DomainEvent> DomainEvents { get; protected set; }

        public abstract object Clone();
        public abstract void Handle(dynamic obj);
    }
}
