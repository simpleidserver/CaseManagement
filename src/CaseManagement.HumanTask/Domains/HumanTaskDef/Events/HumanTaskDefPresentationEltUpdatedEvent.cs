using CaseManagement.Common.Domains;
using System;
using System.Diagnostics;

namespace CaseManagement.HumanTask.Domains
{
    [DebuggerDisplay("Update presentation element")]
    public class HumanTaskDefPresentationEltUpdatedEvent : DomainEvent
    {
        public HumanTaskDefPresentationEltUpdatedEvent(string id, string aggregateId, int version, PresentationElementDefinition presentationElt, DateTime updateDateTime) : base(id, aggregateId, version)
        {
            PresentationElt = presentationElt;
            UpdateDateTime = updateDateTime;
        }

        public PresentationElementDefinition PresentationElt { get; set; }
        public DateTime UpdateDateTime { get; set; }
    }
}
