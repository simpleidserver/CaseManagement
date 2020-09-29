using CaseManagement.Common.Domains;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CaseManagement.HumanTask.Domains
{
    [DebuggerDisplay("Group names are assigned as potential owners")]
    public class PotentialOwnerGroupNamesAssignedEvent : DomainEvent
    {
        public PotentialOwnerGroupNamesAssignedEvent(string id, string aggregateId, int version, ICollection<string> groupNames, DateTime updateDateTime) : base(id, aggregateId, version)
        {
            GroupNames = groupNames;
            UpdateDateTime = updateDateTime;
        }

        public ICollection<string> GroupNames { get; set; }
        public DateTime UpdateDateTime { get; set; }
    }
}
