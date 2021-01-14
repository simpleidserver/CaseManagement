using CaseManagement.Common.Domains;
using System;
using System.Diagnostics;

namespace CaseManagement.HumanTask.Domains.HumanTaskDef.Events
{
    [DebuggerDisplay("Add presentation element")]
    public class HumanTaskDefPresentationElementAddedEvent : DomainEvent
    {
        public HumanTaskDefPresentationElementAddedEvent(string id, string aggregateId, int version, PresentationElementDefinition presentationElement, DateTime receptionDate) : base(id, aggregateId, version)
        {
            PresentationElement = presentationElement;
            ReceptionDate = receptionDate;
        }

        public PresentationElementDefinition PresentationElement { get; set; }
        public DateTime ReceptionDate { get; set; }
    }
}
