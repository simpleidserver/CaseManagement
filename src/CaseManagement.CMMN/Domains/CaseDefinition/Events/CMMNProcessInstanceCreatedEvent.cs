using CaseManagement.Workflow.Infrastructure;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CaseManagement.CMMN.Domains.CaseInstance.Events
{
    [DebuggerDisplay("Create CMMN process {AggregateId}")]
    public class CMMNProcessInstanceCreatedEvent : DomainEvent
    {
        public CMMNProcessInstanceCreatedEvent(string id, string aggregateId, int version, string processFlowTemplateId, string processFlowName, DateTime createDateTime, ICollection<CMMNPlanItemDefinition> planItems) : base(id, aggregateId, version)
        {
            ProcessFlowTemplateId = processFlowTemplateId;
            ProcessFlowName = processFlowName;
            CreateDateTime = createDateTime;
            PlanItems = planItems;
        }

        public string ProcessFlowTemplateId { get; set; }
        public string ProcessFlowName { get; set; }
        public DateTime CreateDateTime { get; set; }
        public ICollection<CMMNPlanItemDefinition> PlanItems { get; set; }
        // public ICollection<CMMNCaseFileItem> FileItems { get; set; }
        // public ICollection<ProcessFlowConnector> Connectors { get; set; }
    }
}
