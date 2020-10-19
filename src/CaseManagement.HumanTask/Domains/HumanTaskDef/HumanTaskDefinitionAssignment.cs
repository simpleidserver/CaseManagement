namespace CaseManagement.HumanTask.Domains
{
    public class HumanTaskDefinitionAssignment
    {
        public HumanTaskDefinitionAssignment()
        {
        }

        /// <summary>
        /// Potential owners of a task are persons who receive the task so that they can claim and complete it.
        /// A potential owner becomes the actual owner of a task by explicitly claiming it.
        /// </summary>
        public PeopleAssignmentDefinition PotentialOwner { get; set; }
        /// <summary>
        /// Excluded owners are are people who cannot become an actual or potential owner and thus they cannot reserve or start the task.
        /// </summary>
        public PeopleAssignmentDefinition ExcludedOwner { get; set; }
        /// <summary>
        /// Is the person who creates the task instance.
        /// </summary>
        public PeopleAssignmentDefinition TaskInitiator { get; set; }
        /// <summary>
        /// The task stakeholders are the people ultimately responsible for the oversight and outcome of the task instance
        /// </summary>
        public PeopleAssignmentDefinition TaskStakeHolder { get; set; }
        /// <summary>
        /// Business administrators play the same role as task stakeholders but at task definition level.
        /// </summary>
        public PeopleAssignmentDefinition BusinessAdministrator { get; set; }
        /// <summary>
        /// Notification recipients are persons who receive the notification.
        /// </summary>
        public PeopleAssignmentDefinition Recipient { get; set; }

        public object Clone()
        {
            return new HumanTaskDefinitionAssignment
            {
                PotentialOwner = (PeopleAssignmentDefinition)PotentialOwner?.Clone(),
                ExcludedOwner = (PeopleAssignmentDefinition)ExcludedOwner?.Clone(),
                TaskInitiator = (PeopleAssignmentDefinition)TaskInitiator?.Clone(),
                TaskStakeHolder = (PeopleAssignmentDefinition)TaskStakeHolder?.Clone(),
                BusinessAdministrator = (PeopleAssignmentDefinition)BusinessAdministrator?.Clone(),
                Recipient = (PeopleAssignmentDefinition)Recipient?.Clone()
            };
        }
    }
}
