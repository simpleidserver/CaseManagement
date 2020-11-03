using System;

namespace CaseManagement.HumanTask.Domains
{
    public class NotificationDefinition : ICloneable
    {
        public NotificationDefinition()
        {
            Operation = new Operation();
            PeopleAssignment = new NotificationDefinitionPeopleAssignment();
            PresentationElement = new PresentationElementDefinition();
        }

        public string Name { get; set; }
        /// <summary>
        /// This element is used to specify the operation used to invoke the notification. 
        /// </summary>
        public Operation Operation { get; set; }
        /// <summary>
        /// This element is used to specify the priority of the notification.
        /// </summary>
        public int Priority { get; set; }
        /// <summary>
        /// This element is used to specify people assigned to the notification. 
        /// </summary>
        public NotificationDefinitionPeopleAssignment PeopleAssignment { get; set; }
        /// <summary>
        ///  This element is used to specify different information used to display the task in a task list, such as name, subject and description.
        /// </summary>
        public PresentationElementDefinition PresentationElement { get; set; }
        public NotificationRendering Rendering { get; set; }

        public object Clone()
        {
            return new NotificationDefinition
            {
                Name = Name,
                Operation = (Operation)Operation?.Clone(),
                Priority = Priority,
                PeopleAssignment = (NotificationDefinitionPeopleAssignment)PeopleAssignment?.Clone(),
                PresentationElement = (PresentationElementDefinition)PresentationElement?.Clone(),
                Rendering = (NotificationRendering)Rendering?.Clone()
            };
        }
    }
}
