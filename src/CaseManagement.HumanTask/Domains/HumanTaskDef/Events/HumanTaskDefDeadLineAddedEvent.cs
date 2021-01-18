using CaseManagement.Common.Domains;
using System;
using System.Diagnostics;

namespace CaseManagement.HumanTask.Domains
{
    [DebuggerDisplay("Add completion deadline")]
    public class HumanTaskDefDeadLineAddedEvent : DomainEvent
    {
        public HumanTaskDefDeadLineAddedEvent(string id, string aggregateId, int version, HumanTaskDefinitionDeadLine deadLine, DateTime updateDateTime) : base(id, aggregateId, version)
        {
            DeadLine = deadLine;
            UpdateDateTime = updateDateTime;
        }

        public HumanTaskDefinitionDeadLine DeadLine { get; set; }
        public DateTime UpdateDateTime { get; set; }
    }
}
