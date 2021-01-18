using CaseManagement.Common.Domains;
using System;
using System.Diagnostics;

namespace CaseManagement.HumanTask.Domains.HumanTaskDef.Events
{
    [DebuggerDisplay("Remove to part")]
    public class HumanTaskDefEscalationToPartRemovedEvent : DomainEvent
    {
        public HumanTaskDefEscalationToPartRemovedEvent(string id, string aggregateId, int version, string deadlineId, string escalationId, string toPartName, DateTime updateDateTime) : base(id, aggregateId, version)
        {
            DeadlineId = deadlineId;
            EscalationId = escalationId;
            ToPartName = toPartName;
            UpdateDateTime = updateDateTime;
        }

        public string DeadlineId { get; set; }
        public string EscalationId { get; set; }
        public string ToPartName { get; set; }
        public DateTime UpdateDateTime { get; set; }
    }
}
