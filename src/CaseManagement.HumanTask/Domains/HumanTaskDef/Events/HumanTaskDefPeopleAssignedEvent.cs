using CaseManagement.Common.Domains;
using System;
using System.Diagnostics;

namespace CaseManagement.HumanTask.Domains.HumanTaskDef.Events
{
    [DebuggerDisplay("People is assigned")]
    public class HumanTaskDefPeopleAssignedEvent : DomainEvent
    {
        public HumanTaskDefPeopleAssignedEvent(string id, string aggregateId, int version, PeopleAssignmentDefinition peopleAssignment, DateTime createDateTime) : base(id, aggregateId, version)
        {
            PeopleAssignment = peopleAssignment;
            CreateDateTime = createDateTime;
        }

        public PeopleAssignmentDefinition PeopleAssignment { get; set; }
        public DateTime CreateDateTime { get; set; }
    }
}
