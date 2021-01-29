using CaseManagement.Common.Domains;
using System;
using System.Diagnostics;

namespace CaseManagement.HumanTask.Domains.HumanTaskDef.Events
{
    [DebuggerDisplay("Update rendering")]
    public class HumanTaskDefRenderingUpdatedEvent : DomainEvent
    {
        public HumanTaskDefRenderingUpdatedEvent(string id, string aggregateId, int version, string rendering, DateTime updateDateTime) : base(id, aggregateId, version)
        {
            Rendering = rendering;
            UpdateDateTime = updateDateTime;
        }

        public string Rendering { get; set; }
        public DateTime UpdateDateTime { get; set; }
    }
}
