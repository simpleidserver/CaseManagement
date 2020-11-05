using CaseManagement.Common.Domains;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CaseManagement.HumanTask.Domains.HumanTaskDef.Events
{
    [DebuggerDisplay("Update rendering")]
    public class HumanTaskDefRenderingUpdatedEvent : DomainEvent
    {
        public HumanTaskDefRenderingUpdatedEvent(string id, string aggregateId, int version, ICollection<RenderingElement> renderingElements, DateTime updateDateTime) : base(id, aggregateId, version)
        {
            RenderingElements = renderingElements;
            UpdateDateTime = updateDateTime;
        }

        public ICollection<RenderingElement> RenderingElements { get; set; }
        public DateTime UpdateDateTime { get; set; }
    }
}
