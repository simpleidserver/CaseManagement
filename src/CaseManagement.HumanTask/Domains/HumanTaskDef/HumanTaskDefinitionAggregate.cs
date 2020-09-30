using System;
using System.Collections.Generic;
using System.Linq;

namespace CaseManagement.HumanTask.Domains
{
    public class HumanTaskDefinitionAggregate : ICloneable
    {
        public HumanTaskDefinitionAggregate()
        {
            ActualOwnerRequired = true;
            Operation = new Operation();
            PeopleAssignment = new TaskPeopleAssignment();
            Renderings = new List<Rendering>();
        }

        /// <summary>
        /// Used to specify the name of the task.
        /// This attribute is mandatory.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Actual owner is required for the task.
        /// Setting the value to "false" is used for composite tasks where subtasks should be activated automatically without user interaction.
        /// Tasks that have been defined to not have subtasks MUST have exactly one actual owner after they have been claimed. For these tasks the value of the attribute value MUST be "true".
        /// Default value is "true"
        /// </summary>
        public bool ActualOwnerRequired { get; set; }
        /// <summary>
        /// This element is used to specify the operation used to invoke the task.
        /// </summary>
        public Operation Operation { get; set; }
        /// <summary>
        /// Specify the priority of the task (from 0 to 10).
        /// </summary>
        public int Priority { get; set; }
        /// <summary>
        /// Used to specify people assigned to different generic human roles, i.e. potential owners, and business administrators.
        /// </summary>
        public TaskPeopleAssignment PeopleAssignment { get; set; }
        /// <summary>
        /// Used to specify constraints concerning delegation of the task.
        /// </summary>
        public Delegation Delegation { get; set; }
        /// <summary>
        ///  This element is used to specify different information used to display the task in a task list, such as name, subject and description.
        /// </summary>
        public PresentationElement PresentationElement { get; set; }
        /// <summary>
        /// This optional element identifies the field (of an xsd simple type) in the output message which reflects the business result of a task.
        /// A conversion takes place to yield an outcome of type xsd:string.
        /// </summary>
        public string Outcome { get; set; }
        /// <summary>
        /// This optional element is used to search for task instances based on a custom search criterion.
        /// </summary>
        public string SearchBy { get; set; }
        /// <summary>
        /// This element is used to specify rendering method. It is optional.
        /// </summary>
        public ICollection<Rendering> Renderings { get; set; }
        /// <summary>
        /// This element specifies different deadlines.
        /// It is optional.
        /// </summary>
        public TaskDeadLines DeadLines { get; set; }
        /// <summary>
        /// This element is used to specify subtasks of a composite task. 
        /// It is optional.
        /// </summary>
        public Composition Composition { get; set; }

        public object Clone()
        {
            return new HumanTaskDefinitionAggregate
            {
                Name = Name,
                ActualOwnerRequired = ActualOwnerRequired,
                Operation = (Operation)Operation?.Clone(),
                Priority = Priority,
                PeopleAssignment = (TaskPeopleAssignment)PeopleAssignment?.Clone(),
                Delegation = (Delegation)Delegation?.Clone(),
                PresentationElement = (PresentationElement)PresentationElement?.Clone(),
                Outcome = Outcome,
                SearchBy = SearchBy,
                Renderings = Renderings.Select(_ => (Rendering)_.Clone()).ToList(),
                DeadLines = (TaskDeadLines)DeadLines?.Clone(),
                Composition = (Composition)Composition?.Clone()
            };
        }
    }
}
