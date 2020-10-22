using CaseManagement.Common.Domains;
using System;
using System.Diagnostics;

namespace CaseManagement.HumanTask.Domains.HumanTaskDef.Events
{
    [DebuggerDisplay("Add start deadline")]
    public class HumanTaskDefStartDeadLineAddedEvent : DomainEvent
    {
        public HumanTaskDefStartDeadLineAddedEvent(string id, string aggregateId, int version, HumanTaskDefinitionDeadLine deadLine, DateTime updateDateTime) : base(id, aggregateId, version)
        {
            DeadLine = deadLine;
            UpdateDateTime = updateDateTime;
        }

        public HumanTaskDefinitionDeadLine DeadLine { get; set; }
        public DateTime UpdateDateTime { get; set; }
    }
}
