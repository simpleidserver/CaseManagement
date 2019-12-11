using CaseManagement.Workflow.Domains;
using CaseManagement.Workflow.Infrastructure;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CaseManagement.CMMN.Domains.CaseInstance.Events
{
    [DebuggerDisplay("Create CMMN process {AggregateId}")]
    public class CMMNProcessInstanceCreatedEvent : DomainEvent
    {
        public CMMNProcessInstanceCreatedEvent(string id, string aggregateId, int version, string processFlowTemplateId, string processFlowName, DateTime createDateTime, ICollection<CMMNPlanItem> planItems, ICollection<CMMNCaseFileItem> fileItems, ICollection<ProcessFlowConnector> connectors) : base(id, aggregateId, version)
        {
            ProcessFlowTemplateId = processFlowTemplateId;
            ProcessFlowName = processFlowName;
            CreateDateTime = createDateTime;
            PlanItems = planItems;
            FileItems = fileItems;
            Connectors = connectors;
        }

        public string ProcessFlowTemplateId { get; set; }
        public string ProcessFlowName { get; set; }
        public DateTime CreateDateTime { get; set; }
        public ICollection<CMMNPlanItem> PlanItems { get; set; }
        public ICollection<CMMNCaseFileItem> FileItems { get; set; }
        public ICollection<ProcessFlowConnector> Connectors { get; set; }
    }
}
