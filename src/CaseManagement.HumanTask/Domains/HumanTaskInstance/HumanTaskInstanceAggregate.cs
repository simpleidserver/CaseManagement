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
            OperationParameters = new Dictionary<string, string>();
            EventHistories = new ConcurrentBag<HumanTaskInstanceEventHistory>();
        }

        public HumanTaskInstanceStatus Status { get; set; }
        public string HumanTaskDefName { get; set; }
        public string ActualOwner { get; set; }
        public Dictionary<string, string> OperationParameters { get; set; }
        public int Priority { get; set; }
        public DateTime? ActivationDeferralTime { get; set; }
        public DateTime? ExpirationTime { get; set; }
        public TaskPeopleAssignment PeopleAssignment { get; set; }
        public ConcurrentBag<HumanTaskInstanceEventHistory> EventHistories { get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime UpdateDateTime { get; set; }

        public static HumanTaskInstanceAggregate New(string userPrincipal, string humanTaskDefName, Dictionary<string, string> operationParameters, TaskPeopleAssignment peopleAssignment, int priority, DateTime? activationDeferralTime, DateTime? expirationTime)
        {
            var id = Guid.NewGuid().ToString();
            var evt = new HumanTaskInstanceCreatedEvent(Guid.NewGuid().ToString(), id, 0, humanTaskDefName, DateTime.UtcNow, operationParameters, peopleAssignment, priority, userPrincipal, activationDeferralTime, expirationTime);
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
                OperationParameters = OperationParameters.ToDictionary(kvp => kvp.Key, kvp => kvp.Value),
                Priority = Priority,
                ActivationDeferralTime = ActivationDeferralTime,
                ExpirationTime = ExpirationTime,
                PeopleAssignment = (TaskPeopleAssignment)PeopleAssignment?.Clone(),
                EventHistories = new ConcurrentBag<HumanTaskInstanceEventHistory>(EventHistories.Select(_ => (HumanTaskInstanceEventHistory)_.Clone())),
                CreateDateTime = CreateDateTime,
                UpdateDateTime = UpdateDateTime
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
            using(var evtHistory = AddEventHistory(evt.Id, evt.CreateDateTime, HumanTaskInstanceEventTypes.CREATED, evt.UserPrincipal))
            {
                AggregateId = evt.AggregateId;
                Status = HumanTaskInstanceStatus.CREATED;
                HumanTaskDefName = evt.HumanTaskDefName;
                Priority = evt.Priority;
                ActivationDeferralTime = evt.ActivationDeferralTime;
                ExpirationTime = evt.ExpirationTime;
                PeopleAssignment = evt.PeopleAssignment;
                OperationParameters = evt.OperationParameters;
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
            PeopleAssignment assign;
            if (evt.GroupNames != null && evt.GroupNames.Any())
            {
                assign = new GroupNamesAssignment
                {
                    GroupNames = evt.GroupNames
                };
            }
            else
            {
                assign = new UserIdentifiersAssignment
                {
                    UserIdentifiers = evt.UserIdentifiers
                };
            }

            using (var evtHistory = AddEventHistory(evt.Id, evt.UpdateDateTime, HumanTaskInstanceEventTypes.ACTIVATE, evt.UserPrincipal))
            {
                PeopleAssignment = new TaskPeopleAssignment
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
                ActualOwner = evt.UserPrincipal;
                UpdateDateTime = evt.ExecutionDateTime;
                Version = evt.Version;
            }
        }

        #endregion

        private void UpdateStatus()
        {
            if (Status == HumanTaskInstanceStatus.CREATED && PeopleAssignment != null && PeopleAssignment.PotentialOwner != null)
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
                var userAssignment = PeopleAssignment.PotentialOwner as UserIdentifiersAssignment;
                if (userAssignment != null && userAssignment.UserIdentifiers.Count() == 1)
                {
                    Status = HumanTaskInstanceStatus.RESERVED;
                    ActualOwner = userAssignment.UserIdentifiers.First();
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
