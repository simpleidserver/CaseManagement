using CaseManagement.Common.Domains;
using System;
using System.Diagnostics;

namespace CaseManagement.HumanTask.Domains
{
    [DebuggerDisplay("Remove deadline")]
    public class HumanTaskInstanceDeadLineRemovedEvent : DomainEvent
    {
        public HumanTaskInstanceDeadLineRemovedEvent(string id, string aggregateId, int version, string deadLineName, HumanTaskInstanceDeadLineTypes type, DateTime updateDateTime) : base(id, aggregateId, version)
        {
            DeadLineName = deadLineName;
            DeadLineType = type;
            UpdateDateTime = updateDateTime;
        }

        public string DeadLineName { get; set; }
        public HumanTaskInstanceDeadLineTypes DeadLineType { get; set; }
        public DateTime UpdateDateTime { get; set; }
    }
}
