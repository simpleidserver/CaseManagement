using CaseManagement.Common.Domains;
using System;
using System.Diagnostics;

namespace CaseManagement.HumanTask.Domains.HumanTaskDef.Events
{
    [DebuggerDisplay("Presentation element is removed")]
    public class HumanTaskDefPresentationElementRemovedEvent : DomainEvent
    {
        public HumanTaskDefPresentationElementRemovedEvent(string id, string aggregateId, int version, PresentationElementUsages usage, string language, DateTime receptionDateTime) : base(id, aggregateId, version)
        {
            Usage = usage;
            Language = language;
            ReceptionDateTime = receptionDateTime;
        }

        public PresentationElementUsages Usage { get; set; }
        public string Language { get; set; }
        public DateTime ReceptionDateTime { get; set; }
    }
}
