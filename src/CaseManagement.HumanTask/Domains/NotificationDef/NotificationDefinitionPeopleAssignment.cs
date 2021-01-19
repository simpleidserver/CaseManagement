using System;

namespace CaseManagement.HumanTask.Domains
{
    public class NotificationDefinitionPeopleAssignment : ICloneable
    {
        /// <summary>
        /// Notification recipients are persons who receive the notification.
        /// </summary>
        public PeopleAssignmentDefinition Recipient { get; set; }
        /// <summary>
        /// Business administrators play the same role as task stakeholders but at task definition level.
        /// </summary>
        public PeopleAssignmentDefinition BusinessAdministrator { get; set; }

        public object Clone()
        {
            return new NotificationDefinitionPeopleAssignment
            {
                Recipient = (PeopleAssignmentDefinition)Recipient?.Clone(),
                BusinessAdministrator = (PeopleAssignmentDefinition)BusinessAdministrator?.Clone()
            };
        }
    }
}
