using CaseManagement.CMMN.Infrastructures;
using System;
using System.Diagnostics;

namespace CaseManagement.CMMN.Domains.Events
{
    [DebuggerDisplay("Create element {CaseElementDefinitionType} instance {CaseElementId}")]
    public class CaseElementCreatedEvent : DomainEvent
    {
        public CaseElementCreatedEvent(string id, string aggregateId, int version, string elementId, string workflowElementDefinitionId, CaseElementTypes workflowElementDefinitionType, DateTime createDateTime, string parentId) : base(id, aggregateId, version)
        {
            CaseElementId = elementId;
            CaseElementDefinitionId = workflowElementDefinitionId;
            CaseElementDefinitionType = workflowElementDefinitionType;
            CreateDateTime = createDateTime;
            ParentId = parentId;
        }

        public string CaseElementId { get; set; }
        public string CaseElementDefinitionId { get; set; }
        public CaseElementTypes CaseElementDefinitionType { get; set; }
        public DateTime CreateDateTime { get; set; }
        public string ParentId { get; set; }
    }
}
