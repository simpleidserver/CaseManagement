using CaseManagement.Common.Domains;
using System;
using System.Diagnostics;

namespace CaseManagement.CMMN.Domains
{
    [Serializable]
    [DebuggerDisplay("add worker task")]
    public class CaseInstanceWorkerTaskAddedEvent : DomainEvent
    {
        public CaseInstanceWorkerTaskAddedEvent(string id, string aggregateId, int version, string casePlanInstanceElementId, string externalId, DateTime createDateTime) : base(id, aggregateId, version)
        {
            CasePlanInstanceElementId = casePlanInstanceElementId;
            ExternalId = externalId;
            CreateDateTime = createDateTime;
        }

        public string CasePlanInstanceElementId { get; set; }
        public string ExternalId { get; set; }
        public DateTime CreateDateTime { get; set; }
    }
}
