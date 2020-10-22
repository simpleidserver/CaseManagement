using CaseManagement.Common.Domains;
using System;
using System.Diagnostics;

namespace CaseManagement.HumanTask.Domains
{
    [DebuggerDisplay("Remove output parameter")]
    public class HumanTaskDefOutputParameterRemovedEvent : DomainEvent
    {
        public HumanTaskDefOutputParameterRemovedEvent(string id, string aggregateId, int version, string parameterName, DateTime updateDateTime) : base(id, aggregateId, version)
        {
            ParameterName = parameterName;
            UpdateDateTime = updateDateTime;
        }

        public string ParameterName { get; set; }
        public DateTime UpdateDateTime { get; set; }
    }
}
