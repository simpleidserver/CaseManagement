using CaseManagement.Common.Domains;
using System;
using System.Diagnostics;

namespace CaseManagement.HumanTask.Domains.HumanTaskDef.Events
{
    [DebuggerDisplay("Remove deadline")]
    public class HumanTaskDefDeadLineRemovedEvent : DomainEvent
    {
        public HumanTaskDefDeadLineRemovedEvent(string id, string aggregateId, int version, string deadLineId, DateTime updateDateTime) : base(id, aggregateId, version)
        {
            DeadLineId = deadLineId;
            UpdateDateTime = updateDateTime;
        }

        public string DeadLineId { get; set; }
        public DateTime UpdateDateTime { get; set; }
    }
}
