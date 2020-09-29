using CaseManagement.Common.Domains;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CaseManagement.HumanTask.Domains
{
    [DebuggerDisplay("User identifiers are assigned as potential owners")]
    public class PotentialOwnerUserIdentifiersAssignedEvent : DomainEvent
    {
        public PotentialOwnerUserIdentifiersAssignedEvent(string id, string aggregateId, int version, ICollection<string> userIdentifiers, DateTime updateDateTime) : base(id, aggregateId, version)
        {
            UserIdentifiers = userIdentifiers;
            UpdateDateTime = updateDateTime;
        }

        public ICollection<string> UserIdentifiers { get; set; }
        public DateTime UpdateDateTime { get; set; }
    }
}
