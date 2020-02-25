using CaseManagement.CMMN.Infrastructures;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CaseManagement.CMMN.Domains.Events
{
    [DebuggerDisplay("Form '{AggregateId}' is updated")]
    public class FormUpdatedEvent : DomainEvent
    {
        public FormUpdatedEvent(string id, string aggregateId, int version, DateTime updateDateTime, ICollection<Translation> titles, ICollection<FormElement> elements) : base(id, aggregateId, version)
        {
            UpdateDateTime = updateDateTime;
            Titles = titles;
            Elements = elements;
        }

        public DateTime UpdateDateTime { get; set; }
        public ICollection<Translation> Titles { get; set; }
        public ICollection<FormElement> Elements { get; set; }
    }
}
