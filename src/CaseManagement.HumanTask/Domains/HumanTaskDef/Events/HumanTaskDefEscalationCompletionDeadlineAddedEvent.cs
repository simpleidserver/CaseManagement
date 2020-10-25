using CaseManagement.Common.Domains;
using System;
using System.Diagnostics;

namespace CaseManagement.HumanTask.Domains.HumanTaskDef.Events
{
    [DebuggerDisplay("Add escalation completion deadline")]
    public class HumanTaskDefEscalationCompletionDeadlineAddedEvent : DomainEvent
    {
        public HumanTaskDefEscalationCompletionDeadlineAddedEvent(string id, 
            string aggregateId, 
            int version,
            string completionDeadLineId, 
            string escalationId, 
            string condition, 
            DateTime updateDateTime) : base(id, aggregateId, version)
        {
            CompletionDeadLineId = completionDeadLineId;
            EscalationId = escalationId;
            Condition = condition;
            UpdateDateTime = updateDateTime;
        }

        public string CompletionDeadLineId { get; set; }
        public string EscalationId { get; set; }
        public string Condition { get; set; }
        public DateTime UpdateDateTime { get; set; }
    }
}
