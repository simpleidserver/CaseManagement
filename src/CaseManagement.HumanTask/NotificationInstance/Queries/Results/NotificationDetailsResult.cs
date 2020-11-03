using CaseManagement.HumanTask.Domains;
using System;

namespace CaseManagement.HumanTask.NotificationInstance.Queries.Results
{
    public class NotificationDetailsResult
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public int Priority { get; set; }
        public string PresentationName { get; set; }
        public string PresentationSubject { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime LastModifiedTime { get; set; }

        public static NotificationDetailsResult ToDto(
            NotificationInstanceAggregate notificationInstance,
            Localization.Translation name,
            Localization.Translation subject)
        {
            return new NotificationDetailsResult
            {
                Id = notificationInstance.AggregateId,
                Name = notificationInstance.NotificationName,
                Priority = notificationInstance.Priority,
                CreatedTime = notificationInstance.CreateDateTime,
                LastModifiedTime = notificationInstance.UpdateDateTime,
                PresentationName = name?.Value,
                PresentationSubject = subject?.Value,
                Status = Enum.GetName(typeof(NotificationInstanceStatus), notificationInstance.Status)
            };
        }
    }
}
