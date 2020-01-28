using CaseManagement.CMMN.Infrastructures;
using System;
using System.Diagnostics;

namespace CaseManagement.CMMN.Domains.Events
{
    [DebuggerDisplay("Case element {CaseElementDefinitionId} is planned")]
    public class CaseElementPlannedEvent : DomainEvent
    {
        public CaseElementPlannedEvent(string id, string aggregateId, int version, string caseElementDefinitionId, string userRole, DateTime createDateTime) : base(id, aggregateId, version)
        {
            CaseElementDefinitionId = caseElementDefinitionId;
            UserRole = userRole;
            CreateDateTime = createDateTime;
        }

        public string CaseElementDefinitionId { get; set; }
        public string UserRole { get; set; }
        public DateTime CreateDateTime { get; set; }
    }
}
