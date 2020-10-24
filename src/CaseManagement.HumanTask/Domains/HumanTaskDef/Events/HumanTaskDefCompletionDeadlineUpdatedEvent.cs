using CaseManagement.Common.Domains;
using System;
using System.Diagnostics;

namespace CaseManagement.HumanTask.Domains.HumanTaskDef.Events
{
    [DebuggerDisplay("Update completion deadline")]
    public class HumanTaskDefCompletionDeadlineUpdatedEvent : DomainEvent
    {
        public HumanTaskDefCompletionDeadlineUpdatedEvent(string id, string aggregateId, int version, string deadLineId, string name, string forExpr, string untilExpr, DateTime updateDateTime) : base(id, aggregateId, version)
        {
            DeadLineId = deadLineId;
            Name = name;
            ForExpr = forExpr;
            UntilExpr = untilExpr;
            UpdateDateTime = updateDateTime;
        }

        public string DeadLineId { get; set; }
        public string Name { get; set; }
        public string ForExpr { get; set; }
        public string UntilExpr { get; set; }
        public DateTime UpdateDateTime { get; set; }
    }
}
