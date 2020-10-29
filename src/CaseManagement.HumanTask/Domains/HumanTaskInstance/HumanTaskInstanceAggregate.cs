using CaseManagement.Common.Domains;
using CaseManagement.Common.Exceptions;
using CaseManagement.HumanTask.Resources;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace CaseManagement.HumanTask.Domains
{
    public class HumanTaskInstanceAggregate : BaseAggregate
    {
        public HumanTaskInstanceAggregate()
        {
            InputParameters = new Dictionary<string, string>();
            OutputParameters = new Dictionary<string, string>();
            Operation = new Operation();
            EventHistories = new ConcurrentBag<HumanTaskInstanceEventHistory>();
            DeadLines = new List<HumanTaskInstanceDeadLine>();
            PresentationElement = new PresentationElementInstance();
            CompletionBehavior = new CompletionBehavior();
        }

        public string HumanTaskInstanceId { get; set; }
        public string ParentHumanTaskName { get; set; }
        public string ParentHumanTaskId { get; set; }
        public HumanTaskInstanceStatus Status { get; set; }
        public string HumanTaskDefName { get; set; }
        public string ActualOwner { get; set; }
        public Dictionary<string, string> InputParameters { get; set; }
        public Dictionary<string, string> OutputParameters { get; set; }
        public Operation Operation { get; set; }
        public int Priority { get; set; }
        public DateTime? ActivationDeferralTime { get; set; }
        public DateTime? ExpirationTime { get; set; }
        public Rendering Rendering { get; set; }
        public HumanTaskInstancePeopleAssignment PeopleAssignment { get; set; }
        public PresentationElementInstance PresentationElement { get; set; }
        public ConcurrentBag<HumanTaskInstanceEventHistory> EventHistories { get; set; }
        public ICollection<HumanTaskInstanceDeadLine> DeadLines { get; set; }
        public HumanTaskInstanceComposition  Composition { get; set; }
        public CompletionBehavior CompletionBehavior { get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime UpdateDateTime { get; set; }

        public static HumanTaskInstanceAggregate New(
            string id,
            string userPrincipal, 
            string humanTaskDefName, 
            Dictionary<string, string> inputParameters, 
            HumanTaskInstancePeopleAssignment peopleAssignment, 
            int priority, 
            DateTime? activationDeferralTime, 
            DateTime? expirationTime, 
            List<HumanTaskInstanceDeadLine> deadLines,
            PresentationElementInstance presentationElementInstance,
            HumanTaskInstanceComposition composition,
            Operation operation,
            CompletionBehavior completion,
            Rendering rendering)
        {
            var evt = new HumanTaskInstanceCreatedEvent(
                Guid.NewGuid().ToString(), 
                id, 
                0, 
                humanTaskDefName, 
                DateTime.UtcNow, 
                inputParameters, 
                peopleAssignment, 
                priority, 
                userPrincipal, 
                deadLines, 
                presentationElementInstance,
                composition,
                operation,
                completion,
                rendering,
                activationDeferralTime,
                expirationTime);
            var result = new HumanTaskInstanceAggregate();
            result.Handle(evt);
            result.DomainEvents.Add(evt);
            return result;
        }

        public override object Clone()
        {
            return new HumanTaskInstanceAggregate
            {
                AggregateId = AggregateId,
                Version = Version,
                Status = Status,
                HumanTaskDefName = HumanTaskDefName,
                ActualOwner = ActualOwner,
                Operation = (Operation)Operation?.Clone(),
                InputParameters = InputParameters.ToDictionary(kvp => kvp.Key, kvp => kvp.Value),
                OutputParameters = OutputParameters.ToDictionary(kvp => kvp.Key, kvp => kvp.Value),
                Priority = Priority,
                ActivationDeferralTime = ActivationDeferralTime,
                ExpirationTime = ExpirationTime,
                PresentationElement = (PresentationElementInstance)PresentationElement?.Clone(),
                PeopleAssignment = (HumanTaskInstancePeopleAssignment)PeopleAssignment?.Clone(),
                EventHistories = new ConcurrentBag<HumanTaskInstanceEventHistory>(EventHistories.Select(_ => (HumanTaskInstanceEventHistory)_.Clone())),
                DeadLines = DeadLines.Select(_ => (HumanTaskInstanceDeadLine)_.Clone()).ToList(),
                CreateDateTime = CreateDateTime,
                UpdateDateTime = UpdateDateTime,
                Composition = (HumanTaskInstanceComposition)Composition?.Clone(),
                HumanTaskInstanceId = HumanTaskInstanceId,
                ParentHumanTaskName = ParentHumanTaskName,
                ParentHumanTaskId = ParentHumanTaskId,
                Rendering = (Rendering)Rendering?.Clone(),
                CompletionBehavior = (CompletionBehavior)CompletionBehavior?.Clone()
            };
        }

        public string GetStreamName()
        {
            return GetStreamName(AggregateId);
        }

        #region Operations

        /// <summary>
        /// Activate the human task instance.
        /// From Created to Ready.
        /// </summary>
        public void Activate(string userPrincipal)
        {
            var evt = new HumanTaskInstanceActivatedEvent(Guid.NewGuid().ToString(), AggregateId, Version + 1, userPrincipal, DateTime.UtcNow);
            Handle(evt);
            DomainEvents.Add(evt);
        }

        /// <summary>
        /// Assign potential owners.
        /// </summary>
        public void Nominate(string userPrincipal, ICollection<string> groupNames, ICollection<string> userIdentifiers)
        {
            var evt = new HumanTaskInstanceNominatedEvent(Guid.NewGuid().ToString(), AggregateId, Version + 1, groupNames, userIdentifiers, userPrincipal, DateTime.UtcNow);
            Handle(evt);
            DomainEvents.Add(evt);
        }

        /// <summary>
        /// Claim responsibility for a task.
        /// </summary>
        /// <param name="userPrincipal"></param>
        public void Claim(string userPrincipal)
        {
            var evt = new HumanTaskInstanceClaimedEvent(Guid.NewGuid().ToString(), AggregateId, Version + 1, userPrincipal, DateTime.UtcNow);
            Handle(evt);
            DomainEvents.Add(evt);
        }

        /// <summary>
        /// Start human task instance.
        /// </summary>
        /// <param name="userPrincipal"></param>
        public void Start(string userPrincipal)
        {
            var evt = new HumanTaskInstanceStartedEvent(Guid.NewGuid().ToString(), AggregateId, Version + 1, userPrincipal, DateTime.UtcNow);
            Handle(evt);
            DomainEvents.Add(evt);
        }

        /// <summary>
        /// Remove the deadline.
        /// </summary>
        public void RemoveDeadLine(string name, HumanTaskInstanceDeadLineTypes type)
        {
            var evt = new HumanTaskInstanceDeadLineRemovedEvent(Guid.NewGuid().ToString(), AggregateId, Version + 1, name, type, DateTime.UtcNow);
            Handle(evt);
            DomainEvents.Add(evt);
        }

        /// <summary>
        /// Set parent.
        /// </summary>
        /// <param name=""></param>
        public void SetParent(string parentName, string parentId)
        {
            var evt = new HumanTaskInstanceParentUpdatedEvent(Guid.NewGuid().ToString(), AggregateId, Version + 1, parentName, parentId, DateTime.UtcNow);
            Handle(evt);
            DomainEvents.Add(evt);
        }

        /// <summary>
        /// Skip the human task instance.
        /// </summary>
        /// <param name="userPrincipal"></param>
        public void Skip(string userPrincipal)
        {
            var evt = new HumanTaskInstanceSkippedEvent(Guid.NewGuid().ToString(), AggregateId, Version + 1, userPrincipal, DateTime.UtcNow);
            Handle(evt);
            DomainEvents.Add(evt);
        }

        /// <summary>
        /// Complete the human task instance.
        /// </summary>
        /// <param name="outputParameters"></param>
        public void Complete(Dictionary<string, string> outputParameters, string userPrincipal)
        {
            var evt = new HumanTaskInstanceCompletedEvent(Guid.NewGuid().ToString(), AggregateId, Version + 1, outputParameters, userPrincipal, DateTime.UtcNow);
            Handle(evt);
            DomainEvents.Add(evt);
        }

        #endregion

        public static string GetStreamName(string id)
        {
            return $"human-task-instance-{id}";
        }

        #region Handle events

        public override void Handle(dynamic evt)
        {
            Handle(evt);
        }

        private void Handle(HumanTaskInstanceCreatedEvent evt)
        {
            using(var evtHistory = AddEventHistory(evt.Id, evt.CreateDateTime, HumanTaskInstanceEventTypes.CREATED, evt.UserPrincipal, evt.InputParameters))
            {
                AggregateId = evt.AggregateId;
                Status = HumanTaskInstanceStatus.CREATED;
                HumanTaskDefName = evt.HumanTaskDefName;
                Priority = evt.Priority;
                ActivationDeferralTime = evt.ActivationDeferralTime;
                ExpirationTime = evt.ExpirationTime;
                PeopleAssignment = evt.PeopleAssignment;
                InputParameters = evt.InputParameters;
                PresentationElement = evt.PresentationElement;
                DeadLines = evt.DeadLines;
                Composition = (HumanTaskInstanceComposition)evt.Composition?.Clone();
                CompletionBehavior = (CompletionBehavior)evt.Completion?.Clone();
                Operation = (Operation)evt.Operation?.Clone();
                Rendering = evt.Rendering;
                UpdateDateTime = evt.CreateDateTime;
                CreateDateTime = evt.CreateDateTime;
                Version = evt.Version;
                UpdateStatus();
            }
        }

        private void Handle(HumanTaskInstanceActivatedEvent evt)
        {
            var errors = new List<KeyValuePair<string, string>>();
            if (Status != HumanTaskInstanceStatus.CREATED)
            {
                errors.Add(new KeyValuePair<string, string>("validation", string.Format(Global.BadState, "Created")));
            }

            if (ActivationDeferralTime != null && DateTime.UtcNow <= ActivationDeferralTime)
            {
                errors.Add(new KeyValuePair<string, string>("validation", Global.NotActivated));
            }

            if (errors.Any())
            {
                throw new AggregateValidationException(errors);
            }

            using (var evtHistory = AddEventHistory(evt.Id, evt.UpdateDateTime, HumanTaskInstanceEventTypes.ACTIVATE, evt.UserPrincipal))
            {
                UpdateStatus();
                UpdateDateTime = evt.UpdateDateTime;
                Version = evt.Version;
            }
        }

        private void Handle(HumanTaskInstanceNominatedEvent evt)
        {
            var assign = new PeopleAssignmentInstance();
            if (evt.GroupNames != null && evt.GroupNames.Any())
            {
                assign.Type = PeopleAssignmentTypes.GROUPNAMES;
                assign.Values = evt.GroupNames;
            }
            else
            {
                assign.Type = PeopleAssignmentTypes.USERIDENTIFIERS;
                assign.Values = evt.UserIdentifiers;
            }

            using (var evtHistory = AddEventHistory(evt.Id, evt.UpdateDateTime, HumanTaskInstanceEventTypes.ACTIVATE, evt.UserPrincipal))
            {
                PeopleAssignment = new HumanTaskInstancePeopleAssignment
                {
                    PotentialOwner = assign
                };
                UpdateStatus();
                UpdateDateTime = evt.UpdateDateTime;
                Version = evt.Version;
            }
        }

        private void Handle(HumanTaskInstanceClaimedEvent evt)
        {
            using (var evtHistory = AddEventHistory(evt.Id, evt.ExecutionDateTime, HumanTaskInstanceEventTypes.CLAIM, evt.UserPrincipal))
            {
                Status = HumanTaskInstanceStatus.RESERVED;
                ActualOwner = evt.UserPrincipal;
                UpdateDateTime = evt.ExecutionDateTime;
                Version = evt.Version;
            }
        }

        private void Handle(HumanTaskInstanceStartedEvent evt)
        {
            using (var evtHistory = AddEventHistory(evt.Id, evt.ExecutionDateTime, HumanTaskInstanceEventTypes.START, evt.UserPrincipal))
            {
                Status = HumanTaskInstanceStatus.INPROGRESS;
                DeadLines = DeadLines.Where(_ => _.Type == HumanTaskInstanceDeadLineTypes.COMPLETION).ToList();
                ActualOwner = evt.UserPrincipal;
                UpdateDateTime = evt.ExecutionDateTime;
                Version = evt.Version;
            }
        }

        private void Handle(HumanTaskInstanceCompletedEvent evt)
        {
            using (var evtHistory = AddEventHistory(evt.Id, evt.UpdateDateTime, HumanTaskInstanceEventTypes.COMPLETE, evt.UserPrincipal, evt.OutputParameters))
            {
                DeadLines = new List<HumanTaskInstanceDeadLine>();
                Status = HumanTaskInstanceStatus.COMPLETED;
                OutputParameters = evt.OutputParameters;
                UpdateDateTime = evt.UpdateDateTime;
                Version = evt.Version;
            }
        }

        private void Handle(HumanTaskInstanceSkippedEvent evt)
        {
            using (var evtHistory = AddEventHistory(evt.Id, evt.UpdateDateTime, HumanTaskInstanceEventTypes.COMPLETE, evt.UserPrincipal))
            {
                DeadLines = new List<HumanTaskInstanceDeadLine>();
                Status = HumanTaskInstanceStatus.OBSOLETE;
                UpdateDateTime = evt.UpdateDateTime;
                Version = evt.Version;
            }
        }

        private void Handle(HumanTaskInstanceDeadLineRemovedEvent evt)
        {
            DeadLines = DeadLines.Where(_ => _.Name != evt.DeadLineName && _.Type != evt.DeadLineType).ToList();
            UpdateDateTime = evt.UpdateDateTime;
            Version = evt.Version;
        }

        private void Handle(HumanTaskInstanceParentUpdatedEvent evt)
        {
            ParentHumanTaskName = evt.ParentName;
            ParentHumanTaskId = evt.ParentId;
            UpdateDateTime = evt.UpdateDateTime;
            Version = evt.Version;
        }

        #endregion

        private void UpdateStatus()
        {
            if (Status == HumanTaskInstanceStatus.CREATED && PeopleAssignment != null && 
                PeopleAssignment.PotentialOwner != null &&
                PeopleAssignment.PotentialOwner.Values.Any())
            {
                if (ActivationDeferralTime != null)
                {
                    if (DateTime.UtcNow >= ActivationDeferralTime.Value)
                    {
                        Status = HumanTaskInstanceStatus.READY;
                    }
                }
                else
                {
                    Status = HumanTaskInstanceStatus.READY;
                }
            }

            if (Status == HumanTaskInstanceStatus.READY)
            {
                if (PeopleAssignment != null && 
                    PeopleAssignment.PotentialOwner != null && 
                    PeopleAssignment.PotentialOwner.Values.Count() == 1 && 
                    PeopleAssignment.PotentialOwner.Type == PeopleAssignmentTypes.USERIDENTIFIERS)
                {
                    Status = HumanTaskInstanceStatus.RESERVED;
                    ActualOwner = PeopleAssignment.PotentialOwner.Values.First();
                }
            }
        }

        private EventHistoryEnricher AddEventHistory(string evtId, DateTime evtDateTime, HumanTaskInstanceEventTypes evtType, string userPrincipal, object evtData = null)
        {
            return new EventHistoryEnricher(this, evtId, evtDateTime, evtType, userPrincipal, evtData);
        }

        private class EventHistoryEnricher : IDisposable
        {
            private bool _isCommitted;
            private readonly HumanTaskInstanceAggregate _instance;
            private readonly string _evtId;
            private readonly DateTime _evtDateTime;
            private readonly HumanTaskInstanceEventTypes _evtType;
            private readonly string _userPrincipal;
            private readonly object _evtData;
            private readonly string _startOwner;

            public EventHistoryEnricher(HumanTaskInstanceAggregate instance, string evtId, DateTime evtDateTime, HumanTaskInstanceEventTypes evtType, string userPrincipal, object evtData = null)
            {
                _evtId = evtId;
                _isCommitted = false;
                _instance = instance;
                _evtDateTime = evtDateTime;
                _evtType = evtType;
                _userPrincipal = userPrincipal;
                _evtData = evtData;
                _startOwner = instance.ActualOwner;
            }

            public void Dispose()
            {
                if (!_isCommitted)
                {
                    Commit();
                }
            }

            public void Commit()
            {
                _instance.EventHistories.Add(new HumanTaskInstanceEventHistory
                {
                    EventId = _evtId,
                    EndOwner = _instance.ActualOwner,
                    EventData = _evtData == null ? string.Empty : JsonConvert.SerializeObject(_evtData),
                    EventTime = _evtDateTime,
                    EventType = _evtType,
                    HumanTaskIdentifier = _instance.AggregateId,
                    StartOwner = _startOwner,
                    TaskStatus = _instance.Status,
                    UserPrincipal = _userPrincipal
                });
                _isCommitted = true;
            }
        }
    }
}
