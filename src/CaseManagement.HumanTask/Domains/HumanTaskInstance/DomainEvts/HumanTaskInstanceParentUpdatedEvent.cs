using CaseManagement.Common.Domains;
using System;
using System.Diagnostics;

namespace CaseManagement.HumanTask.Domains
{
    [DebuggerDisplay("Set parent")]
    public class HumanTaskInstanceParentUpdatedEvent : DomainEvent
    {
        public HumanTaskInstanceParentUpdatedEvent(string id, string aggregateId, int version, string parentName, string parentId, DateTime updateDateTime) : base(id, aggregateId, version)
        {
            ParentName = parentName;
            ParentId = parentId;
            UpdateDateTime = updateDateTime;
        }

        public string ParentName { get; set; }
        public string ParentId { get; set; }
        public DateTime UpdateDateTime { get; set; }
    }
}
