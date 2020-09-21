using CaseManagement.Common.Domains;
using System;
using System.Diagnostics;

namespace CaseManagement.CMMN.Domains
{
    [Serializable]
    [DebuggerDisplay("add worker task")]
    public class CaseInstanceWorkerTaskAddedEvent : DomainEvent
    {
        public CaseInstanceWorkerTaskAddedEvent(string id, string aggregateId, int version, string casePlanInstanceElementId, DateTime createDateTime, CasePlanInstanceRole caseOwnerRole) : base(id, aggregateId, version)
        {
            CasePlanInstanceElementId = casePlanInstanceElementId;
            CreateDateTime = createDateTime;
            CaseOwnerRole = caseOwnerRole;
        }

        public string CasePlanInstanceElementId { get; set; }
        public DateTime CreateDateTime { get; set; }
        public CasePlanInstanceRole CaseOwnerRole { get; set; }
    }
}
