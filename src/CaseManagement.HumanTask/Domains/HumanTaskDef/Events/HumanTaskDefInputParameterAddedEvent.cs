using CaseManagement.Common.Domains;
using System;
using System.Diagnostics;

namespace CaseManagement.HumanTask.Domains
{
    [DebuggerDisplay("Add input parameter")]
    public class HumanTaskDefInputParameterAddedEvent : DomainEvent
    {
        public HumanTaskDefInputParameterAddedEvent(string id, string aggregateId, int version, Parameter parameter, DateTime updateDateTime) : base(id, aggregateId, version)
        {
            Parameter = parameter;
            UpdateDateTime = updateDateTime;
        }

        public Parameter Parameter { get; set; }
        public DateTime UpdateDateTime { get; set; }
    }
}
