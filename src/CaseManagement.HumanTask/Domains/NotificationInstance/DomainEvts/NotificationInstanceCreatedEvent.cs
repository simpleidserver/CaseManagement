using CaseManagement.Common.Domains;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CaseManagement.HumanTask.Domains
{
    [DebuggerDisplay("Create notification")]
    public class NotificationInstanceCreatedEvent : DomainEvent
    {
        public NotificationInstanceCreatedEvent(string id, string aggregateId, int version, string notificationName, int priority, Dictionary<string, string> operationParameters, ICollection<PresentationElementInstance> presentationElements, ICollection<PeopleAssignmentInstance> peopleAssignments, string rendering, DateTime createDateTime) : base(id, aggregateId, version)
        {
            NotificationName = notificationName;
            Priority = priority;
            OperationParameters = operationParameters;
            PresentationElements = presentationElements;
            PeopleAssignments = peopleAssignments;
            Rendering = rendering;
            CreateDateTime = createDateTime;
        }

        public string NotificationName { get; set; }
        public int Priority { get; set; }
        public Dictionary<string, string> OperationParameters { get; set; }
        public ICollection<PresentationElementInstance> PresentationElements { get; set; }
        public ICollection<PeopleAssignmentInstance> PeopleAssignments { get; set; }
        public string Rendering { get; set; }
        public DateTime CreateDateTime { get; set; }
    }
}
