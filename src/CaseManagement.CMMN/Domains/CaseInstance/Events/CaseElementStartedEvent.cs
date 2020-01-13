using CaseManagement.Workflow.Infrastructure;
using System;
using System.Diagnostics;

namespace CaseManagement.CMMN.Domains.Events
{
    [DebuggerDisplay("Start the element {CaseElementDefinitionId}")]
    public class CaseElementStartedEvent : DomainEvent
    {
        public CaseElementStartedEvent(string id, string aggregateId, int version, string elementDefinitionId, DateTime startDateTime ) : base(id, aggregateId, version)
        {
            CaseElementDefinitionId = elementDefinitionId;
            StartDateTime = startDateTime;
        }

        public string CaseElementDefinitionId { get; set; }
        public DateTime StartDateTime { get; set; }
    }
}
