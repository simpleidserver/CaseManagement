using CaseManagement.Workflow.Infrastructure;
using CaseManagement.Workflow.Infrastructure.EvtBus;
using System;
using System.Collections.Generic;

namespace CaseManagement.Workflow.Domains.Events
{
    public class ProcessFlowInstanceCreatedEvent : DomainEvent
    {
        public ProcessFlowInstanceCreatedEvent(string id, DateTime createDateTime, ICollection<ProcessFlowInstanceElement> elements, ICollection<ProcessFlowConnector> connectors)
        {
            Id = id;
            CreateDateTime = createDateTime;
            Elements = elements;
            Connectors = connectors;
        }

        public string Id { get; set; }
        public DateTime CreateDateTime { get; set; }
        public ICollection<ProcessFlowInstanceElement> Elements { get; set; }
        public ICollection<ProcessFlowConnector> Connectors { get; set; }
    }
}
