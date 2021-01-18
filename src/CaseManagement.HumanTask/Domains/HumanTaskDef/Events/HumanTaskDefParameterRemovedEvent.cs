using CaseManagement.Common.Domains;
using System;
using System.Diagnostics;

namespace CaseManagement.HumanTask.Domains
{
    [DebuggerDisplay("Remove parameter")]
    public class HumanTaskDefParameterRemovedEvent : DomainEvent
    {
        public HumanTaskDefParameterRemovedEvent(string id, string aggregateId, int version, string parameterId, DateTime updateDateTime) : base(id, aggregateId, version)
        {
            ParameterId = parameterId;
            UpdateDateTime = updateDateTime;
        }

        public string ParameterId { get; set; }
        public DateTime UpdateDateTime { get; set; }
    }
}
