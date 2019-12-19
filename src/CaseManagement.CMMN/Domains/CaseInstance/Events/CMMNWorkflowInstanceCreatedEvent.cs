using CaseManagement.Workflow.Infrastructure;
using System;
using System.Diagnostics;

namespace CaseManagement.CMMN.Domains.Events
{
    [DebuggerDisplay("Create CMMMN {AggregateId} workflow instance from {DefinitionId}")]
    public class CMMNWorkflowInstanceCreatedEvent : DomainEvent
    {
        public CMMNWorkflowInstanceCreatedEvent(string id, string aggregateId, int version, string definitionId, DateTime createDateTime) : base(id, aggregateId, version)
        {
            DefinitionId = definitionId;
            CreateDateTime = createDateTime;
        }
        
        public string DefinitionId { get; set; }
        public DateTime CreateDateTime { get; set; }
    }
}
