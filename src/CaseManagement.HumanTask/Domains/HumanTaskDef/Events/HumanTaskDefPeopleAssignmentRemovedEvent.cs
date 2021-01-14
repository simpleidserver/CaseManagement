using CaseManagement.Common.Domains;
using System;
using System.Diagnostics;

namespace CaseManagement.HumanTask.Domains.HumanTaskDef.Events
{
    [DebuggerDisplay("People assignment is removed")]
    public class HumanTaskDefPeopleAssignmentRemovedEvent : DomainEvent
    {
        public HumanTaskDefPeopleAssignmentRemovedEvent(string id, string aggregateId, int version, string assignmentId, DateTime removedDateTime) : base(id, aggregateId, version)
        {
            AssignmentId = assignmentId;
            RemovedDateTime = removedDateTime;
        }

        public string AssignmentId { get; set; }
        public DateTime RemovedDateTime { get; set; }
    }
}
