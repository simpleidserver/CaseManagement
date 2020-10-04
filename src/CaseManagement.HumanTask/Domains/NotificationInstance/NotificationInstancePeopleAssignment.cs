using System;

namespace CaseManagement.HumanTask.Domains
{
    public class NotificationInstancePeopleAssignment : ICloneable
    {
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
            return new NotificationInstancePeopleAssignment
            {
                BusinessAdministrator = (PeopleAssignmentInstance)BusinessAdministrator?.Clone(),
                Recipient = (PeopleAssignmentInstance)Recipient?.Clone()
            };
        }
    }
}
