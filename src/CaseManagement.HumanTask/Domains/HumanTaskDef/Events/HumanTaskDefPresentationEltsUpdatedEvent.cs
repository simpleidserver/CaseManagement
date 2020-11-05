using CaseManagement.Common.Domains;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CaseManagement.HumanTask.Domains
{
    [DebuggerDisplay("Update presentation elements")]
    public class HumanTaskDefPresentationEltsUpdatedEvent : DomainEvent
    {
        public HumanTaskDefPresentationEltsUpdatedEvent(string id, string aggregateId, int version, IEnumerable<PresentationElementDefinition> presentationElts, DateTime updateDateTime) : base(id, aggregateId, version)
        {
            PresentationElts = presentationElts;
            UpdateDateTime = updateDateTime;
        }

        public IEnumerable<PresentationElementDefinition> PresentationElts { get; set; }
        public DateTime UpdateDateTime { get; set; }
    }
}
