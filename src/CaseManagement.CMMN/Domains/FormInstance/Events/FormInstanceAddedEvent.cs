using CaseManagement.CMMN.Infrastructures;
using System;
using System.Diagnostics;

namespace CaseManagement.CMMN.Domains.FormInstance.Events
{
    [DebuggerDisplay("Create form {AggregateId} for element {CaseElementInstanceId} (Role = {PerformerRole})")]
    public class FormInstanceAddedEvent : DomainEvent
    {
        public FormInstanceAddedEvent(string id, string aggregateId, int version, string formId, DateTime createDateTime, string performerRole, string caseElementInstanceId, string casePlanInstanceId) : base(id, aggregateId, version)
        {
            FormId = formId;
            CreateDateTime = createDateTime;
            PerformerRole = performerRole;
            CaseElementInstanceId = caseElementInstanceId;
            CasePlanInstanceId = casePlanInstanceId;
        }

        public string FormId { get; set; }
        public DateTime CreateDateTime { get; set; }
        public string PerformerRole { get; set; }
        public string CasePlanInstanceId { get; set; }
        public string CaseElementInstanceId { get; set; }
    }
}