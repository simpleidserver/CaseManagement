using CaseManagement.CMMN.Infrastructures;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CaseManagement.CMMN.Domains.FormInstance.Events
{
    [DebuggerDisplay("Submit form {AggregateId}")]
    public class FormInstanceSubmittedEvent : DomainEvent
    {
        public FormInstanceSubmittedEvent(string id, string aggregateId, int version, Dictionary<string, string> content, DateTime updateDateTime) : base(id, aggregateId, version)
        {
            Content = content;
            UpdateDateTime = updateDateTime;
        }

        public Dictionary<string, string> Content { get; set; }
        public DateTime UpdateDateTime { get; set; }
    }
}