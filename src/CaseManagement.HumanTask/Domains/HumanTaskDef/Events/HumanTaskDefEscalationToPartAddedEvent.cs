using CaseManagement.Common.Domains;
using System;
using System.Diagnostics;

namespace CaseManagement.HumanTask.Domains.HumanTaskDef.Events
{
    [DebuggerDisplay("Add to part")]
    public class HumanTaskDefEscalationToPartAddedEvent : DomainEvent
    {
        public HumanTaskDefEscalationToPartAddedEvent(string id, string aggregateId, int version, string deadlineId, string escalationId, ToPart toPart, DateTime updateDateTime) : base(id, aggregateId, version)
        {
            DeadlineId = deadlineId;
            EscalationId = escalationId;
            ToPart = toPart;
            UpdateDateTime = updateDateTime;
        }

        public string DeadlineId { get; set; }
        public string EscalationId { get; set; }
        public ToPart ToPart { get; set; }
        public DateTime UpdateDateTime { get; set; }
    }
}
