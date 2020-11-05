using CaseManagement.Common.Domains;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CaseManagement.HumanTask.Domains
{
    public class NotificationInstanceAggregate : BaseAggregate
    {
        public NotificationInstanceAggregate()
        {
            OperationParameters = new Dictionary<string, string>();
            PeopleAssignments = new List<PeopleAssignmentInstance>();
        }

        public string NotificationName { get; set; }
        public int Priority { get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime UpdateDateTime { get; set; }
        public NotificationInstanceStatus Status { get; set; }
        public Dictionary<string, string> OperationParameters { get; set; }
        public ICollection<PresentationElementInstance> PresentationElements { get; set; }
        public ICollection<PresentationElementInstance> Names { get => PresentationElements.Where(_ => _.Usage == PresentationElementUsages.NAME).ToList(); }
        public ICollection<PresentationElementInstance> Subjects { get => PresentationElements.Where(_ => _.Usage == PresentationElementUsages.SUBJECT).ToList(); }
        public ICollection<PresentationElementInstance> Descriptions { get => PresentationElements.Where(_ => _.Usage == PresentationElementUsages.DESCRIPTION).ToList(); }
        public ICollection<PeopleAssignmentInstance> PeopleAssignments { get; set; }
        public string Rendering { get; set; }

        public static NotificationInstanceAggregate New(string id, 
            int priority,
            string notificationName, 
            Dictionary<string, string> operationParameters,
            ICollection<PresentationElementInstance> presentationElements,
            ICollection<PeopleAssignmentInstance> peopleAssignments, 
            string rendering)
        {
            var result = new NotificationInstanceAggregate();
            var evt = new NotificationInstanceCreatedEvent(Guid.NewGuid().ToString(), id, 0, notificationName, priority, operationParameters, presentationElements, peopleAssignments, rendering, DateTime.UtcNow);
            result.Handle(evt);
            result.DomainEvents.Add(evt);
            return result;
        }

        public override object Clone()
        {
            return new NotificationInstanceAggregate
            {
                AggregateId = AggregateId,
                Version = Version,
                NotificationName = NotificationName,
                Priority = Priority,
                CreateDateTime = CreateDateTime,
                UpdateDateTime = UpdateDateTime,
                Status = Status,
                OperationParameters = OperationParameters.ToDictionary(kvp => kvp.Key, kvp => kvp.Value),
                PresentationElements = PresentationElements.Select(_ => (PresentationElementInstance)_.Clone()).ToList(),
                PeopleAssignments = PeopleAssignments.Select(_ => (PeopleAssignmentInstance)_.Clone()).ToList(),
                Rendering = Rendering
            };
        }

        public override void Handle(dynamic evt)
        {
            Handle(evt);
        }

        #region Handle domain events

        private void Handle(NotificationInstanceCreatedEvent evt)
        {
            AggregateId = evt.AggregateId;
            Version = evt.Version;
            NotificationName = evt.NotificationName;
            Priority = evt.Priority;
            Status = NotificationInstanceStatus.READY;
            OperationParameters = evt.OperationParameters;
            PresentationElements = evt.PresentationElements;
            PeopleAssignments = evt.PeopleAssignments;
            Rendering = evt.Rendering;
            CreateDateTime = evt.CreateDateTime;
        }

        #endregion
    }
}
