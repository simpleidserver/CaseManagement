using CaseManagement.Workflow.Infrastructure;
using System;
using System.Diagnostics;

namespace CaseManagement.CMMN.Domains.Events
{
    [DebuggerDisplay("Create PlanItem {PlanItemDefinitionId} instance {ElementId}")]
    public class CMMNPlanItemInstanceCreatedEvent : DomainEvent
    {
        public CMMNPlanItemInstanceCreatedEvent(string id, string aggregateId, int version, string elementId, string planItemDefinitionId, CMMNPlanItemDefinitionTypes planItemDefinitionType, DateTime createDateTime) : base(id, aggregateId, version)
        {
            ElementId = elementId;
            PlanItemDefinitionId = planItemDefinitionId;
            PlanItemDefinitionType = planItemDefinitionType;
            CreateDateTime = createDateTime;
        }

        public string ElementId { get; set; }
        public string PlanItemDefinitionId { get; set; }
        public CMMNPlanItemDefinitionTypes PlanItemDefinitionType { get; set; }
        public DateTime CreateDateTime { get; set; }
    }
}
