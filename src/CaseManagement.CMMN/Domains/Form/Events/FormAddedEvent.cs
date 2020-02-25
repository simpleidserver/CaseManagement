using CaseManagement.CMMN.Infrastructures;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CaseManagement.CMMN.Domains.Events
{
    [DebuggerDisplay("Form '{AggregateId}' is added")]
    public class FormAddedEvent : DomainEvent
    {
        public FormAddedEvent(string id, string aggregateId, int version, DateTime createDateTime, string formId, ICollection<Translation> titles, ICollection<FormElement> elements) : base(id, aggregateId, version)
        {
            CreateDateTime = createDateTime;
            FormId = formId;
            Titles = titles;
            Elements = elements;
        }

        public DateTime CreateDateTime { get; set; }
        public string FormId { get; set; }
        public ICollection<Translation> Titles { get; set; }
        public ICollection<FormElement> Elements { get; set; }
    }
}
