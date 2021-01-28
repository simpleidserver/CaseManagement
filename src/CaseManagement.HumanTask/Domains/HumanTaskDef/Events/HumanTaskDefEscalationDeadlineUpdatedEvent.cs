using CaseManagement.Common.Domains;
using System;
using System.Diagnostics;

namespace CaseManagement.HumanTask.Domains
{
    [DebuggerDisplay("Escalation is updated")]
    public class HumanTaskDefEscalationDeadlineUpdatedEvent : DomainEvent
    {
        public HumanTaskDefEscalationDeadlineUpdatedEvent(string id, string aggregateId, int version, string deadlineId, string escalationId, string condition, string notificationId, DateTime updateDateTime) : base(id, aggregateId, version)
        {
            DeadLineId = deadlineId;
            EscalationId = escalationId;
            Condition = condition;
            NotificationId = notificationId;
            UpdateDateTime = updateDateTime;
        }

        public string DeadLineId { get; set; }
        public string EscalationId { get; set; }
        public string Condition { get; set; }
        public string NotificationId { get; set; }
        public DateTime UpdateDateTime { get; set; }
    }
}
