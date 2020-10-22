using CaseManagement.Common.Domains;
using System;
using System.Diagnostics;

namespace CaseManagement.HumanTask.Domains.HumanTaskDef.Events
{
    [DebuggerDisplay("Add completion deadline")]
    public class HumanTaskDefCompletionDeadLineAddedEvent : DomainEvent
    {
        public HumanTaskDefCompletionDeadLineAddedEvent(string id, string aggregateId, int version, HumanTaskDefinitionDeadLine deadLine, DateTime updateDateTime) : base(id, aggregateId, version)
        {
            DeadLine = deadLine;
            UpdateDateTime = updateDateTime;
        }

        public HumanTaskDefinitionDeadLine DeadLine { get; set; }
        public DateTime UpdateDateTime { get; set; }
    }
}
