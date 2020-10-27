using CaseManagement.HumanTask.Domains;
using System;
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

        public static TaskInstanceDetailsResult ToDto(HumanTaskInstanceAggregate humanTaskInstance, Localization.Translation name, Localization.Translation subject)
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
                HasPotentialOwners = humanTaskInstance.PeopleAssignment.PotentialOwner != null ? humanTaskInstance.PeopleAssignment.PotentialOwner.Values.Any() : false,
                Status = Enum.GetName(typeof(HumanTaskInstanceStatus), humanTaskInstance.Status)
            };
        }
    }
}
