using CaseManagement.Common.Domains;
using System;
using System.Diagnostics;

namespace CaseManagement.HumanTask.Domains
{
    [DebuggerDisplay("Assign people")]
    public class HumanTaskPeopleAssignedEvent : DomainEvent
    {
        public HumanTaskPeopleAssignedEvent(
            string id,
            string aggregateId,
            int version,
            PeopleAssignmentDefinition[] peopleAssignments,
            DateTime updateDateTime) : base(id, aggregateId, version)
        {
            PeopleAssignments = peopleAssignments;
            UpdateDateTime = updateDateTime;
        }

        public PeopleAssignmentDefinition[] PeopleAssignments { get; set; }
        public DateTime UpdateDateTime { get; set; }
    }
}
