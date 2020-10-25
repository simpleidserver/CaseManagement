using CaseManagement.Common.Domains;
using System;
using System.Diagnostics;

namespace CaseManagement.HumanTask.Domains.HumanTaskDef.Events
{
    [DebuggerDisplay("Remove escalation from start deadline")]
    public class HumanTaskDefEscalationStartDeadlineRemovedEvent : DomainEvent
    {
        public HumanTaskDefEscalationStartDeadlineRemovedEvent(string id, string aggregateId, int version, string startDeadLineId, string escalationId, DateTime updateDateTime) : base(id, aggregateId, version)
        {
            StartDeadLineId = startDeadLineId;
            EscalationId = escalationId;
            UpdateDateTime = updateDateTime;
        }

        public string StartDeadLineId { get; set; }
        public string EscalationId { get; set; }
        public DateTime UpdateDateTime { get; set; }
    }
}
