using CaseManagement.Common.Domains;
using System;
using System.Collections.Generic;

namespace CaseManagement.HumanTask.Domains
{
    public class HumanTaskInstanceCreatedEvent : DomainEvent
    {
        public HumanTaskInstanceCreatedEvent(string id, string aggregateId, int version, string humanTaskDefName, DateTime createDateTime, Dictionary<string, string> operationParameters, TaskPeopleAssignment peopleAssignment, int priority, DateTime? activationDeferralTime = null, DateTime? expirationTime = null) : base(id, aggregateId, version)
        {
            HumanTaskDefName = humanTaskDefName;
            CreateDateTime = createDateTime;
            OperationParameters = operationParameters;
            PeopleAssignment = peopleAssignment;
            Priority = priority;
            ActivationDeferralTime = activationDeferralTime;
            ExpirationTime = expirationTime;
        }

        public string HumanTaskDefName { get; set; }
        public DateTime CreateDateTime { get; set; }
        public Dictionary<string, string> OperationParameters { get; set; }
        public TaskPeopleAssignment PeopleAssignment { get; set; }
        public int Priority { get; set; }
        public DateTime? ActivationDeferralTime { get; set; }
        public DateTime? ExpirationTime { get; set; }
    }
}
