using CaseManagement.Common.Domains;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CaseManagement.HumanTask.Domains
{
    [DebuggerDisplay("Execute nomination on the human task instance")]
    public class HumanTaskInstanceNominatedEvent : DomainEvent
    {
        public HumanTaskInstanceNominatedEvent(string id, string aggregateId, int version, ICollection<string> groupNames, ICollection<string> userIdentifiers, string userPrincipal, DateTime updateDateTime) : base(id, aggregateId, version)
        {
            GroupNames = groupNames;
            UserIdentifiers = userIdentifiers;
            UserPrincipal = userPrincipal;
            UpdateDateTime = updateDateTime;
        }

        public ICollection<string> GroupNames { get; set; }
        public ICollection<string> UserIdentifiers { get; set; }
        public string UserPrincipal { get; set; }
        public DateTime UpdateDateTime { get; set; }
    }
}
