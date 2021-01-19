using CaseManagement.Common.Domains;
using System;
using System.Diagnostics;

namespace CaseManagement.HumanTask.Domains
{
    [DebuggerDisplay("Add presentation parameter")]
    public class NotificationDefinitionPresentationElementAddedEvent : DomainEvent
    {
        public NotificationDefinitionPresentationElementAddedEvent(string id, string aggregateId, int version, PresentationElementDefinition presentationElement, DateTime receptionDate) : base(id, aggregateId, version)
        {
            PresentationElement = presentationElement;
            ReceptionDate = receptionDate;
        }

        public PresentationElementDefinition PresentationElement { get; set; }
        public DateTime ReceptionDate { get; set; }
    }
}
