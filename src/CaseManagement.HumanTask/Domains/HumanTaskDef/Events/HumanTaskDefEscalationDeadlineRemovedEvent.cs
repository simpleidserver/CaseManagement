using CaseManagement.Common.Domains;
using System;
using System.Diagnostics;

namespace CaseManagement.HumanTask.Domains.HumanTaskDef.Events
{
    [DebuggerDisplay("Remove escalation from deadline")]
    public class HumanTaskDefEscalationDeadlineRemovedEvent : DomainEvent
    {
        public HumanTaskDefEscalationDeadlineRemovedEvent(string id, string aggregateId, int version, string deadlineId, string escalationId, DateTime updateDateTime) : base(id, aggregateId, version)
        {
            DeadlineId = deadlineId;
            EscalationId = escalationId;
            UpdateDateTime = updateDateTime;
        }

        public string DeadlineId { get; set; }
        public string EscalationId { get; set; }
        public DateTime UpdateDateTime { get; set; }
    }
}
