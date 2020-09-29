using CaseManagement.Common.Domains;
using CaseManagement.Common.Exceptions;
using CaseManagement.HumanTask.Resources;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CaseManagement.HumanTask.Domains
{
    public class HumanTaskInstanceAggregate : BaseAggregate
    {
        public HumanTaskInstanceAggregate()
        {
            OperationParameters = new Dictionary<string, string>();
        }

        public HumanTaskInstanceStates State { get; set; }
        public string HumanTaskDefName { get; set; }
        public string ActualOwner { get; set; }
        public Dictionary<string, string> OperationParameters { get; set; }
        public int Priority { get; set; }
        public DateTime? ActivationDeferralTime { get; set; }
        public DateTime? ExpirationTime { get; set; }
        public TaskPeopleAssignment PeopleAssignment { get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime UpdateDateTime { get; set; }

        public static HumanTaskInstanceAggregate New(string humanTaskDefName, Dictionary<string, string> operationParameters, TaskPeopleAssignment peopleAssignment, int priority, DateTime? activationDeferralTime, DateTime? expirationTime)
        {
            var id = Guid.NewGuid().ToString();
            var evt = new HumanTaskInstanceCreatedEvent(Guid.NewGuid().ToString(), id, 0, humanTaskDefName, DateTime.UtcNow, operationParameters, peopleAssignment, priority, activationDeferralTime, expirationTime);
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
                State = State,
                HumanTaskDefName = HumanTaskDefName,
                ActualOwner = ActualOwner,
                OperationParameters = OperationParameters.ToDictionary(kvp => kvp.Key, kvp => kvp.Value),
                Priority = Priority,
                ActivationDeferralTime = ActivationDeferralTime,
                ExpirationTime = ExpirationTime,
                PeopleAssignment = (TaskPeopleAssignment)PeopleAssignment?.Clone(),
                CreateDateTime = CreateDateTime,
                UpdateDateTime = UpdateDateTime
            };
        }

        public string GetStreamName()
        {
            return GetStreamName(AggregateId);
        }

        #region Operations

        public void UpdatePotentialOwners(GroupNamesAssignment assign)
        {
            var evt = new PotentialOwnerGroupNamesAssignedEvent(Guid.NewGuid().ToString(), AggregateId, Version + 1, assign.GroupNames, DateTime.UtcNow);
            Handle(evt);
            DomainEvents.Add(evt);
        }

        public void UpdatePotentialOwners(UserIdentifiersAssignment assign)
        {
            var evt = new PotentialOwnerUserIdentifiersAssignedEvent(Guid.NewGuid().ToString(), AggregateId, Version + 1, assign.UserIdentifiers, DateTime.UtcNow);
            Handle(evt);
            DomainEvents.Add(evt);
        }

        public void Activate()
        {
            var evt = new ActivatedEvent(Guid.NewGuid().ToString(), AggregateId, Version + 1, DateTime.UtcNow);
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
            AggregateId = evt.AggregateId;
            State = HumanTaskInstanceStates.CREATED;
            HumanTaskDefName = evt.HumanTaskDefName;
            Priority = evt.Priority;
            ActivationDeferralTime = evt.ActivationDeferralTime;
            ExpirationTime = evt.ExpirationTime;
            PeopleAssignment = evt.PeopleAssignment;
            OperationParameters = evt.OperationParameters;
            UpdateDateTime = evt.CreateDateTime;
            CreateDateTime = evt.CreateDateTime;
            Version = evt.Version;
        }

        private void Handle(PotentialOwnerGroupNamesAssignedEvent evt)
        {
            PeopleAssignment = new TaskPeopleAssignment
            {
                PotentialOwner = new GroupNamesAssignment
                {
                    GroupNames = evt.GroupNames
                }
            };
            UpdateState();
            UpdateDateTime = evt.UpdateDateTime;
            Version = evt.Version;
        }

        private void Handle(PotentialOwnerUserIdentifiersAssignedEvent evt)
        {
            PeopleAssignment = new TaskPeopleAssignment
            {
                PotentialOwner = new UserIdentifiersAssignment
                {
                    UserIdentifiers = evt.UserIdentifiers
                }
            };
            UpdateState();
            UpdateDateTime = evt.UpdateDateTime;
            Version = evt.Version;
        }

        private void Handle(ActivatedEvent evt)
        {
            var errors = new List<KeyValuePair<string, string>>();
            if (State != HumanTaskInstanceStates.CREATED)
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

            UpdateState();
            UpdateDateTime = evt.UpdateDateTime;
            Version = evt.Version;
        }

        #endregion

        private void UpdateState()
        {
            if (State == HumanTaskInstanceStates.CREATED)
            {
                if (ActivationDeferralTime != null && DateTime.UtcNow >= ActivationDeferralTime.Value)
                {
                    State = HumanTaskInstanceStates.READY;
                }
            }

            if (State == HumanTaskInstanceStates.READY)
            {
                if (PeopleAssignment == null && PeopleAssignment.PotentialOwner != null)
                {
                    var userAssignment = PeopleAssignment.PotentialOwner as UserIdentifiersAssignment;
                    if (userAssignment != null && userAssignment.UserIdentifiers.Count() == 1)
                    {
                        State = HumanTaskInstanceStates.RESERVED;
                        ActualOwner = userAssignment.UserIdentifiers.First();
                    }
                }
            }
        }
    }
}
