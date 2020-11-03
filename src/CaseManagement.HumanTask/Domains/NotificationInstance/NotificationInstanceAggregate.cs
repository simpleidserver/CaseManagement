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
            PeopleAssignment = new NotificationInstancePeopleAssignment();
        }

        public string NotificationName { get; set; }
        public int Priority { get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime UpdateDateTime { get; set; }
        public NotificationInstanceStatus Status { get; set; }
        public Dictionary<string, string> OperationParameters { get; set; }
        public PresentationElementInstance PresentationElement { get; set; }
        public NotificationInstancePeopleAssignment PeopleAssignment { get; set; }
        public NotificationRendering Rendering { get; set; }

        public static NotificationInstanceAggregate New(string id, 
            int priority,
            string notificationName, 
            Dictionary<string, string> operationParameters, 
            PresentationElementInstance presentationElement, 
            NotificationInstancePeopleAssignment peopleAssignment, 
            NotificationRendering rendering)
        {
            var result = new NotificationInstanceAggregate();
            var evt = new NotificationInstanceCreatedEvent(Guid.NewGuid().ToString(), id, 0, notificationName, priority, operationParameters, presentationElement, peopleAssignment, rendering, DateTime.UtcNow);
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
                PresentationElement = (PresentationElementInstance)PresentationElement?.Clone(),
                PeopleAssignment = (NotificationInstancePeopleAssignment)PeopleAssignment?.Clone(),
                Rendering = (NotificationRendering)Rendering?.Clone()
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
            PresentationElement = evt.PresentationElement;
            PeopleAssignment = evt.PeopleAssignment;
            Rendering = evt.Rendering;
            CreateDateTime = evt.CreateDateTime;
        }

        #endregion
    }
}
