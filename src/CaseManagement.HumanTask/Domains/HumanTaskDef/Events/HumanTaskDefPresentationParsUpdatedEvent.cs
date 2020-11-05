using CaseManagement.Common.Domains;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CaseManagement.HumanTask.Domains.HumanTaskDef.Events
{
    [DebuggerDisplay("Update presentation parameters")]
    public class HumanTaskDefPresentationParsUpdatedEvent : DomainEvent
    {
        public HumanTaskDefPresentationParsUpdatedEvent(string id, string aggregateId, int version, ICollection<PresentationParameter> presentationParameters, DateTime updateDateTime) : base(id, aggregateId, version)
        {
            PresentationParameters = presentationParameters;
            UpdateDateTime = updateDateTime;
        }

        public ICollection<PresentationParameter> PresentationParameters { get; set; }
        public DateTime UpdateDateTime { get; set; }
    }
}
