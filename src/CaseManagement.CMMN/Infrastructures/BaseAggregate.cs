using System;
using System.Collections.Generic;

namespace CaseManagement.CMMN.Infrastructures
{
    public abstract class BaseAggregate : ICloneable
    {
        public BaseAggregate()
        {
            DomainEvents = new List<DomainEvent>();
            SupportedDomainEvents = new List<Type>();
        }

        public string Id { get; set; }
        public int Version { get; set; }
        public ICollection<DomainEvent> DomainEvents { get; protected set; }
        public ICollection<Type> SupportedDomainEvents { get; protected set; }

        public abstract object Clone();
        public abstract void Handle(object obj);
    }
}
