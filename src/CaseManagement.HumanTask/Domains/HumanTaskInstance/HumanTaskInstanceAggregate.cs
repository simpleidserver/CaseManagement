using CaseManagement.Common.Domains;
using CaseManagement.Common.Exceptions;
using CaseManagement.HumanTask.Resources;
using Newtonsoft.Json;
using System;
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
            EventHistories = new List<HumanTaskInstanceEventHistory>();
            DeadLines = new List<HumanTaskInstanceDeadLine>();
            PresentationElements = new List<PresentationElementInstance>();
            Completions = new List<Completion>();
            SubTasks = new List<HumanTaskInstanceSubTask>();
            CallbackOperations = new List<CallbackOperation>();
        }

        public string HumanTaskInstanceId { get; set; }
        public string ParentHumanTaskName { get; set; }
        public string ParentHumanTaskId { get; set; }
        public HumanTaskInstanceStatus Status { get; set; }
        public string HumanTaskDefName { get; set; }
        public string ActualOwner { get; set; }
        public int Priority { get; set; }
        public DateTime? ActivationDeferralTime { get; set; }
        public DateTime? ExpirationTime { get; set; }
        public CompositionTypes Type { get; set; }
        public InstantiationPatterns InstantiationPattern { get; set; }
        public CompletionBehaviors CompletionBehavior { get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime UpdateDateTime { get; set; }
        public Dictionary<string, string> InputParameters { get; set; }
        public Dictionary<string, string> OutputParameters { get; set; }
        public ICollection<Parameter> OperationParameters { get; set; }
        public ICollection<Parameter> InputOperationParameters { get => OperationParameters.Where(_ => _.Usage == ParameterUsages.INPUT).ToList(); }
        public ICollection<Parameter> OutputOperationParameters { get => OperationParameters.Where(_ => _.Usage == ParameterUsages.OUTPUT).ToList(); }
        public string Rendering { get; set; }
        public ICollection<Completion> Completions { get; set; }
        public ICollection<PresentationElementInstance> PresentationElements { get; set; }
        public ICollection<PresentationElementInstance> Names { get => PresentationElements.Where(_ => _.Usage == PresentationElementUsages.NAME).ToList(); }
        public ICollection<PresentationElementInstance> Subjects { get => PresentationElements.Where(_ => _.Usage == PresentationElementUsages.SUBJECT).ToList(); }
        public ICollection<PresentationElementInstance> Descriptions { get => PresentationElements.Where(_ => _.Usage == PresentationElementUsages.DESCRIPTION).ToList(); }
        public ICollection<HumanTaskInstanceDeadLine> DeadLines { get; set; }
        public ICollection<PeopleAssignmentInstance> PeopleAssignments { get; set; }
        public ICollection<HumanTaskInstanceSubTask> SubTasks { get; set; }
        public ICollection<PeopleAssignmentInstance> PotentialOwners { get => PeopleAssignments.GetPotentialOwners().ToList(); }
        public ICollection<PeopleAssignmentInstance> BusinessAdministrators { get => PeopleAssignments.GetBusinessAdministrators().ToList(); }
        public ICollection<PeopleAssignmentInstance> ExcludedOwners { get => PeopleAssignments.GetExcludedOwners().ToList(); }
        public ICollection<PeopleAssignmentInstance> TaskInitiators { get => PeopleAssignments.GetTaskInitiators().ToList(); }
        public ICollection<PeopleAssignmentInstance> TaskStakeHolders { get => PeopleAssignments.GetTaskStakeHolders().ToList(); }
        public ICollection<HumanTaskInstanceEventHistory> EventHistories { get; set; }
        public ICollection<CallbackOperation> CallbackOperations { get; set; }

        public static HumanTaskInstanceAggregate New(
            string id,
            string userPrincipal, 
            string humanTaskDefName, 
            Dictionary<string, string> inputParameters, 
            ICollection<PeopleAssignmentInstance> peopleAssignments, 
            int priority, 
            DateTime? activationDeferralTime, 
            DateTime? expirationTime, 
            ICollection<HumanTaskInstanceDeadLine> deadLines,
            ICollection<PresentationElementInstance> presentationElements,
            CompositionTypes type,
            InstantiationPatterns instantiationPattern,
            ICollection<HumanTaskInstanceSubTask> subTasks,
            ICollection<Parameter> operationParameters,
            CompletionBehaviors completionBehavior,
            ICollection<Completion> completions,
            string rendering,
            ICollection<CallbackOperation> callbackOperations)
        {
            var evt = new HumanTaskInstanceCreatedEvent(
                Guid.NewGuid().ToString(), 
                id, 
                0, 
                humanTaskDefName, 
                DateTime.UtcNow, 
                inputParameters,
                peopleAssignments, 
                priority, 
                userPrincipal, 
                deadLines, 
                presentationElements,
                type,
                instantiationPattern,
                subTasks,
                operationParameters,
                completionBehavior,
                completions,
                rendering,
                callbackOperations,
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
                OperationParameters = OperationParameters.Select(_ => (Parameter)_.Clone()).ToList(),
                InputParameters = InputParameters.ToDictionary(kvp => kvp.Key, kvp => kvp.Value),
                OutputParameters = OutputParameters.ToDictionary(kvp => kvp.Key, kvp => kvp.Value),
                Priority = Priority,
                ActivationDeferralTime = ActivationDeferralTime,
                ExpirationTime = ExpirationTime,
                PresentationElements = PresentationElements.Select(_ => (PresentationElementInstance)_.Clone()).ToList(),
                PeopleAssignments = PeopleAssignments.Select(_ => (PeopleAssignmentInstance)_.Clone()).ToList(),
                EventHistories = EventHistories.Select(_ => (HumanTaskInstanceEventHistory)_.Clone()).ToList(),
                DeadLines = DeadLines.Select(_ => (HumanTaskInstanceDeadLine)_.Clone()).ToList(),
                CreateDateTime = CreateDateTime,
                UpdateDateTime = UpdateDateTime,
                SubTasks = SubTasks.Select(_ => (HumanTaskInstanceSubTask)_.Clone()).ToList(),
                InstantiationPattern = InstantiationPattern,
                Type = Type,
                HumanTaskInstanceId = HumanTaskInstanceId,
                ParentHumanTaskName = ParentHumanTaskName,
                ParentHumanTaskId = ParentHumanTaskId,
                Completions = Completions.Select(_ => (Completion)_.Clone()).ToList(),
                Rendering = Rendering,
                CallbackOperations = CallbackOperations.Select(_ => (CallbackOperation)_.Clone()).ToList(),
                CompletionBehavior = CompletionBehavior
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
        public void Nominate(string userPrincipal, IEnumerable<string> groupNames, IEnumerable<string> userIdentifiers)
        {
            var evt = new HumanTaskInstanceNominatedEvent(Guid.NewGuid().ToString(), AggregateId, Version + 1, userIdentifiers, groupNames, userPrincipal, DateTime.UtcNow);
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
        public void RemoveDeadLine(string name, DeadlineUsages usage)
        {
            var evt = new HumanTaskInstanceDeadLineRemovedEvent(Guid.NewGuid().ToString(), AggregateId, Version + 1, name, usage, DateTime.UtcNow);
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
                PeopleAssignments = evt.PeopleAssignments;
                InputParameters = evt.InputParameters;
                PresentationElements = evt.PresentationElements;
                DeadLines = evt.DeadLines;
                Type = evt.Type;
                SubTasks = evt.SubTasks;
                InstantiationPattern = evt.InstantiationPattern;
                CompletionBehavior = evt.CompletionBehavior;
                Completions = evt.Completions;
                OperationParameters = evt.OperationParameters;
                CallbackOperations = evt.CallbackOperations;
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
            using (var evtHistory = AddEventHistory(evt.Id, evt.UpdateDateTime, HumanTaskInstanceEventTypes.ACTIVATE, evt.UserPrincipal))
            {
                if (evt.UserIdentifiers != null && evt.UserIdentifiers.Any())
                {
                    foreach(var userIdentifier in evt.UserIdentifiers)
                    {
                        PeopleAssignments.Add(new PeopleAssignmentInstance
                        {
                            Type = PeopleAssignmentTypes.USERIDENTIFIERS,
                            Usage = PeopleAssignmentUsages.POTENTIALOWNER,
                            Value = userIdentifier
                        });
                    }
                }

                if (evt.GroupNames != null && evt.GroupNames.Any())
                {
                    foreach (var groupName in evt.GroupNames)
                    {
                        PeopleAssignments.Add(new PeopleAssignmentInstance
                        {
                            Type = PeopleAssignmentTypes.GROUPNAMES,
                            Usage = PeopleAssignmentUsages.POTENTIALOWNER,
                            Value = groupName
                        });
                    }
                }

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
                DeadLines = DeadLines.Where(_ => _.Usage == DeadlineUsages.COMPLETION).ToList();
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
            var clonedDeadlines = DeadLines
                .Where(_ => _.Name != evt.DeadLineName && _.Usage == evt.Usage)
                .Select(_ => _.Clone())
                .Cast<HumanTaskInstanceDeadLine>()
                .ToList();
            DeadLines.Clear();
            foreach(var clonedDeadline in clonedDeadlines)
            {
                DeadLines.Add(clonedDeadline);
            }

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
            if (Status == HumanTaskInstanceStatus.CREATED && PotentialOwners.Any())
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
                if (PotentialOwners.Count() == 1 && PotentialOwners.First().Type == PeopleAssignmentTypes.USERIDENTIFIERS)
                {
                    Status = HumanTaskInstanceStatus.RESERVED;
                    ActualOwner = PotentialOwners.First().Value;
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
