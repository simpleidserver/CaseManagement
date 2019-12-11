using CaseManagement.Workflow.Infrastructure;
using System.Collections.Generic;
using System.Diagnostics;

namespace CaseManagement.CMMN.Domains.CaseInstance.Events
{
    [DebuggerDisplay("Create CaseFileItem {AggregateId}")]
    public class CMMNCaseFileItemCreatedEvent : DomainEvent
    {
        public CMMNCaseFileItemCreatedEvent(string id, string aggregateId, int version, string elementId, Dictionary<string, string> metadataLst) : base(id, aggregateId, version)
        {
            ElementId = elementId;
            MetadataLst = metadataLst;
        }

        public string ElementId { get; set; }
        public Dictionary<string, string> MetadataLst { get; set; }
    }
}
