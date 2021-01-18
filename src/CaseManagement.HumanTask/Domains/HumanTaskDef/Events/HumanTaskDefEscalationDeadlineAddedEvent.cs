using CaseManagement.Common.Domains;
using System;
using System.Diagnostics;

namespace CaseManagement.HumanTask.Domains.HumanTaskDef.Events
{
    [DebuggerDisplay("Add escalation deadline")]
    public class HumanTaskDefEscalationDeadlineAddedEvent : DomainEvent
    {
        public HumanTaskDefEscalationDeadlineAddedEvent(string id,
            string aggregateId,
            int version,
            string deadlineId,
            string escalationId,
            string condition,
            DateTime updateDateTime) : base(id, aggregateId, version)
        {
            DeadLineId = deadlineId;
            EscalationId = escalationId;
            Condition = condition;
            UpdateDateTime = updateDateTime;
        }

        public string DeadLineId { get; set; }
        public string EscalationId { get; set; }
        public string Condition { get; set; }
        public DateTime UpdateDateTime { get; set; }
    }
}
