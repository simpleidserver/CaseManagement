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
        public DateTime CreateDateTime { get; set; }
        public DateTime UpdateDateTime { get; set; }

        public override object Clone()
        {
            return new RoleAggregate
            {
                Id = Id,
                UserIds = UserIds.ToList(),
                UpdateDateTime = UpdateDateTime,
                CreateDateTime = CreateDateTime
            };
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

        public override void Handle(object obj)
        {
            if (obj is RoleAddedEvent)
            {
                Handle((RoleAddedEvent)obj);
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
    }
}
