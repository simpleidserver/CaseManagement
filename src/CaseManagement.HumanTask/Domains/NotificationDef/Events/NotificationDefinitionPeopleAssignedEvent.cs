using CaseManagement.Common.Domains;
using System;
using System.Diagnostics;

namespace CaseManagement.HumanTask.Domains
{
    [DebuggerDisplay("People is assigned")]
    public class NotificationDefinitionPeopleAssignedEvent : DomainEvent
    {
        public NotificationDefinitionPeopleAssignedEvent(string id, string aggregateId, int version, PeopleAssignmentDefinition peopleAssignment, DateTime createDateTime) : base(id, aggregateId, version)
        {
            PeopleAssignment = peopleAssignment;
            CreateDateTime = createDateTime;
        }

        public PeopleAssignmentDefinition PeopleAssignment { get; set; }
        public DateTime CreateDateTime { get; set; }
    }
}
