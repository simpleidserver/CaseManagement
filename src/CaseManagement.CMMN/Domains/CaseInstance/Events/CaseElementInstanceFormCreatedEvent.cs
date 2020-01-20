using CaseManagement.CMMN.Infrastructures;
using System;
using System.Diagnostics;

namespace CaseManagement.CMMN.Domains.Events
{
    [DebuggerDisplay("Create form {FormId} for element {CaseElementId} (Role = {PerformerRef})")]
    public class CaseElementInstanceFormCreatedEvent : DomainEvent
    {
        public CaseElementInstanceFormCreatedEvent(string id, string aggregateId, int version, string elementId, string formInstanceId, string formId, string perfomerRef, DateTime createDateTime) : base(id, aggregateId, version)
        {
            CaseElementId = elementId;
            FormInstanceId = formInstanceId;
            FormId = formId;
            PerformerRef = perfomerRef;
            CreateDateTime = createDateTime;
        }

        public string CaseElementId { get; set; }
        public string FormInstanceId { get; set; }
        public string FormId { get; set; }
        public string PerformerRef { get; set; }
        public DateTime CreateDateTime { get; set; }
    }
}
