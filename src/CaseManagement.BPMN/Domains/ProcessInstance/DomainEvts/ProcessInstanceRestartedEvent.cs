using CaseManagement.Common.Domains;
using System;
using System.Diagnostics;

namespace CaseManagement.BPMN.Domains
{
    [DebuggerDisplay("Restart process instance")]
    [Serializable]
    public class ProcessInstanceRestartedEvent : DomainEvent
    {
        public ProcessInstanceRestartedEvent(string id, string aggregateId, int version, DateTime updateDateTime) : base(id, aggregateId, version)
        {
            UpdateDateTime = updateDateTime;
        }

        public DateTime UpdateDateTime { get; set; }
    }
}
