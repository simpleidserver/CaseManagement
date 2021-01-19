using CaseManagement.Common.Domains;
using System;
using System.Diagnostics;

namespace CaseManagement.HumanTask.Domains
{
    [DebuggerDisplay("Presentation parameter is added")]
    public class NotificationDefinitionPresentationParameterAddedEvent : DomainEvent
    {
        public NotificationDefinitionPresentationParameterAddedEvent(string id, string aggregateId, int version, PresentationParameter presentationParameter, DateTime createDateTime) : base(id, aggregateId, version)
        {
            PresentationParameter = presentationParameter;
            CreateDateTime = createDateTime;
        }

        public PresentationParameter PresentationParameter { get; set; }
        public DateTime CreateDateTime { get; set; }
    }
}
