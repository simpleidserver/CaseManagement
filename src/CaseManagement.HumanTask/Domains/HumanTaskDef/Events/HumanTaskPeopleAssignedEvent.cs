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
            PeopleAssignmentDefinition potentialOwner,
            PeopleAssignmentDefinition excludedOwner,
            PeopleAssignmentDefinition taskInitiator,
            PeopleAssignmentDefinition taskStakeHolder,
            PeopleAssignmentDefinition businessAdministrator,
            PeopleAssignmentDefinition recipient,
            DateTime updateDateTime) : base(id, aggregateId, version)
        {
            PotentialOwner = potentialOwner;
            ExcludedOwner = excludedOwner;
            TaskInitiator = taskInitiator;
            TaskStakeHolder = taskStakeHolder;
            BusinessAdministrator = businessAdministrator;
            Recipient = recipient;
            UpdateDateTime = updateDateTime;
        }

        public PeopleAssignmentDefinition PotentialOwner { get; set; }
        public PeopleAssignmentDefinition ExcludedOwner { get; set; }
        public PeopleAssignmentDefinition TaskInitiator { get; set; }
        public PeopleAssignmentDefinition TaskStakeHolder { get; set; }
        public PeopleAssignmentDefinition BusinessAdministrator { get; set; }
        public PeopleAssignmentDefinition Recipient { get; set; }
        public DateTime UpdateDateTime { get; set; }
    }
}
