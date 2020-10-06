using CaseManagement.Common.Domains;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CaseManagement.HumanTask.Domains
{
    [DebuggerDisplay("Create human task instance")]
    public class HumanTaskInstanceCreatedEvent : DomainEvent
    {
        public HumanTaskInstanceCreatedEvent(
            string id,
            string aggregateId, 
            int version, 
            string humanTaskDefName, 
            DateTime createDateTime, 
            Dictionary<string, string> inputParameters,
            HumanTaskInstancePeopleAssignment peopleAssignment, 
            int priority, 
            string userPrincipal, 
            List<HumanTaskInstanceDeadLine> deadLines, 
            PresentationElementInstance presentationElement,
            HumanTaskInstanceComposition composition,
            Operation operation,
            CompletionBehavior completion,
            DateTime? activationDeferralTime = null, 
            DateTime? expirationTime = null) : base(id, aggregateId, version)
        {
            HumanTaskDefName = humanTaskDefName;
            CreateDateTime = createDateTime;
            InputParameters = inputParameters;
            PeopleAssignment = peopleAssignment;
            Priority = priority;
            UserPrincipal = userPrincipal;
            DeadLines = deadLines;
            PresentationElement = presentationElement;
            Composition = composition;
            Operation = operation;
            Completion = completion;
            ActivationDeferralTime = activationDeferralTime;
            ExpirationTime = expirationTime;
        }

        public string HumanTaskDefName { get; set; }
        public DateTime CreateDateTime { get; set; }
        public Dictionary<string, string> InputParameters { get; set; }
        public HumanTaskInstancePeopleAssignment PeopleAssignment { get; set; }
        public int Priority { get; set; }
        public string UserPrincipal { get; set; }
        public List<HumanTaskInstanceDeadLine> DeadLines { get; set; }
        public PresentationElementInstance PresentationElement { get; set; }
        public HumanTaskInstanceComposition Composition { get; set; }
        public Operation Operation { get; set; }
        public CompletionBehavior Completion { get; set; }
        public DateTime? ActivationDeferralTime { get; set; }
        public DateTime? ExpirationTime { get; set; }
    }
}
