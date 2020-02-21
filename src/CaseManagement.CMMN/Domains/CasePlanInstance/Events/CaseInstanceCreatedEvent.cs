using CaseManagement.CMMN.Infrastructures;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CaseManagement.CMMN.Domains.Events
{
    [DebuggerDisplay("Create CMMMN {AggregateId} workflow instance from {CasePlanId}")]
    public class CaseInstanceCreatedEvent : DomainEvent
    {
        public CaseInstanceCreatedEvent(string id, string aggregateId, int version, string definitionId, DateTime createDateTime, string performer, ICollection<string> roles) : base(id, aggregateId, version)
        {
            CaseDefinitionId = definitionId;
            CreateDateTime = createDateTime;
            Performer = performer;
            Roles = roles;
        }
        
        public string CaseDefinitionId { get; set; }
        public DateTime CreateDateTime { get; set; }
        public string Performer { get; set; }
        public ICollection<string> Roles { get; set; }
    }
}
