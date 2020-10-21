using CaseManagement.Common.Domains;
using System;

namespace CaseManagement.HumanTask.Domains
{
    public class HumanTaskDefinitionAggregate : BaseAggregate
    {
        public HumanTaskDefinitionAggregate()
        {
            ActualOwnerRequired = true;
            Operation = new Operation();
            PresentationElement = new PresentationElementDefinition();
            PeopleAssignment = new HumanTaskDefinitionAssignment();
            Rendering = new Rendering();
            DeadLines = new HumanTaskDefinitionDeadLines();
            CompletionBehavior = new CompletionBehavior();
        }

        public DateTime CreateDateTime { get; set; }
        public DateTime UpdateDateTime { get; set; }
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
        public HumanTaskDefinitionAssignment PeopleAssignment { get; set; }
        /// <summary>
        /// Used to specify constraints concerning delegation of the task.
        /// </summary>
        public Delegation Delegation { get; set; }
        /// <summary>
        ///  This element is used to specify different information used to display the task in a task list, such as name, subject and description.
        /// </summary>
        public PresentationElementDefinition PresentationElement { get; set; }
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
        public Rendering Rendering { get; set; }
        /// <summary>
        /// This element specifies different deadlines.
        /// It is optional.
        /// </summary>
        public HumanTaskDefinitionDeadLines DeadLines { get; set; }
        /// <summary>
        /// This element is used to specify subtasks of a composite task. 
        /// It is optional.
        /// </summary>
        public HumanTaskDefinitionComposition Composition { get; set; }
        public CompletionBehavior CompletionBehavior { get; set; }

        public override object Clone()
        {
            return new HumanTaskDefinitionAggregate
            {
                CreateDateTime = CreateDateTime,
                UpdateDateTime = UpdateDateTime,
                Name = Name,
                AggregateId = AggregateId,
                Version = Version,
                ActualOwnerRequired = ActualOwnerRequired,
                Operation = (Operation)Operation?.Clone(),
                Priority = Priority,
                PeopleAssignment = (HumanTaskDefinitionAssignment)PeopleAssignment?.Clone(),
                Delegation = (Delegation)Delegation?.Clone(),
                PresentationElement = (PresentationElementDefinition)PresentationElement?.Clone(),
                Outcome = Outcome,
                SearchBy = SearchBy,
                Rendering = (Rendering)Rendering?.Clone(),
                DeadLines = (HumanTaskDefinitionDeadLines)DeadLines?.Clone(),
                Composition = (HumanTaskDefinitionComposition)Composition?.Clone(),
                CompletionBehavior = (CompletionBehavior)CompletionBehavior?.Clone()
            };
        }

        public static HumanTaskDefinitionAggregate New(string name)
        {
            var id = Guid.NewGuid().ToString();
            var evt = new HumanTaskDefCreatedEvent(Guid.NewGuid().ToString(), id, 0, name, DateTime.UtcNow);
            var result = new HumanTaskDefinitionAggregate();
            result.Handle(evt);
            return result;
        }

        public void UpdateInfo(string name, int priority)
        {
            var evt = new HumanTaskInfoUpdatedEvent(Guid.NewGuid().ToString(), AggregateId, Version + 1, name, priority, DateTime.UtcNow);
            Handle(evt);
        }

        public void UpdatePeopleAssignment(
            PeopleAssignmentDefinition potentialOwner,
            PeopleAssignmentDefinition excludedOwner,
            PeopleAssignmentDefinition taskInitiator,
            PeopleAssignmentDefinition taskStakeHolder,
            PeopleAssignmentDefinition businessAdministrator,
            PeopleAssignmentDefinition recipient)
        {
            var evt = new HumanTaskPeopleAssignedEvent(Guid.NewGuid().ToString(),
                AggregateId,
                Version + 1,
                potentialOwner,
                excludedOwner,
                taskInitiator,
                taskStakeHolder,
                businessAdministrator,
                recipient,
                DateTime.UtcNow);
            Handle(evt);
        }

        public override void Handle(dynamic evt)
        {
            Handle(evt);
        }

        private void Handle(HumanTaskDefCreatedEvent evt) 
        {
            AggregateId = evt.AggregateId;
            Version = evt.Version;
            Name = evt.Name;
            CreateDateTime = evt.CreateDateTime;
        }

        private void Handle(HumanTaskInfoUpdatedEvent evt)
        {
            Name = evt.Name;
            Priority = evt.Priority;
            UpdateDateTime = evt.UpdateDateTime;
            Version = evt.Version;
        }

        private void Handle(HumanTaskPeopleAssignedEvent evt)
        {
            PeopleAssignment.BusinessAdministrator = evt.BusinessAdministrator;
            PeopleAssignment.ExcludedOwner = evt.ExcludedOwner;
            PeopleAssignment.PotentialOwner = evt.PotentialOwner;
            PeopleAssignment.Recipient = evt.Recipient;
            PeopleAssignment.TaskInitiator = evt.TaskInitiator;
            PeopleAssignment.TaskStakeHolder = evt.TaskStakeHolder;
            UpdateDateTime = evt.UpdateDateTime;
            Version = evt.Version;
        }
    }
}
