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
            RenderingElements = new List<RenderingElement>();
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
        public ICollection<RenderingElement> RenderingElements { get; set; }
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
                ActualOwnerRequired = ActualOwnerRequired,
                OperationParameters = OperationParameters.Select(_ => (Parameter)_.Clone()).ToList(),
                Priority = Priority,
                PeopleAssignments = PeopleAssignments.Select(_ => (PeopleAssignmentDefinition)_.Clone()).ToList(),
                PresentationElements = PresentationElements.Select(_ => (PresentationElementDefinition)_.Clone()).ToList(),
                PresentationParameters = PresentationParameters.Select(_ => (PresentationParameter)_.Clone()).ToList(),
                Outcome = Outcome,
                SearchBy = SearchBy,
                RenderingElements = RenderingElements.Select(_ => (RenderingElement)_.Clone()).ToList(),
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

        public string AddStartDeadLine(HumanTaskDefinitionDeadLine deadLine)
        {
            var id = Guid.NewGuid().ToString();
            deadLine.Id = id;
            deadLine.Usage = DeadlineUsages.START;
            var evt = new HumanTaskDefStartDeadLineAddedEvent(Guid.NewGuid().ToString(), AggregateId, Version + 1, deadLine, DateTime.UtcNow);
            Handle(evt);
            return id;
        }

        public string AddCompletionDeadLine(HumanTaskDefinitionDeadLine deadLine)
        {
            var id = Guid.NewGuid().ToString();
            deadLine.Id = id;
            deadLine.Usage = DeadlineUsages.COMPLETION;
            var evt = new HumanTaskDefCompletionDeadLineAddedEvent(Guid.NewGuid().ToString(), AggregateId, Version + 1, deadLine, DateTime.UtcNow);
            Handle(evt);
            return id;
        }

        public void IncrementInstance()
        {
            NbInstances++;
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

        public void DeleteEscalationCompletionDeadline(string completionDeadLineId, string escalationId)
        {
            var evt = new HumanTaskDefEscalationCompletionDeadlineRemovedEvent(Guid.NewGuid().ToString(), AggregateId, Version + 1, completionDeadLineId, escalationId, DateTime.UtcNow);
            Handle(evt);
        }

        public void DeleteEscalationStartDeadline(string startDeadlineId, string escalationId)
        {
            var evt = new HumanTaskDefEscalationStartDeadlineRemovedEvent(Guid.NewGuid().ToString(), AggregateId, Version + 1, startDeadlineId, escalationId, DateTime.UtcNow);
            Handle(evt);
        }

        public void UpdateEscalationStartDeadline(string completionDeadLineId, string escalationId, Escalation escalation)
        {
            var evt = new HumanTaskDefEscalationStartDeadlineUpdatedEvent(Guid.NewGuid().ToString(), AggregateId, Version + 1, completionDeadLineId, escalationId, escalation, DateTime.UtcNow);
            Handle(evt);
        }

        public void UpdateEscalationCompletionDeadline(string completionDeadLineId, string escalationId, Escalation escalation)
        {
            var evt = new HumanTaskDefEscalationCompletionDeadlineUpdatedEvent(Guid.NewGuid().ToString(), AggregateId, Version + 1, completionDeadLineId, escalationId, escalation, DateTime.UtcNow);
            Handle(evt);
        }

        public void UpdateRendering(ICollection<RenderingElement> renderingElements)
        {
            var evt = new HumanTaskDefRenderingUpdatedEvent(Guid.NewGuid().ToString(), AggregateId, Version + 1, renderingElements, DateTime.UtcNow);
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

        private void Handle(HumanTaskDefInputParameterAddedEvent evt)
        {
            OperationParameters.Add(new Parameter
            {
                IsRequired = evt.Parameter.IsRequired,
                Name = evt.Parameter.Name,
                Type = evt.Parameter.Type,
                Usage = ParameterUsages.INPUT
            });
            UpdateDateTime = evt.UpdateDateTime;
            Version = evt.Version;
        }

        private void Handle(HumanTaskDefOutputParameterAddedEvent evt)
        {
            OperationParameters.Add(new Parameter
            {
                IsRequired = evt.Parameter.IsRequired,
                Name = evt.Parameter.Name,
                Type = evt.Parameter.Type,
                Usage = ParameterUsages.OUTPUT
            });
            UpdateDateTime = evt.UpdateDateTime;
            Version = evt.Version;
        }

        private void Handle(HumanTaskDefInputParameterRemovedEvent evt)
        {
            var op = OperationParameters.FirstOrDefault(_ => _.Usage == ParameterUsages.INPUT && _.Name == evt.ParameterName);
            OperationParameters.Remove(op);
            UpdateDateTime = evt.UpdateDateTime;
            Version = evt.Version;
        }

        private void Handle(HumanTaskDefOutputParameterRemovedEvent evt)
        {
            var op = OperationParameters.FirstOrDefault(_ => _.Usage == ParameterUsages.OUTPUT && _.Name == evt.ParameterName);
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

        private void Handle(HumanTaskDefStartDeadLineAddedEvent evt)
        {
            CheckDeadLine(evt.DeadLine);
            DeadLines.Add(evt.DeadLine);
            UpdateDateTime = evt.UpdateDateTime;
            Version = evt.Version;
        }

        private void Handle(HumanTaskDefCompletionDeadLineAddedEvent evt)
        {
            CheckDeadLine(evt.DeadLine);
            DeadLines.Add(evt.DeadLine);
            UpdateDateTime = evt.UpdateDateTime;
            Version = evt.Version;
        }

        private void Handle(HumanTaskDefCompletionDeadLineRemovedEvent evt)
        {
            var e = DeadLines.FirstOrDefault(_ => _.Id == evt.DeadLineId && _.Usage == DeadlineUsages.COMPLETION);
            if (e == null)
            {
                throw new AggregateValidationException(new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("validation", Global.CompletionDeadLineDoesntExist)
                });
            }

            DeadLines.Remove(e);
            UpdateDateTime = evt.UpdateDateTime;
            Version = evt.Version;
        }

        private void Handle(HumanTaskDefStartDeadLineRemovedEvent evt)
        {
            var e = DeadLines.FirstOrDefault(_ => _.Id == evt.DeadLineId && _.Usage == DeadlineUsages.START);
            if (e == null)
            {
                throw new AggregateValidationException(new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("validation", Global.StartDeadLineDoesntExist)
                });
            }

            DeadLines.Remove(e);
            UpdateDateTime = evt.UpdateDateTime;
            Version = evt.Version;
        }

        private void Handle(HumanTaskDefEscalationStartDeadlineAddedEvent evt)
        {
            var errors = new List<KeyValuePair<string, string>>();
            var completionDeadLine = DeadLines.FirstOrDefault(_ => _.Id == evt.StartDeadLineId && 
            _.Usage == DeadlineUsages.START);
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
            var completionDeadLine = DeadLines.FirstOrDefault(_ => _.Id == evt.CompletionDeadLineId && _.Usage == DeadlineUsages.COMPLETION);
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
            var d = DeadLines.FirstOrDefault(_ => _.Id == evt.DeadLineId && _.Usage == DeadlineUsages.START);
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
            var d = DeadLines.FirstOrDefault(_ => _.Id == evt.DeadLineId && _.Usage == DeadlineUsages.COMPLETION);
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

        private void Handle(HumanTaskDefEscalationStartDeadlineRemovedEvent evt)
        {
            var kvp = CheckEscalation(DeadLines.Where(_ => _.Usage == DeadlineUsages.START).ToList(), 
                evt.StartDeadLineId, evt.EscalationId, 
                Global.UnknownStartDeadline);
            kvp.Key.Escalations.Remove(kvp.Value);
            UpdateDateTime = evt.UpdateDateTime;
            Version = evt.Version;
        }

        private void Handle(HumanTaskDefEscalationCompletionDeadlineRemovedEvent evt)
        {
            var kvp = CheckEscalation(DeadLines.Where(_ => _.Usage == DeadlineUsages.COMPLETION).ToList(),
                evt.CompletionDeadLineId, evt.EscalationId,
                Global.UnknownCompletionDeadline);
            kvp.Key.Escalations.Remove(kvp.Value);
            UpdateDateTime = evt.UpdateDateTime;
            Version = evt.Version;
        }

        private void Handle(HumanTaskDefEscalationStartDeadlineUpdatedEvent evt)
        {
            var kvp = CheckEscalation(DeadLines.Where(_ => _.Usage == DeadlineUsages.START).ToList(),
                evt.CompletionDeadLineId, evt.EscalationId, 
                Global.UnknownStartDeadline);
            kvp.Value.Condition = evt.Escalation.Condition;
            kvp.Value.Notification.OperationParameters.Clear();
            kvp.Value.Notification.PeopleAssignments.Clear();
            kvp.Value.Notification.PresentationElements.Clear();
            kvp.Value.Notification.PresentationParameters.Clear();
            kvp.Value.ToParts.Clear();
            kvp.Value.Notification = new NotificationDefinition
            {
                Name = evt.Escalation.Notification.Name,
                Priority = evt.Escalation.Notification.Priority,
                Rendering = evt.Escalation.Notification.Rendering
            };
            foreach(var operationParameter in evt.Escalation.Notification.OperationParameters)
            {
                kvp.Value.Notification.OperationParameters.Add(new Parameter
                {
                    IsRequired = operationParameter.IsRequired,
                    Name = operationParameter.Name,
                    Type = operationParameter.Type,
                    Usage = operationParameter.Usage
                });
            }

            foreach(var peopleAssignment in evt.Escalation.Notification.PeopleAssignments)
            {
                kvp.Value.Notification.PeopleAssignments.Add(new PeopleAssignmentDefinition
                {
                    Type = peopleAssignment.Type,
                    Usage = peopleAssignment.Usage,
                    Value = peopleAssignment.Value
                });
            }

            foreach(var presentationElt in evt.Escalation.Notification.PresentationElements)
            {
                kvp.Value.Notification.PresentationElements.Add(new PresentationElementDefinition
                {
                    ContentType = presentationElt.ContentType,
                    Value = presentationElt.Value,
                    Language = presentationElt.Language,
                    Usage = presentationElt.Usage
                });
            }

            foreach(var presentationParameter in evt.Escalation.Notification.PresentationParameters)
            {
                kvp.Value.Notification.PresentationParameters.Add(new PresentationParameter
                {
                    Expression = presentationParameter.Expression,
                    Name = presentationParameter.Name,
                    Type = presentationParameter.Type
                });
            }

            foreach(var toPart in evt.Escalation.ToParts)
            {
                kvp.Value.ToParts.Add(new ToPart
                {
                    Expression = toPart.Expression,
                    Name = toPart.Name
                });
            }

            UpdateDateTime = evt.UpdateDateTime;
            Version = evt.Version;
        }

        private void Handle(HumanTaskDefEscalationCompletionDeadlineUpdatedEvent evt)
        {
            var kvp = CheckEscalation(DeadLines.Where(_ => _.Usage == DeadlineUsages.COMPLETION).ToList(), 
                evt.CompletionDeadLineId, evt.EscalationId, 
                Global.UnknownCompletionDeadline);
            kvp.Value.Condition = evt.Escalation.Condition;
            kvp.Value.Notification = evt.Escalation.Notification;
            kvp.Value.ToParts = evt.Escalation.ToParts;
            UpdateDateTime = evt.UpdateDateTime;
            Version = evt.Version;
        }

        private void Handle(HumanTaskDefRenderingUpdatedEvent evt)
        {
            RenderingElements.Clear();
            foreach(var renderingElt in evt.RenderingElements)
            {
                RenderingElements.Add(new RenderingElement
                {
                    Default = renderingElt.Default,
                    Labels = renderingElt.Labels.Select(_ => new Translation
                    {
                        Language = _.Language,
                        Value = _.Value
                    }).ToList(),
                    Values = renderingElt.Values.Select(_ => new OptionValue
                    {
                        DisplayNames = _.DisplayNames.Select(d => new Translation
                        {
                            Language = d.Language,
                            Value = d.Value
                        }).ToList()
                    }).ToList(),
                    ValueType = renderingElt.ValueType,
                    XPath = renderingElt.XPath
                });
            }

            RenderingElements = evt.RenderingElements;
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
