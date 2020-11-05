using CaseManagement.Common.Domains;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CaseManagement.HumanTask.Domains
{
    [DebuggerDisplay("Execute nomination on the human task instance")]
    public class HumanTaskInstanceNominatedEvent : DomainEvent
    {
        public HumanTaskInstanceNominatedEvent(string id, string aggregateId, int version, IEnumerable<string> userIdentifiers, IEnumerable<string> groupNames, string userPrincipal, DateTime updateDateTime) : base(id, aggregateId, version)
        {
            UserIdentifiers = userIdentifiers;
            GroupNames = groupNames;
            UserPrincipal = userPrincipal;
            UpdateDateTime = updateDateTime;
        }

        public IEnumerable<string> UserIdentifiers { get; set; }
        public IEnumerable<string> GroupNames { get; set; }
        public string UserPrincipal { get; set; }
        public DateTime UpdateDateTime { get; set; }
    }
}
