using CaseManagement.Common.Domains;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CaseManagement.HumanTask.Domains
{
    [DebuggerDisplay("Create notification")]
    public class NotificationInstanceCreatedEvent : DomainEvent
    {
        public NotificationInstanceCreatedEvent(string id, string aggregateId, int version, string notificationName, Dictionary<string, string> operationParameters, PresentationElementInstance presentationElement, NotificationInstancePeopleAssignment peopleAssignment, NotificationRendering rendering, DateTime createDateTime) : base(id, aggregateId, version)
        {
            NotificationName = notificationName;
            OperationParameters = operationParameters;
            PresentationElement = presentationElement;
            PeopleAssignment = peopleAssignment;
            Rendering = rendering;
            CreateDateTime = createDateTime;
        }

        public string NotificationName { get; set; }
        public Dictionary<string, string> OperationParameters { get; set; }
        public PresentationElementInstance PresentationElement { get; set; }
        public NotificationInstancePeopleAssignment PeopleAssignment { get; set; }
        public NotificationRendering Rendering { get; set; }
        public DateTime CreateDateTime { get; set; }
    }
}
