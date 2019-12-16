using CaseManagement.Workflow.Infrastructure;
using System.Diagnostics;

namespace CaseManagement.CMMN.Domains.CaseInstance.Events
{
    [DebuggerDisplay("Create manual start {AggregateId}")]
    public class CMMNManualStartCreated : DomainEvent
    {
        public CMMNManualStartCreated(string id, string aggregateId, int version, string elementId) : base(id, aggregateId, version)
        {
            ElementId = elementId;
        }

        public string ElementId { get; set; }
    }
}
