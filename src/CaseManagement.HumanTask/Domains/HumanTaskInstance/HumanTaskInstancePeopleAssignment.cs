using System;

namespace CaseManagement.HumanTask.Domains
{
    public class HumanTaskInstancePeopleAssignment : ICloneable
    {
        /// <summary>
        /// Potential owners of a task are persons who receive the task so that they can claim and complete it.
        /// A potential owner becomes the actual owner of a task by explicitly claiming it.
        /// </summary>
        public PeopleAssignmentInstance PotentialOwner { get; set; }
        /// <summary>
        /// Excluded owners are are people who cannot become an actual or potential owner and thus they cannot reserve or start the task.
        /// </summary>
        public PeopleAssignmentInstance ExcludedOwner { get; set; }
        /// <summary>
        /// Is the person who creates the task instance.
        /// </summary>
        public PeopleAssignmentInstance TaskInitiator { get; set; }
        /// <summary>
        /// The task stakeholders are the people ultimately responsible for the oversight and outcome of the task instance
        /// </summary>
        public PeopleAssignmentInstance TaskStakeHolder { get; set; }
        /// <summary>
        /// Business administrators play the same role as task stakeholders but at task definition level.
        /// </summary>
        public PeopleAssignmentInstance BusinessAdministrator { get; set; }
        /// <summary>
        /// Notification recipients are persons who receive the notification.
        /// </summary>
        public PeopleAssignmentInstance Recipient { get; set; }

        public object Clone()
        {
            return new HumanTaskInstancePeopleAssignment
            {
                PotentialOwner = (PeopleAssignmentInstance)PotentialOwner?.Clone(),
                ExcludedOwner = (PeopleAssignmentInstance)ExcludedOwner?.Clone(),
                TaskInitiator = (PeopleAssignmentInstance)TaskInitiator?.Clone(),
                TaskStakeHolder = (PeopleAssignmentInstance)TaskStakeHolder?.Clone(),
                BusinessAdministrator = (PeopleAssignmentInstance)BusinessAdministrator?.Clone(),
                Recipient = (PeopleAssignmentInstance)Recipient?.Clone()
            };
        }
    }
}