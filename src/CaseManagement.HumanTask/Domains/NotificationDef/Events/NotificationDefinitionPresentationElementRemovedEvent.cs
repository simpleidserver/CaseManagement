using CaseManagement.Common.Domains;
using System;
using System.Diagnostics;

namespace CaseManagement.HumanTask.Domains
{
    [DebuggerDisplay("Presentation element is removed")]
    public class NotificationDefinitionPresentationElementRemovedEvent : DomainEvent
    {
        public NotificationDefinitionPresentationElementRemovedEvent(string id, string aggregateId, int version, PresentationElementUsages usage, string language, DateTime receptionDateTime) : base(id, aggregateId, version)
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
