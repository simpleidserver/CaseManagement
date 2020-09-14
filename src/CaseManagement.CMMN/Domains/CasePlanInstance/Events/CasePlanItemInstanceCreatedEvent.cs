using System;
using System.Diagnostics;

namespace CaseManagement.CMMN.Domains
{
    [Serializable]
    [DebuggerDisplay("Add new case plan instance '{CasePlanItemInstanceId}'")]
    public class CasePlanItemInstanceCreatedEvent: DomainEvent
    {
        public CasePlanItemInstanceCreatedEvent(string id, string aggregateId, int version, string casePlanItemInstanceId, DateTime updateDateTime) : base(id, aggregateId, version)
        {
            CasePlanItemInstanceId = casePlanItemInstanceId;
            UpdateDateTime = updateDateTime;
        }

        public string CasePlanItemInstanceId { get; set; }
        public DateTime UpdateDateTime { get; set; }
    }
}
