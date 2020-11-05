using System;
using System.Collections.Generic;
using System.Linq;

namespace CaseManagement.HumanTask.Domains
{
    public class NotificationDefinition : ICloneable
    {
        public NotificationDefinition()
        {
            OperationParameters = new List<Parameter>();
            PeopleAssignments = new List<PeopleAssignmentDefinition>();
            PresentationElements = new List<PresentationElementDefinition>();
            PresentationParameters = new List<PresentationParameter>();
        }

        public long Id { get; set; }
        public string EscalationId { get; set; }
        public string Name { get; set; }
        /// <summary>
        /// This element is used to specify the operation used to invoke the notification. 
        /// </summary>
        public ICollection<Parameter> OperationParameters { get; set; }
        public ICollection<Parameter> InputParameters { get => OperationParameters.Where(_ => _.Usage == ParameterUsages.INPUT).ToList(); }
        public ICollection<Parameter> OutputParameters { get => OperationParameters.Where(_ => _.Usage == ParameterUsages.OUTPUT).ToList(); }
        /// <summary>
        /// This element is used to specify the priority of the notification.
        /// </summary>
        public int Priority { get; set; }
        /// <summary>
        /// This element is used to specify people assigned to the notification. 
        /// </summary>
        public ICollection<PeopleAssignmentDefinition> PeopleAssignments { get; set; }
        /// <summary>
        ///  This element is used to specify different information used to display the task in a task list, such as name, subject and description.
        /// </summary>
        public ICollection<PresentationElementDefinition> PresentationElements { get; set; }
        public string Rendering { get; set; }
        public ICollection<PresentationParameter> PresentationParameters { get; set; }
        public Escalation Escalation { get; set; }

        public object Clone()
        {
            return new NotificationDefinition
            {
                Name = Name,
                OperationParameters = OperationParameters.Select(_ => (Parameter)_.Clone()).ToList(),
                Priority = Priority,
                PeopleAssignments = PeopleAssignments.Select(_ => (PeopleAssignmentDefinition)_.Clone()).ToList(),
                PresentationElements = PresentationElements.Select(_ => (PresentationElementDefinition)_.Clone()).ToList(),
                Rendering = Rendering
            };
        }
    }
}
