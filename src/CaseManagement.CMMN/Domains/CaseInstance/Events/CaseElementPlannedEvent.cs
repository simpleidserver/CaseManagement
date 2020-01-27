using CaseManagement.CMMN.Infrastructures;
using System.Collections.Generic;
using System.Diagnostics;

namespace CaseManagement.CMMN.Domains.Events
{
    [DebuggerDisplay("Case element {CaseElementDefinitionId} is planned")]
    public class CaseElementPlannedEvent : DomainEvent
    {
        public CaseElementPlannedEvent(string id, string aggregateId, int version, string caseElementDefinitionId, IEnumerable<string> userRoles) : base(id, aggregateId, version)
        {
            CaseElementDefinitionId = caseElementDefinitionId;
            UserRoles = userRoles;
        }

        public string CaseElementDefinitionId { get; set; }
        public IEnumerable<string> UserRoles { get; set; }
    }
}
