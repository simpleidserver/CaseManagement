using CaseManagement.CMMN.Infrastructures;
using System;
using System.Diagnostics;

namespace CaseManagement.CMMN.Domains.Events
{
    [DebuggerDisplay("Finish the element {CaseElementDefinitionId}")]
    public class CaseElementFinishedEvent : DomainEvent
    {
        public CaseElementFinishedEvent(string id, string aggregateId, int version, string elementDefinitionId, DateTime endDateTime) : base(id, aggregateId, version)
        {
            CaseElementDefinitionId = elementDefinitionId;
            EndDateTime = endDateTime;
        }

        public string CaseElementDefinitionId { get; set; }
        public DateTime EndDateTime { get; set; }
    }
}
