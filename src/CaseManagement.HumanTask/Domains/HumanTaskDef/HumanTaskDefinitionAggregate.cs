using CaseManagement.Common.Domains;
using CaseManagement.Common.Exceptions;
using CaseManagement.Common.ISO8601;
using CaseManagement.HumanTask.Domains.HumanTaskDef.Events;
using CaseManagement.HumanTask.Resources;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CaseManagement.HumanTask.Domains
{
    public class HumanTaskDefinitionAggregate : BaseAggregate
    {
        public HumanTaskDefinitionAggregate()
        {
            ActualOwnerRequired = true;
            OperationParameters = new List<Parameter>();
            PresentationElements = new List<PresentationElementDefinition>();
            PeopleAssignments = new List<PeopleAssignmentDefinition>();
            DeadLines = new List<HumanTaskDefinitionDeadLine>();
            Completions = new List<Completion>();
            SubTasks = new List<HumanTaskDefinitionSubTask>();
            PresentationParameters = new List<PresentationParameter>();
        }

        public int NbInstances { get; set; }
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
        /// Specify the priority of the task (from 0 to 10).
        /// </summary>
        public int Priority { get; set; }
        /// <summary>
        /// This optional element identifies the field (of an xsd simple type) in the output message which reflects the business result of a task.
        /// A conversion takes place to yield an outcome of type xsd:string.
        /// </summary>
        public string Outcome { get; set; }
        /// <summary>
        /// This optional element is used to search for task instances based on a custom search criterion.
        /// </summary>
        public string SearchBy { get; set; }
        public string ValidatorFullQualifiedName { get; set; }
        /// <summary>
        /// This element is used to specify subtasks of a composite task. 
        /// It is optional.
        /// </summary>
        public CompositionTypes Type { get; set; }
        public InstantiationPatterns InstantiationPattern { get; set; }
        public CompletionBehaviors CompletionAction { get; set; }
        public ICollection<Completion> Completions { get; set; }
        /// <summary>
        /// This element is used to specify the operation used to invoke the task.
        /// </summary>
        public ICollection<Parameter> OperationParameters { get; set; }
        public ICollection<Parameter> InputParameters { get => OperationParameters.Where(_ => _.Usage == ParameterUsages.INPUT).ToList() ; }
        public ICollection<Parameter> OutputParameters { get => OperationParameters.Where(_ => _.Usage == ParameterUsages.OUTPUT).ToList(); }
        /// <summary>
        /// This optional attribute specifies the way sub-tasks are instantiated.
        /// </summary>
        public ICollection<HumanTaskDefinitionSubTask> SubTasks { get; set; }
        /// <summary>
        /// This element is used to specify rendering method. It is optional.
        /// </summary>
        public string Rendering { get; set; }
        /// <summary>
        /// This element specifies different deadlines.
        /// It is optional.
        /// </summary>
        public ICollection<HumanTaskDefinitionDeadLine> DeadLines { get; set; }
        /// <summary>
        /// Used to specify people assigned to different generic human roles, i.e. potential owners, and business administrators.
        /// </summary>
        public ICollection<PeopleAssignmentDefinition> PeopleAssignments { get; set; }
        /// <summary>
        ///  This element is used to specify different information used to display the task in a task list, such as name, subject and description.
        /// </summary>
        public ICollection<PresentationElementDefinition> PresentationElements { get; set; }
        /// <summary>
        /// This element specifies parameters used in presentation elements subject and description.
        /// </summary>
        public ICollection<PresentationParameter> PresentationParameters { get; set; }

        public override object Clone()
        {
            return new HumanTaskDefinitionAggregate
            {
                CreateDateTime = CreateDateTime,
                UpdateDateTime = UpdateDateTime,
                Name = Name,
                NbInstances = NbInstances,
                AggregateId = AggregateId,
                Version = Version,
                ValidatorFullQualifiedName = ValidatorFullQualifiedName,
                ActualOwnerRequired = ActualOwnerRequired,
                OperationParameters = OperationParameters.Select(_ => (Parameter)_.Clone()).ToList(),
                Priority = Priority,
                PeopleAssignments = PeopleAssignments.Select(_ => (PeopleAssignmentDefinition)_.Clone()).ToList(),
                PresentationElements = PresentationElements.Select(_ => (PresentationElementDefinition)_.Clone()).ToList(),
                PresentationParameters = PresentationParameters.Select(_ => (PresentationParameter)_.Clone()).ToList(),
                Outcome = Outcome,
                SearchBy = SearchBy,
                Rendering = Rendering,
                DeadLines = DeadLines.Select(_ => (HumanTaskDefinitionDeadLine)_.Clone()).ToList(),
                CompletionAction = CompletionAction,
                InstantiationPattern = InstantiationPattern,
                Type = Type,
                SubTasks = SubTasks.Select(_ => (HumanTaskDefinitionSubTask)_.Clone()).ToList(),
                Completions = Completions.Select(_ => (Completion)_.Clone()).ToList()
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

        public void UpdatePeopleAssignment(params PeopleAssignmentDefinition[] peopleAssigments)
        {
            var evt = new HumanTaskPeopleAssignedEvent(Guid.NewGuid().ToString(),
                AggregateId,
                Version + 1, 
                peopleAssigments,
                DateTime.UtcNow);
            Handle(evt);
        }

        public string AddParameter(Parameter parameter)
        {
            var parameterId = Guid.NewGuid().ToString();
            parameter.Id = parameterId;
            var evt = new NotificationDefinitionParameterAddedEvent(Guid.NewGuid().ToString(), AggregateId, Version + 1, parameter, DateTime.UtcNow);
            Handle(evt);
            return parameterId;
        }

        public void RemoveParameter(string parameterId)
        {
            var evt = new NotificationDefinitionParameterRemovedEvent(Guid.NewGuid().ToString(), AggregateId, Version + 1, parameterId, DateTime.UtcNow);
            Handle(evt);
        }

        public void UpdatePresentationElts(IEnumerable<PresentationElementDefinition> elts)
        {
            var evt = new HumanTaskDefPresentationEltsUpdatedEvent(Guid.NewGuid().ToString(), AggregateId, Version + 1, elts, DateTime.UtcNow);
            Handle(evt);
        }

        public void UpdatePresentationParameters(ICollection<PresentationParameter> pars)
        {
            var evt = new HumanTaskDefPresentationParsUpdatedEvent(Guid.NewGuid().ToString(), AggregateId, Version + 1, pars, DateTime.UtcNow);
            Handle(evt);
        }

        public string AddDeadLine(HumanTaskDefinitionDeadLine deadLine)
        {
            var id = Guid.NewGuid().ToString();
            deadLine.Id = id;
            var evt = new HumanTaskDefDeadLineAddedEvent(Guid.NewGuid().ToString(), AggregateId, Version + 1, deadLine, DateTime.UtcNow);
            Handle(evt);
            return id;
        }

        public void AddPresentationElement(PresentationElementDefinition presentationElement)
        {
            var evt = new HumanTaskDefPresentationElementAddedEvent(Guid.NewGuid().ToString(), AggregateId, Version + 1, presentationElement, DateTime.UtcNow);
            Handle(evt);
        }

        public void DeletePresentationElement(PresentationElementUsages usage, string language)
        {
            var evt = new HumanTaskDefPresentationElementRemovedEvent(Guid.NewGuid().ToString(), AggregateId, Version + 1, usage, language, DateTime.UtcNow);
            Handle(evt);
        }

        public void IncrementInstance()
        {
            NbInstances++;
        }

        public void DeleteDeadLine(string deadLineId)
        {
            var evt = new HumanTaskDefDeadLineRemovedEvent(Guid.NewGuid().ToString(), AggregateId, Version + 1, deadLineId, DateTime.UtcNow);
            Handle(evt);
        }

        public void UpdateDeadline(string id, string name, string forExpr, string untilExpr)
        {
            var evt = new HumanTaskDefDeadlineUpdatedEvent(Guid.NewGuid().ToString(), AggregateId, Version + 1, id, name, forExpr, untilExpr, DateTime.UtcNow);
            Handle(evt);
        }

        public string AddEscalationDeadline(string startDeadlineId, string condition, string notificationId)
        {
            var result = Guid.NewGuid().ToString();
            var evt = new HumanTaskDefEscalationDeadlineAddedEvent(Guid.NewGuid().ToString(), AggregateId, Version + 1, startDeadlineId, result, condition, notificationId, DateTime.UtcNow);
            Handle(evt);
            return result;
        }

        public void UpdateEscalationDeadline(string deadlineId, string escalationId, string condition, string notificationId)
        {
            var evt = new HumanTaskDefEscalationDeadlineUpdatedEvent(Guid.NewGuid().ToString(), AggregateId, Version + 1, deadlineId, escalationId, condition, notificationId, DateTime.UtcNow);
            Handle(evt);
        }

        public void DeleteEscalationDeadline(string completionDeadLineId, string escalationId)
        {
            var evt = new HumanTaskDefEscalationDeadlineRemovedEvent(Guid.NewGuid().ToString(), AggregateId, Version + 1, completionDeadLineId, escalationId, DateTime.UtcNow);
            Handle(evt);
        }

        public void UpdateRendering(string rendering)
        {
            var evt = new HumanTaskDefRenderingUpdatedEvent(Guid.NewGuid().ToString(), AggregateId, Version + 1, rendering, DateTime.UtcNow);
            Handle(evt);
        }

        public void AddPresentationParameter(PresentationParameter presentationParameter)
        {
            var evt = new HumanTaskPresentationParameterAddedEvent(Guid.NewGuid().ToString(), AggregateId, Version + 1, presentationParameter, DateTime.UtcNow);
            Handle(evt);
        }

        public void DeletePresentationParameter(string name)
        {
            var evt = new HumanTaskPresentationParameterRemovedEvent(Guid.NewGuid().ToString(), AggregateId, Version + 1, name, DateTime.UtcNow);
            Handle(evt);
        }

        public string Assign(PeopleAssignmentDefinition peopleAssignment)
        {
            peopleAssignment.Id = Guid.NewGuid().ToString();
            var evt = new HumanTaskDefPeopleAssignedEvent(Guid.NewGuid().ToString(), AggregateId, Version + 1, peopleAssignment, DateTime.UtcNow);
            Handle(evt);
            return peopleAssignment.Id;
        }

        public void RemoveAssignment(string assignmentId)
        {
            var evt = new HumanTaskDefPeopleAssignmentRemovedEvent(Guid.NewGuid().ToString(), AggregateId, Version + 1, assignmentId, DateTime.UtcNow);
            Handle(evt);
        }

        public void AddEscalationToPart(string deadlineId, string escalationId, ToPart toPart)
        {
            var evt = new HumanTaskDefEscalationToPartAddedEvent(Guid.NewGuid().ToString(), AggregateId, Version + 1, deadlineId, escalationId, toPart, DateTime.UtcNow);
            Handle(evt);
        }

        public void DeleteEscalationToPart(string deadlineId, string escalationId, string toPartName)
        {
            var evt = new HumanTaskDefEscalationToPartRemovedEvent(Guid.NewGuid().ToString(), AggregateId, Version + 1, deadlineId, escalationId, toPartName, DateTime.UtcNow);
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
            PeopleAssignments.Clear();
            foreach(var l in evt.PeopleAssignments)
            {
                PeopleAssignments.Add(l);
            }

            UpdateDateTime = evt.UpdateDateTime;
            Version = evt.Version;
        }

        private void Handle(NotificationDefinitionParameterAddedEvent evt)
        {
            var parameter = OperationParameters.FirstOrDefault(_ => _.Name == evt.Parameter.Name && _.Usage == evt.Parameter.Usage);
            if (parameter != null)
            {
                throw new AggregateValidationException(new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("validation", string.Format(Global.OperationParameterExists, evt.Parameter.Name))
                });
            }

            OperationParameters.Add(new Parameter
            {
                Id = evt.Parameter.Id,
                IsRequired = evt.Parameter.IsRequired,
                Name = evt.Parameter.Name,
                Type = evt.Parameter.Type,
                Usage = evt.Parameter.Usage
            });
            UpdateDateTime = evt.UpdateDateTime;
            Version = evt.Version;
        }

        private void Handle(NotificationDefinitionParameterRemovedEvent evt)
        {
            var op = OperationParameters.FirstOrDefault(_ => _.Id == evt.ParameterId);
            if (op == null)
            {
                throw new AggregateValidationException(new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("validation", string.Format(Global.OperationParameterDoesntExist, evt.ParameterId))
                });
            }

            OperationParameters.Remove(op);
            UpdateDateTime = evt.UpdateDateTime;
            Version = evt.Version;
        }

        private void Handle(HumanTaskDefPresentationEltsUpdatedEvent evt)
        {
            PresentationElements.Clear();
            foreach(var p in evt.PresentationElts)
            {
                PresentationElements.Add(p);
            }

            UpdateDateTime = evt.UpdateDateTime;
            Version = evt.Version;
        }

        private void Handle(HumanTaskDefPresentationParsUpdatedEvent evt)
        {
            PresentationParameters.Clear();
            foreach (var p in evt.PresentationParameters)
            {
                PresentationParameters.Add(p);
            }

            UpdateDateTime = evt.UpdateDateTime;
            Version = evt.Version;
        }

        private void Handle(HumanTaskDefDeadLineAddedEvent evt)
        {
            CheckDeadLine(evt.DeadLine);
            DeadLines.Add(evt.DeadLine);
            UpdateDateTime = evt.UpdateDateTime;
            Version = evt.Version;
        }

        private void Handle(HumanTaskDefDeadLineRemovedEvent evt)
        {
            var e = DeadLines.FirstOrDefault(_ => _.Id == evt.DeadLineId);
            if (e == null)
            {
                throw new AggregateValidationException(new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("validation", Global.DeadLineDoesntExist)
                });
            }

            DeadLines.Remove(e);
            UpdateDateTime = evt.UpdateDateTime;
            Version = evt.Version;
        }

        private void Handle(HumanTaskDefEscalationDeadlineAddedEvent evt)
        {
            var errors = new List<KeyValuePair<string, string>>();
            var deadline = DeadLines.FirstOrDefault(_ => _.Id == evt.DeadLineId);
            if (deadline == null)
            {
                errors.Add(new KeyValuePair<string, string>("validation", Global.UnknownDeadline));
            }

            if (errors.Any())
            {
                throw new AggregateValidationException(errors);
            }

            deadline.Escalations.Add(new Escalation
            {
                NotificationId = evt.NotificationId,
                Condition = evt.Condition,
                Id = evt.EscalationId
            });
            UpdateDateTime = evt.UpdateDateTime;
            Version = evt.Version;
        }

        private void Handle(HumanTaskDefEscalationDeadlineUpdatedEvent evt)
        {
            Escalation escalation = null;
            var errors = new List<KeyValuePair<string, string>>();
            var deadline = DeadLines.FirstOrDefault(_ => _.Id == evt.DeadLineId);
            if (deadline == null)
            {
                errors.Add(new KeyValuePair<string, string>("validation", Global.UnknownDeadline));
            }
            else
            {
                escalation = deadline.Escalations.FirstOrDefault(_ => _.Id == evt.EscalationId);
                if (escalation == null)
                {
                    errors.Add(new KeyValuePair<string, string>("validation", Global.UnknownEscalation));
                }
            }

            if (errors.Any())
            {
                throw new AggregateValidationException(errors);
            }

            escalation.NotificationId = evt.NotificationId;
            escalation.Condition = evt.Condition;
            UpdateDateTime = evt.UpdateDateTime;
            Version = evt.Version;
        }

        private void Handle(HumanTaskDefDeadlineUpdatedEvent evt)
        {
            var d = DeadLines.FirstOrDefault(_ => _.Id == evt.DeadLineId);
            if (d == null)
            {
                throw new AggregateValidationException(new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("validation", Global.UnknownDeadline)
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

        private void Handle(HumanTaskDefEscalationDeadlineRemovedEvent evt)
        {
            var kvp = CheckEscalation(DeadLines, evt.DeadlineId, evt.EscalationId, Global.UnknownDeadline);
            kvp.Key.Escalations.Remove(kvp.Value);
            UpdateDateTime = evt.UpdateDateTime;
            Version = evt.Version;
        }

        private void Handle(HumanTaskDefRenderingUpdatedEvent evt)
        {
            Rendering = evt.Rendering;
            UpdateDateTime = evt.UpdateDateTime;
            Version = evt.Version;
        }

        private void Handle(HumanTaskPresentationParameterAddedEvent evt)
        {
            if (PresentationParameters.Any(_ => _.Name == evt.PresentationParameter.Name))
            {
                throw new AggregateValidationException(new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("validation", string.Format(Global.PresentationParameterExists, evt.PresentationParameter.Name))
                });
            }

            PresentationParameters.Add(evt.PresentationParameter);
            UpdateDateTime = evt.CreateDateTime;
            Version = evt.Version;
        }

        private void Handle(HumanTaskPresentationParameterRemovedEvent evt)
        {
            var record = PresentationParameters.FirstOrDefault(_ => _.Name == evt.Name);
            if (record == null)
            {
                throw new AggregateValidationException(new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("validation", string.Format(Global.PresentationParameterDoesntExist, evt.Name))
                });
            }

            PresentationParameters.Remove(record);
            UpdateDateTime = evt.ReceptionDate;
            Version = evt.Version;
        }

        private void Handle(HumanTaskDefPresentationElementAddedEvent evt)
        {
            PresentationElements.Add(evt.PresentationElement);
            UpdateDateTime = evt.ReceptionDate;
            Version = evt.Version;
        }

        private void Handle(HumanTaskDefPresentationElementRemovedEvent evt)
        {
            var presentationElt = PresentationElements.FirstOrDefault(_ => _.Language == evt.Language && _.Usage == evt.Usage);
            if (presentationElt == null)
            {
                throw new AggregateValidationException(new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("validation", Global.PresentationElementDoesntExist)
                });
            }

            PresentationElements.Remove(presentationElt);
            UpdateDateTime = evt.ReceptionDateTime;
            Version = evt.Version;
        }

        private void Handle(HumanTaskDefPeopleAssignedEvent evt)
        {
            PeopleAssignments.Add(evt.PeopleAssignment);
            UpdateDateTime = evt.CreateDateTime;
            Version = evt.Version;
        }

        private void Handle(HumanTaskDefPeopleAssignmentRemovedEvent evt)
        {
            var record = PeopleAssignments.First(_ => _.Id == evt.AssignmentId);
            if (record == null)
            {
                throw new AggregateValidationException(new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("validation", string.Format(Global.PeopleAssignmentDoesntExist, evt.AssignmentId))
                });
            }

            PeopleAssignments.Remove(record);
            UpdateDateTime = evt.RemovedDateTime;
            Version = evt.Version;
        }

        private void Handle(HumanTaskDefEscalationToPartAddedEvent evt)
        {
            var deadline = DeadLines.FirstOrDefault(_ => _.Id == evt.DeadlineId);
            if (deadline == null)
            {
                throw new AggregateValidationException(new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("validation", string.Format(Global.UnknownDeadline, evt.DeadlineId))
                });
            }

            var escalation = deadline.Escalations.FirstOrDefault(_ => _.Id == evt.EscalationId);
            if (escalation == null)
            {
                throw new AggregateValidationException(new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("validation", string.Format(Global.UnknownEscalation, evt.EscalationId))
                });
            }

            var toPart = escalation.ToParts.FirstOrDefault(_ => _.Name == evt.ToPart.Name);
            if (toPart != null)
            {
                throw new AggregateValidationException(new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("validation", string.Format(Global.ExistingToPart, evt.ToPart.Name))
                });
            }

            escalation.ToParts.Add(evt.ToPart);
            UpdateDateTime = evt.UpdateDateTime;
            Version = evt.Version;
        }
        
        private void Handle(HumanTaskDefEscalationToPartRemovedEvent evt)
        {
            var deadline = DeadLines.FirstOrDefault(_ => _.Id == evt.DeadlineId);
            if (deadline == null)
            {
                throw new AggregateValidationException(new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("validation", string.Format(Global.UnknownDeadline, evt.DeadlineId))
                });
            }

            var escalation = deadline.Escalations.FirstOrDefault(_ => _.Id == evt.EscalationId);
            if (escalation == null)
            {
                throw new AggregateValidationException(new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("validation", string.Format(Global.UnknownEscalation, evt.EscalationId))
                });
            }

            var toPart = escalation.ToParts.FirstOrDefault(_ => _.Name == evt.ToPartName);
            if (toPart == null)
            {
                throw new AggregateValidationException(new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("validation", string.Format(Global.UnknownToPart, evt.ToPartName))
                });
            }

            escalation.ToParts.Remove(toPart);
            UpdateDateTime = evt.UpdateDateTime;
            Version = evt.Version;
        }

        private KeyValuePair<HumanTaskDefinitionDeadLine, Escalation> CheckEscalation(
            ICollection<HumanTaskDefinitionDeadLine> deadlines, string deadlineId, string escalationId, string unknownEscalationError)
        {
            var errors = new List<KeyValuePair<string, string>>();
            var deadline = deadlines.FirstOrDefault(_ => _.Id == deadlineId);
            Escalation esc = null;
            if (deadline == null)
            {
                errors.Add(new KeyValuePair<string, string>("validation", unknownEscalationError));
            }
            else
            {
                esc = deadline.Escalations.FirstOrDefault(_ => _.Id == escalationId);
                if (esc == null)
                {
                    errors.Add(new KeyValuePair<string, string>("validation", Global.UnknownEscalation));
                }
            }

            if (errors.Any())
            {
                throw new AggregateValidationException(errors);
            }

            return new KeyValuePair<HumanTaskDefinitionDeadLine, Escalation>(deadline, esc);
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
