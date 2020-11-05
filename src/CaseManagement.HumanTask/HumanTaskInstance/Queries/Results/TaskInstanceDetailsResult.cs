using CaseManagement.HumanTask.Authorization;
using CaseManagement.HumanTask.Domains;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CaseManagement.HumanTask.HumanTaskInstance.Queries.Results
{
    public class TaskInstanceDetailsResult
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public int Priority { get; set; }
        public string ActualOwner { get; set; }
        public DateTime? ActivationDeferralTime { get; set; }
        public DateTime? ExpirationTime { get; set; }
        public string PresentationName { get; set; }
        public string PresentationSubject { get; set; }
        public string ParentTaskId { get; set; }
        public bool RenderingMethodExists { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime LastModifiedTime { get; set; }
        public bool HasPotentialOwners { get; set; }
        public ICollection<string> PossibleActions { get; set; }

        public enum TaskinstanceActions
        {
            NOMINATE = 0,
            CLAIM = 1,
            START = 2,
            COMPLETE = 3
        }

        public static TaskInstanceDetailsResult ToDto(
            HumanTaskInstanceAggregate humanTaskInstance, 
            Localization.Translation name, 
            Localization.Translation subject, 
            ICollection<UserRoles> userRoles,
            string currentUser)
        {
            return new TaskInstanceDetailsResult
            {
                Id = humanTaskInstance.AggregateId,
                Name = humanTaskInstance.HumanTaskDefName,
                Priority = humanTaskInstance.Priority,
                ActualOwner = humanTaskInstance.ActualOwner,
                ExpirationTime = humanTaskInstance.ExpirationTime,
                ActivationDeferralTime = humanTaskInstance.ActivationDeferralTime,
                PresentationName = name?.Value,
                PresentationSubject = subject?.Value,
                ParentTaskId = humanTaskInstance.ParentHumanTaskId,
                CreatedTime = humanTaskInstance.CreateDateTime,
                LastModifiedTime = humanTaskInstance.UpdateDateTime,
                HasPotentialOwners = humanTaskInstance.PotentialOwners.Any(),
                Status = Enum.GetName(typeof(HumanTaskInstanceStatus), humanTaskInstance.Status),
                PossibleActions = GetPossibleActions(
                    humanTaskInstance.Status,
                    humanTaskInstance.ActualOwner,
                    userRoles,
                    currentUser).Select(_ => _.ToString()).ToList()
            };
        }

        private static ICollection<TaskinstanceActions> GetPossibleActions(HumanTaskInstanceStatus status, string actualOwner, ICollection<UserRoles> userRoles, string currentUser)
        {
            var result = new List<TaskinstanceActions>();
            if (userRoles == null || string.IsNullOrWhiteSpace(currentUser))
            {
                return result;
            }

            if (status == HumanTaskInstanceStatus.CREATED)
            {
                if (userRoles.Contains(UserRoles.BUSINESSADMINISTRATOR))
                {
                    result.Add(TaskinstanceActions.NOMINATE);
                }
            }

            if (status == HumanTaskInstanceStatus.READY)
            {
                if (userRoles.Contains(UserRoles.POTENTIALOWNER))
                {
                    result.Add(TaskinstanceActions.CLAIM);
                }
            }

            if (status == HumanTaskInstanceStatus.RESERVED)
            {
                if (actualOwner == currentUser)
                {
                    result.Add(TaskinstanceActions.START);
                }
            }

            if (status == HumanTaskInstanceStatus.INPROGRESS)
            {
                if (actualOwner == currentUser)
                {
                    result.Add(TaskinstanceActions.COMPLETE);
                }
            }

            return result;
        }
    }
}
