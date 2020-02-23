using CaseManagement.CMMN.Infrastructures;
using System;
using System.Diagnostics;

namespace CaseManagement.CMMN.Domains.Events
{
    [DebuggerDisplay("Create element {CasePlanElementType} instance {CasePlanElementId}")]
    public class CaseElementCreatedEvent : DomainEvent
    {
        public CaseElementCreatedEvent(string id, string aggregateId, int version, string casePlanElementInstanceId, string casePlanElementId, string casePlanElementName, CaseElementTypes casePlanElementType, DateTime createDateTime, string parentId) : base(id, aggregateId, version)
        {
            CasePlanElementInstanceId = casePlanElementInstanceId;
            CasePlanElementId = casePlanElementId;
            CasePlanElementName = casePlanElementName;
            CasePlanElementType = casePlanElementType;
            CreateDateTime = createDateTime;
            ParentId = parentId;
        }

        public string CasePlanElementInstanceId { get; set; }
        public string CasePlanElementId { get; set; }
        public string CasePlanElementName { get; set; }
        public CaseElementTypes CasePlanElementType { get; set; }
        public DateTime CreateDateTime { get; set; }
        public string ParentId { get; set; }
    }
}
