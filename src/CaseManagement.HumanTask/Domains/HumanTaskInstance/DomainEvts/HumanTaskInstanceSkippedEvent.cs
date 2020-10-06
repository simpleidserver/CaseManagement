using CaseManagement.Common.Domains;
using System;
using System.Diagnostics;

namespace CaseManagement.HumanTask.Domains
{
    [DebuggerDisplay("Skip the human task instance")]
    public class HumanTaskInstanceSkippedEvent : DomainEvent
    {
        public HumanTaskInstanceSkippedEvent(string id, string aggregateId, int version, string userPrincipal, DateTime updateDateTime) : base(id, aggregateId, version)
        {
            UserPrincipal = userPrincipal;
            UpdateDateTime = updateDateTime;
        }

        public string UserPrincipal { get; set; }
        public DateTime UpdateDateTime { get; set; }
    }
}
