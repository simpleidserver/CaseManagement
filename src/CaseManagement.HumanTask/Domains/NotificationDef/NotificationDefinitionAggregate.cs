using CaseManagement.Common.Domains;
using CaseManagement.Common.Exceptions;
using CaseManagement.HumanTask.Resources;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CaseManagement.HumanTask.Domains
{
    public class NotificationDefinitionAggregate : BaseAggregate
    {
        public NotificationDefinitionAggregate()
        {
            OperationParameters = new List<Parameter>();
            PeopleAssignments = new List<PeopleAssignmentDefinition>();
            PresentationElements = new List<PresentationElementDefinition>();
            PresentationParameters = new List<PresentationParameter>();
        }

        public string Name { get; set; }
        public int NbInstances { get; set; }
        /// <summary>
        /// This element is used to specify the priority of the notification.
        /// </summary>
        public int Priority { get; set; }
        public string Rendering { get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime UpdateDateTime { get; set; }
        /// <summary>
        /// This element is used to specify the operation used to invoke the notification. 
        /// </summary>
        public ICollection<Parameter> OperationParameters { get; set; }
        public ICollection<Parameter> InputParameters { get => OperationParameters.Where(_ => _.Usage == ParameterUsages.INPUT).ToList(); }
        public ICollection<Parameter> OutputParameters { get => OperationParameters.Where(_ => _.Usage == ParameterUsages.OUTPUT).ToList(); }
        /// <summary>
        /// This element is used to specify people assigned to the notification. 
        /// </summary>
        public ICollection<PeopleAssignmentDefinition> PeopleAssignments { get; set; }
        /// <summary>
        ///  This element is used to specify different information used to display the task in a task list, such as name, subject and description.
        /// </summary>
        public ICollection<PresentationElementDefinition> PresentationElements { get; set; }
        public ICollection<PresentationParameter> PresentationParameters { get; set; }

        public static NotificationDefinitionAggregate New(string name)
        {
            var evt = new NotificationDefinitionAddedEvent(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), 0, name, DateTime.UtcNow);
            var result = new NotificationDefinitionAggregate();
            result.Handle(evt);
            return result;
        }

        public void UpdateInfo(string name, int priority)
        {
            var evt = new NotificationDefinitionUpdatedEvent(Guid.NewGuid().ToString(), AggregateId, Version + 1, name, priority, DateTime.UtcNow);
            Handle(evt);
        }

        public string AddOperationParameter(Parameter parameter)
        {
            var parameterId = Guid.NewGuid().ToString();
            parameter.Id = parameterId;
            var evt = new NotificationDefinitionParameterAddedEvent(Guid.NewGuid().ToString(), AggregateId, Version + 1, parameter, DateTime.UtcNow);
            Handle(evt);
            return parameter.Id;
        }

        public void DeleteOperationParameter(string parameterId)
        {
            var evt = new NotificationDefinitionParameterRemovedEvent(Guid.NewGuid().ToString(), AggregateId, Version + 1, parameterId, DateTime.UtcNow);
            Handle(evt);
        }

        public string Assign(PeopleAssignmentDefinition peopleAssignment)
        {
            peopleAssignment.Id = Guid.NewGuid().ToString();
            var evt = new NotificationDefinitionPeopleAssignedEvent(Guid.NewGuid().ToString(), AggregateId, Version + 1, peopleAssignment, DateTime.UtcNow);
            Handle(evt);
            return peopleAssignment.Id;
        }

        public void RemoveAssignment(string assignmentId)
        {
            var evt = new NotificationDefinitionPeopleAssignmentRemovedEvent(Guid.NewGuid().ToString(), AggregateId, Version + 1, assignmentId, DateTime.UtcNow);
            Handle(evt);
        }

        public void AddPresentationElement(PresentationElementDefinition presentationElement)
        {
            var evt = new NotificationDefinitionPresentationElementAddedEvent(Guid.NewGuid().ToString(), AggregateId, Version + 1, presentationElement, DateTime.UtcNow);
            Handle(evt);
        }

        public void DeletePresentationElement(PresentationElementUsages usage, string language)
        {
            var evt = new NotificationDefinitionPresentationElementRemovedEvent(Guid.NewGuid().ToString(), AggregateId, Version + 1, usage, language, DateTime.UtcNow);
            Handle(evt);
        }

        public void AddPresentationParameter(PresentationParameter presentationParameter)
        {
            var evt = new NotificationDefinitionPresentationParameterAddedEvent(Guid.NewGuid().ToString(), AggregateId, Version + 1, presentationParameter, DateTime.UtcNow);
            Handle(evt);
        }

        public void DeletePresentationParameter(string name)
        {
            var evt = new NotificationDefinitionPresentationParameterRemovedEvent(Guid.NewGuid().ToString(), AggregateId, Version + 1, name, DateTime.UtcNow);
            Handle(evt);
        }

        public override void Handle(dynamic evt)
        {
            Handle(evt);
        }

        private void Handle(NotificationDefinitionAddedEvent evt)
        {
            AggregateId = evt.AggregateId;
            Name = evt.Name;
            Version = evt.Version;
            CreateDateTime = evt.CreateDateTime;
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

        private void Handle(NotificationDefinitionPeopleAssignedEvent evt)
        {
            PeopleAssignments.Add(evt.PeopleAssignment);
            UpdateDateTime = evt.CreateDateTime;
            Version = evt.Version;
        }

        private void Handle(NotificationDefinitionPeopleAssignmentRemovedEvent evt)
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

        private void Handle(NotificationDefinitionPresentationElementAddedEvent evt)
        {
            if (PresentationElements.Any(_ => _.Usage == evt.PresentationElement.Usage && _.Language == evt.PresentationElement.Language))
            {
                throw new AggregateValidationException(new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("validation", Global.PresentationElementExists)
                });
            }

            PresentationElements.Add(evt.PresentationElement);
            UpdateDateTime = evt.ReceptionDate;
            Version = evt.Version;
        }

        private void Handle(NotificationDefinitionPresentationElementRemovedEvent evt)
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

        private void Handle(NotificationDefinitionPresentationParameterAddedEvent evt)
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

        private void Handle(NotificationDefinitionPresentationParameterRemovedEvent evt)
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

        private void Handle(NotificationDefinitionUpdatedEvent evt)
        {
            Name = evt.Name;
            Priority = evt.Priority;
            UpdateDateTime = evt.UpdateDateTime;
            Version = evt.Version;
        }

        public override object Clone()
        {
            return new NotificationDefinitionAggregate
            {
                AggregateId = AggregateId,
                Version = Version,
                UpdateDateTime = UpdateDateTime,
                NbInstances = NbInstances,
                CreateDateTime = CreateDateTime,
                Name = Name,
                Priority = Priority,
                Rendering = Rendering,
                PeopleAssignments = PeopleAssignments.Select(_ => (PeopleAssignmentDefinition)_.Clone()).ToList(),
                PresentationElements = PresentationElements.Select(_ => (PresentationElementDefinition)_.Clone()).ToList(),
                PresentationParameters = PresentationParameters.Select(_ => (PresentationParameter)_.Clone()).ToList(),
                OperationParameters = OperationParameters.Select(_ => (Parameter)_.Clone()).ToList()
            };
        }
    }
}
