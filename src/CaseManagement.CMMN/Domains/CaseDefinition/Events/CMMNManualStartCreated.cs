using CaseManagement.Workflow.Infrastructure;
using System.Diagnostics;

namespace CaseManagement.CMMN.Domains.CaseInstance.Events
{
    [DebuggerDisplay("Create manual start {AggregateId}")]
    public class CMMNManualStartCreated : DomainEvent
    {
        public CMMNManualStartCreated(string id, string aggregateId, int version, string elementId, int elementVersion) : base(id, aggregateId, version)
        {
            ElementId = elementId;
            ElementVersion = elementVersion;
        }

        public string ElementId { get; set; }
        public int ElementVersion { get; set; }
    }
}
