using CaseManagement.Common.Domains;
using System;
using System.Diagnostics;

namespace CaseManagement.HumanTask.Domains.HumanTaskDef.Events
{
    [DebuggerDisplay("Remove start deadline")]
    public class HumanTaskDefStartDeadLineRemovedEvent : DomainEvent
    {
        public HumanTaskDefStartDeadLineRemovedEvent(string id, string aggregateId, int version, string deadLineId, DateTime updateDateTime) : base(id, aggregateId, version)
        {
            DeadLineId = deadLineId;
            UpdateDateTime = updateDateTime;
        }

        public string DeadLineId { get; set; }
        public DateTime UpdateDateTime { get; set; }
    }
}
