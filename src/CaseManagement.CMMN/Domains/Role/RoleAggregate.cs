using CaseManagement.CMMN.Domains.Role.Events;
using CaseManagement.CMMN.Infrastructures;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CaseManagement.CMMN.Domains
{
    public class RoleAggregate : BaseAggregate
    {
        public RoleAggregate()
        {
            UserIds = new List<string>();
        }

        public ICollection<string> UserIds { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime UpdateDateTime { get; set; }

        public override object Clone()
        {
            return new RoleAggregate
            {
                Id = Id,
                UserIds = UserIds.ToList(),
                UpdateDateTime = UpdateDateTime,
                CreateDateTime = CreateDateTime,
                IsDeleted = IsDeleted
            };
        }

        public static RoleAggregate New(IEnumerable<DomainEvent> evts)
        {
            var result = new RoleAggregate();
            foreach(var evt in evts)
            {
                result.Handle(evt);
            }

            return result;
        }

        public static RoleAggregate New(string id)
        {
            var result = new RoleAggregate();
            lock(result.DomainEvents)
            {
                var evt = new RoleAddedEvent(Guid.NewGuid().ToString(), id, 0, DateTime.UtcNow);
                result.Handle(evt);
                result.DomainEvents.Add(evt);
                return result;
            }
        }

        public void Update(ICollection<string> users)
        {
            lock(DomainEvents)
            {
                var duplicateUsers = users.Where(u => UserIds.Contains(u));
                if (IsDeleted)
                {
                    throw new AggregateValidationException(new Dictionary<string, string>
                    {
                        { "validation", "the role is removed" }
                    });
                }

                var evt = new RoleUpdatedEvent(Guid.NewGuid().ToString(), Id, Version + 1, users, DateTime.UtcNow);
                Handle(evt);
                DomainEvents.Add(evt);
            }
        }

        public void Delete()
        {
            lock(DomainEvents)
            {
                var evt = new RoleDeletedEvent(Guid.NewGuid().ToString(), Id, Version + 1);
                Handle(evt);
                DomainEvents.Add(evt);
            }
        }

        public override void Handle(object obj)
        {
            if (obj is RoleAddedEvent)
            {
                Handle((RoleAddedEvent)obj);
            }

            if (obj is RoleUpdatedEvent)
            {
                Handle((RoleUpdatedEvent)obj);
            }

            if (obj is RoleDeletedEvent)
            {
                Handle((RoleDeletedEvent)obj);
            }
        }

        public static string GetStreamName(string id)
        {
            return $"role-{id}";
        }

        private void Handle(RoleAddedEvent evt)
        {
            Id = evt.AggregateId;
            Version = evt.Version;
            CreateDateTime = evt.CreateDateTime;
        }

        private void Handle(RoleUpdatedEvent evt)
        {
            UpdateDateTime = evt.UpdateDateTime;
            UserIds = evt.Users;
            Version = evt.Version;
        }

        private void Handle(RoleDeletedEvent evt)
        {
            IsDeleted = true;
            Version = evt.Version;
        }
    }
}
