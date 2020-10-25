using CaseManagement.Common.Domains;
using CaseManagement.Common.Exceptions;
using CaseManagement.Common.ISO8601;
using CaseManagement.HumanTask.Domains.HumanTaskDef.Events;
using CaseManagement.HumanTask.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;

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

        public void AddInputParameter(Parameter parameter)
        {
            var evt = new HumanTaskDefInputParameterAddedEvent(Guid.NewGuid().ToString(), AggregateId, Version + 1, parameter, DateTime.UtcNow);
            Handle(evt);
        }

        public void AddOutputParameter(Parameter parameter)
        {
            var evt = new HumanTaskDefOutputParameterAddedEvent(Guid.NewGuid().ToString(), AggregateId, Version + 1, parameter, DateTime.UtcNow);
            Handle(evt);
        }

        public void RemoveInputParameter(string parameterName)
        {
            var evt = new HumanTaskDefInputParameterRemovedEvent(Guid.NewGuid().ToString(), AggregateId, Version + 1, parameterName, DateTime.UtcNow);
            Handle(evt);
        }

        public void RemoveOutputParameter(string parameterName)
        {
            var evt = new HumanTaskDefOutputParameterRemovedEvent(Guid.NewGuid().ToString(), AggregateId, Version + 1, parameterName, DateTime.UtcNow);
            Handle(evt);
        }

        public void UpdatePresentationElt(PresentationElementDefinition elt)
        {
            var evt = new HumanTaskDefPresentationEltUpdatedEvent(Guid.NewGuid().ToString(), AggregateId, Version + 1, elt, DateTime.UtcNow);
            Handle(evt);
        }

        public string AddStartDeadLine(HumanTaskDefinitionDeadLine deadLine)
        {
            var id = Guid.NewGuid().ToString();
            deadLine.Id = id;
            var evt = new HumanTaskDefStartDeadLineAddedEvent(Guid.NewGuid().ToString(), AggregateId, Version + 1, deadLine, DateTime.UtcNow);
            Handle(evt);
            return id;
        }

        public string AddCompletionDeadLine(HumanTaskDefinitionDeadLine deadLine)
        {
            var id = Guid.NewGuid().ToString();
            deadLine.Id = id;
            var evt = new HumanTaskDefCompletionDeadLineAddedEvent(Guid.NewGuid().ToString(), AggregateId, Version + 1, deadLine, DateTime.UtcNow);
            Handle(evt);
            return id;
        }

        public void DeleteCompletionDeadLine(string deadLineId)
        {
            var evt = new HumanTaskDefCompletionDeadLineRemovedEvent(Guid.NewGuid().ToString(), AggregateId, Version + 1, deadLineId, DateTime.UtcNow);
            Handle(evt);
        }

        public void DeleteStartDeadLine(string deadLineId)
        {
            var evt = new HumanTaskDefStartDeadLineRemovedEvent(Guid.NewGuid().ToString(), AggregateId, Version + 1, deadLineId, DateTime.UtcNow);
            Handle(evt);
        }

        public void UpdateStartDeadline(string id, string name, string forExpr, string untilExpr)
        {
            var evt = new HumanTaskDefStartDeadlineUpdatedEvent(Guid.NewGuid().ToString(), AggregateId, Version + 1, id, name, forExpr, untilExpr, DateTime.UtcNow);
            Handle(evt);
        }

        public void UpdateCompletionDeadline(string id, string name, string forExpr, string untilExpr)
        {
            var evt = new HumanTaskDefCompletionDeadlineUpdatedEvent(Guid.NewGuid().ToString(), AggregateId, Version + 1, id, name, forExpr, untilExpr, DateTime.UtcNow);
            Handle(evt);
        }

        public string AddEscalationStartDeadline(string startDeadlineId, string condition)
        {
            var result = Guid.NewGuid().ToString();
            var evt = new HumanTaskDefEscalationStartDeadlineAddedEvent(Guid.NewGuid().ToString(), AggregateId, Version + 1, startDeadlineId, result, condition, DateTime.UtcNow);
            Handle(evt);
            return result;
        }

        public string AddEscalationCompletionDeadline(string completionDeadlineId, string condition)
        {
            var result = Guid.NewGuid().ToString();
            var evt = new HumanTaskDefEscalationCompletionDeadlineAddedEvent(Guid.NewGuid().ToString(), AggregateId, Version + 1, completionDeadlineId, result, condition, DateTime.UtcNow);
            Handle(evt);
            return result;
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

        private void Handle(HumanTaskDefInputParameterAddedEvent evt)
        {
            Operation.InputParameters.Add(evt.Parameter);
            UpdateDateTime = evt.UpdateDateTime;
            Version = evt.Version;
        }

        private void Handle(HumanTaskDefOutputParameterAddedEvent evt)
        {
            Operation.OutputParameters.Add(evt.Parameter);
            UpdateDateTime = evt.UpdateDateTime;
            Version = evt.Version;
        }

        private void Handle(HumanTaskDefInputParameterRemovedEvent evt)
        {
            var op = Operation.InputParameters.First(_ => _.Name == evt.ParameterName);
            Operation.InputParameters.Remove(op);
            UpdateDateTime = evt.UpdateDateTime;
            Version = evt.Version;
        }

        private void Handle(HumanTaskDefOutputParameterRemovedEvent evt)
        {
            var op = Operation.OutputParameters.First(_ => _.Name == evt.ParameterName);
            Operation.OutputParameters.Remove(op);
            UpdateDateTime = evt.UpdateDateTime;
            Version = evt.Version;
        }

        private void Handle(HumanTaskDefPresentationEltUpdatedEvent evt)
        {
            PresentationElement = evt.PresentationElt;
            UpdateDateTime = evt.UpdateDateTime;
            Version = evt.Version;
        }

        private void Handle(HumanTaskDefStartDeadLineAddedEvent evt)
        {
            CheckDeadLine(evt.DeadLine);
            DeadLines.StartDeadLines.Add(evt.DeadLine);
            UpdateDateTime = evt.UpdateDateTime;
            Version = evt.Version;
        }

        private void Handle(HumanTaskDefCompletionDeadLineAddedEvent evt)
        {
            CheckDeadLine(evt.DeadLine);
            DeadLines.CompletionDeadLines.Add(evt.DeadLine);
            UpdateDateTime = evt.UpdateDateTime;
            Version = evt.Version;
        }

        private void Handle(HumanTaskDefCompletionDeadLineRemovedEvent evt)
        {
            var e = DeadLines.CompletionDeadLines.FirstOrDefault(_ => _.Id == evt.DeadLineId);
            if (e == null)
            {
                throw new AggregateValidationException(new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("validation", Global.CompletionDeadLineDoesntExist)
                });
            }

            DeadLines.CompletionDeadLines.Remove(e);
            UpdateDateTime = evt.UpdateDateTime;
            Version = evt.Version;
        }

        private void Handle(HumanTaskDefStartDeadLineRemovedEvent evt)
        {
            var e = DeadLines.StartDeadLines.FirstOrDefault(_ => _.Id == evt.DeadLineId);
            if (e == null)
            {
                throw new AggregateValidationException(new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("validation", Global.StartDeadLineDoesntExist)
                });
            }

            DeadLines.StartDeadLines.Remove(e);
            UpdateDateTime = evt.UpdateDateTime;
            Version = evt.Version;
        }

        private void Handle(HumanTaskDefEscalationStartDeadlineAddedEvent evt)
        {
            var errors = new List<KeyValuePair<string, string>>();
            var completionDeadLine = DeadLines.StartDeadLines.FirstOrDefault(_ => _.Id == evt.StartDeadLineId);
            if (completionDeadLine == null)
            {
                errors.Add(new KeyValuePair<string, string>("validation", Global.UnknownStartDeadline));
            }

            if (errors.Any())
            {
                throw new AggregateValidationException(errors);
            }

            completionDeadLine.Escalations.Add(new Escalation
            {
                Condition = evt.Condition,
                Id = evt.EscalationId
            });
            UpdateDateTime = evt.UpdateDateTime;
            Version = evt.Version;
        }

        private void Handle(HumanTaskDefEscalationCompletionDeadlineAddedEvent evt)
        {
            var errors = new List<KeyValuePair<string, string>>();
            var completionDeadLine = DeadLines.CompletionDeadLines.FirstOrDefault(_ => _.Id == evt.CompletionDeadLineId);
            if (completionDeadLine == null)
            {
                errors.Add(new KeyValuePair<string, string>("validation", Global.UnknownCompletionDeadline));
            }

            if (errors.Any())
            {
                throw new AggregateValidationException(errors);
            }

            completionDeadLine.Escalations.Add(new Escalation
            {
                Condition = evt.Condition,
                Id = evt.EscalationId
            });
            UpdateDateTime = evt.UpdateDateTime;
            Version = evt.Version;
        }

        private void Handle(HumanTaskDefStartDeadlineUpdatedEvent evt)
        {
            var d = DeadLines.StartDeadLines.FirstOrDefault(_ => _.Id == evt.DeadLineId);
            if (d == null)
            {
                throw new AggregateValidationException(new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("validation", Global.UnknownStartDeadline)
                });
            }

            CheckDeadLine(new HumanTaskDefinitionDeadLine
            {
                Name = evt.Name,
                For = evt.ForExpr,
                Until = evt.UntilExpr
            });
            d.Name = evt.Name;
            d.For = evt.ForExpr;
            d.Until = evt.UntilExpr;
            UpdateDateTime = evt.UpdateDateTime;
            Version = evt.Version;
        }

        private void Handle(HumanTaskDefCompletionDeadlineUpdatedEvent evt)
        {
            var d = DeadLines.CompletionDeadLines.FirstOrDefault(_ => _.Id == evt.DeadLineId);
            if (d == null)
            {
                throw new AggregateValidationException(new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("validation", Global.UnknownCompletionDeadline)
                });
            }

            CheckDeadLine(new HumanTaskDefinitionDeadLine
            {
                Name = evt.Name,
                For = evt.ForExpr,
                Until = evt.UntilExpr
            });
            d.Name = evt.Name;
            d.For = evt.ForExpr;
            d.Until = evt.UntilExpr;
            UpdateDateTime = evt.UpdateDateTime;
            Version = evt.Version;
        }

        private void CheckDeadLine(HumanTaskDefinitionDeadLine deadLine)
        {
            const string untilName = "deadline.until";
            const string forName = "deadline.for";
            var errors = new List<KeyValuePair<string, string>>();
            if (string.IsNullOrWhiteSpace(deadLine.Name))
            {
                errors.Add(new KeyValuePair<string, string>("validation", string.Format(Global.MissingParameter, "deadline.name")));
            }

            if (string.IsNullOrWhiteSpace(deadLine.For) && string.IsNullOrWhiteSpace(deadLine.Until))
            {
                errors.Add(new KeyValuePair<string, string>("validation", string.Format(Global.MissingParameter, $"{forName},{untilName}")));
            }
            else if (!string.IsNullOrWhiteSpace(deadLine.Until) && !string.IsNullOrWhiteSpace(deadLine.For))
            {
                errors.Add(new KeyValuePair<string, string>("validation", string.Format(Global.ParametersCannotSpecifiedAtSameTime, $"{forName},{untilName}")));
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(deadLine.Until) && ISO8601Parser.ParseTimeInterval(deadLine.Until) == null)
                {
                    errors.Add(new KeyValuePair<string, string>("validation", string.Format(Global.ParameterNotValidISO8601, untilName)));
                }
            }

            if (errors.Any())
            {
                throw new AggregateValidationException(errors);
            }
        }
    }
}
