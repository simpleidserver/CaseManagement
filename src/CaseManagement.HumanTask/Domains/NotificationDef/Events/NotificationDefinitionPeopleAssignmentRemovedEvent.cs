using CaseManagement.Common.Domains;
using System;
using System.Diagnostics;

namespace CaseManagement.HumanTask.Domains
{
    [DebuggerDisplay("People assignment is removed")]
    public class NotificationDefinitionPeopleAssignmentRemovedEvent : DomainEvent
    {
        public NotificationDefinitionPeopleAssignmentRemovedEvent(string id, string aggregateId, int version, string assignmentId, DateTime removedDateTime) : base(id, aggregateId, version)
        {
            AssignmentId = assignmentId;
            RemovedDateTime = removedDateTime;
        }

        public string AssignmentId { get; set; }
        public DateTime RemovedDateTime { get; set; }
    }
}
