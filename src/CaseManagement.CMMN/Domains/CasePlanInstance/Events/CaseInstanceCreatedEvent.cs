using CaseManagement.CMMN.Infrastructures;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CaseManagement.CMMN.Domains.Events
{
    [DebuggerDisplay("Create CMMMN {AggregateId} workflow instance from {CasePlanId}")]
    public class CaseInstanceCreatedEvent : DomainEvent
    {
        public CaseInstanceCreatedEvent(string id, string aggregateId, int version, string casePlanId, string casePlanName, DateTime createDateTime, string performer, ICollection<string> roles) : base(id, aggregateId, version)
        {
            CasePlanId = casePlanId;
            CasePlanName = casePlanName;
            CreateDateTime = createDateTime;
            Performer = performer;
            Roles = roles;
        }
        
        public string CasePlanId { get; set; }
        public string CasePlanName { get; set; }
        public DateTime CreateDateTime { get; set; }
        public string Performer { get; set; }
        public ICollection<string> Roles { get; set; }
    }
}
