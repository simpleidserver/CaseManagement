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
            ICollection<PeopleAssignmentInstance> peopleAssignments, 
            int priority, 
            string userPrincipal, 
            ICollection<HumanTaskInstanceDeadLine> deadLines, 
            ICollection<PresentationElementInstance> presentationElements,
            CompositionTypes type,
            InstantiationPatterns instantiationPattern,
            ICollection<HumanTaskInstanceSubTask> subTasks,
            ICollection<Parameter> operationParameters,
            CompletionBehaviors completionBehavior,
            ICollection<Completion> completions,
            ICollection<RenderingElement> renderingElts,
            ICollection<CallbackOperation> callbackOperations,
            DateTime? activationDeferralTime = null, 
            DateTime? expirationTime = null) : base(id, aggregateId, version)
        {
            HumanTaskDefName = humanTaskDefName;
            CreateDateTime = createDateTime;
            InputParameters = inputParameters;
            PeopleAssignments = peopleAssignments;
            Priority = priority;
            UserPrincipal = userPrincipal;
            DeadLines = deadLines;
            PresentationElements = presentationElements;
            Type = type;
            InstantiationPattern = instantiationPattern;
            SubTasks = subTasks;
            OperationParameters = operationParameters;
            CompletionBehavior = completionBehavior;
            Completions = completions;
            RenderingElts = renderingElts;
            CallbackOperations = callbackOperations;
            ActivationDeferralTime = activationDeferralTime;
            ExpirationTime = expirationTime;
        }

        public string HumanTaskDefName { get; set; }
        public DateTime CreateDateTime { get; set; }
        public Dictionary<string, string> InputParameters { get; set; }
        public ICollection<PeopleAssignmentInstance> PeopleAssignments { get; set; }
        public int Priority { get; set; }
        public string UserPrincipal { get; set; }
        public ICollection<HumanTaskInstanceDeadLine> DeadLines { get; set; }
        public ICollection<PresentationElementInstance> PresentationElements { get; set; }
        public CompositionTypes Type { get; set; }
        public InstantiationPatterns InstantiationPattern { get; set; }
        public ICollection<HumanTaskInstanceSubTask> SubTasks { get; set; }
        public ICollection<Parameter> OperationParameters { get; set; }
        public CompletionBehaviors CompletionBehavior { get; set; }
        public ICollection<Completion> Completions { get; set; }
        public ICollection<RenderingElement> RenderingElts { get; set; }
        public ICollection<CallbackOperation> CallbackOperations { get; set; }
        public DateTime? ActivationDeferralTime { get; set; }
        public DateTime? ExpirationTime { get; set; }
    }
}
