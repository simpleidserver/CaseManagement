using CaseManagement.CMMN.Infrastructures;
using System;
using System.Diagnostics;

namespace CaseManagement.CMMN.Domains.Events
{
    [DebuggerDisplay("Confirm case element {CaseElementDefinitionId} planning")]
    public class CaseElementPlanificationConfirmedEvent : DomainEvent
    {
        public CaseElementPlanificationConfirmedEvent(string id, string aggregateId, int version, string caseElementDefinitionId, string user, DateTime createDateTime) : base(id, aggregateId, version)
        {
            CaseElementDefinitionId = caseElementDefinitionId;
            User = user;
            CreateDateTime = createDateTime;
        }

        public string CaseElementDefinitionId { get; set; }
        public string User { get; set; }
        public DateTime CreateDateTime { get; set; }
    }
}
