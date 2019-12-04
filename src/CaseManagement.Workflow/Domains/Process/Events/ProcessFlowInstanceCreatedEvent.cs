using CaseManagement.Workflow.Infrastructure;
using System;
using System.Collections.Generic;

namespace CaseManagement.Workflow.Domains.Events
{
    public class ProcessFlowInstanceCreatedEvent : DomainEvent
    {
        public ProcessFlowInstanceCreatedEvent(string id, string aggregateId, int version, string processFlowTemplateId, string processFlowName, DateTime createDateTime, ICollection<ProcessFlowInstanceElement> elements, ICollection<ProcessFlowConnector> connectors) : base(id, aggregateId, version)
        {
            ProcessFlowTemplateId = processFlowTemplateId;
            ProcessFlowName = processFlowName;
            CreateDateTime = createDateTime;
            Elements = elements;
            Connectors = connectors;
        }

        public string ProcessFlowTemplateId { get; set; }
        public string ProcessFlowName { get; set; }
        public DateTime CreateDateTime { get; set; }
        public ICollection<ProcessFlowInstanceElement> Elements { get; set; }
        public ICollection<ProcessFlowConnector> Connectors { get; set; }
    }
}
