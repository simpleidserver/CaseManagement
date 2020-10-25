using CaseManagement.Common.Domains;
using System;
using System.Diagnostics;

namespace CaseManagement.HumanTask.Domains.HumanTaskDef.Events
{
    [DebuggerDisplay("Add escalation start deadline")]
    public class HumanTaskDefEscalationStartDeadlineAddedEvent : DomainEvent
    {
        public HumanTaskDefEscalationStartDeadlineAddedEvent(string id,
            string aggregateId,
            int version,
            string startDeadLineId,
            string escalationId,
            string condition,
            DateTime updateDateTime) : base(id, aggregateId, version)
        {
            StartDeadLineId = startDeadLineId;
            EscalationId = escalationId;
            Condition = condition;
            UpdateDateTime = updateDateTime;
        }

        public string StartDeadLineId { get; set; }
        public string EscalationId { get; set; }
        public string Condition { get; set; }
        public DateTime UpdateDateTime { get; set; }
    }
}
