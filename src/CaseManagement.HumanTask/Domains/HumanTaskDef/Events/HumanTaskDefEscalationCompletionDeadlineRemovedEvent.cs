using CaseManagement.Common.Domains;
using System;
using System.Diagnostics;

namespace CaseManagement.HumanTask.Domains.HumanTaskDef.Events
{
    [DebuggerDisplay("Remove escalation from completion deadline")]
    public class HumanTaskDefEscalationCompletionDeadlineRemovedEvent : DomainEvent
    {
        public HumanTaskDefEscalationCompletionDeadlineRemovedEvent(string id, string aggregateId, int version, string completionDeadLineId, string escalationId, DateTime updateDateTime) : base(id, aggregateId, version)
        {
            CompletionDeadLineId = completionDeadLineId;
            EscalationId = escalationId;
            UpdateDateTime = updateDateTime;
        }

        public string CompletionDeadLineId { get; set; }
        public string EscalationId { get; set; }
        public DateTime UpdateDateTime { get; set; }
    }
}
