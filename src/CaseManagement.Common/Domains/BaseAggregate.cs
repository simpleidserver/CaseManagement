using System;
using System.Collections.Concurrent;

namespace CaseManagement.Common.Domains
{
    public abstract class BaseAggregate : ICloneable
    {
        public BaseAggregate()
        {
            DomainEvents = new BlockingCollection<DomainEvent>();
        }

        public string AggregateId { get; set; }
        public int Version { get; set; }
        public BlockingCollection<DomainEvent> DomainEvents { get; set; }

        public abstract object Clone();

        public abstract void Handle(dynamic evt);
    }
}
