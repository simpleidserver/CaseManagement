using CaseManagement.CMMN.Infrastructures;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CaseManagement.CMMN.Domains.Events
{
    [DebuggerDisplay("Submit form {FormInstanceId}")]
    public class CaseElementInstanceFormSubmittedEvent : DomainEvent
    {
        public CaseElementInstanceFormSubmittedEvent(string id, string aggregateId, int version, string elementId, string formInstanceId, Dictionary<string, string> formValues, DateTime updateDateTime) : base(id, aggregateId, version)
        {
            CaseElementId = elementId;
            FormInstanceId = formInstanceId;
            FormValues = formValues;
            UpdatedDateTime = updateDateTime;
        }

        public string CaseElementId { get; set; }
        public string FormInstanceId { get; set; }
        public Dictionary<string, string> FormValues { get; set; }
        public DateTime UpdatedDateTime { get; set; }
    }
}
