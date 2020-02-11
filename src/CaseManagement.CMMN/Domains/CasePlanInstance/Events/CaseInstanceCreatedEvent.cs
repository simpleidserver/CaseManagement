using CaseManagement.CMMN.Infrastructures;
using System;
using System.Diagnostics;

namespace CaseManagement.CMMN.Domains.Events
{
    [DebuggerDisplay("Create CMMMN {AggregateId} workflow instance from {CasePlanId}")]
    public class CaseInstanceCreatedEvent : DomainEvent
    {
        public CaseInstanceCreatedEvent(string id, string aggregateId, int version, string definitionId, DateTime createDateTime) : base(id, aggregateId, version)
        {
            CaseDefinitionId = definitionId;
            CreateDateTime = createDateTime;
        }
        
        public string CaseDefinitionId { get; set; }
        public DateTime CreateDateTime { get; set; }
    }
}
