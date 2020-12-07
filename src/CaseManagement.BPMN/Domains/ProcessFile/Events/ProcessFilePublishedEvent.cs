using CaseManagement.Common.Domains;
using System;
using System.Diagnostics;

namespace CaseManagement.BPMN.Domains.ProcessDefinition.Events
{
    [DebuggerDisplay("Publish process definition")]
    [Serializable]
    public class ProcessFilePublishedEvent : DomainEvent
    {
        public ProcessFilePublishedEvent(string id, string aggregateId, int version, DateTime publishedDateTime) : base(id, aggregateId, version)
        {
            PublishedDateTime = publishedDateTime;
        }

        public DateTime PublishedDateTime { get; set; }
    }
}
