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

        public static NotificationDetailsResult ToDto(NotificationInstanceAggregate notificationInstance)
        {
            return new NotificationDetailsResult
            {
                Id = notificationInstance.AggregateId,
                Name = notificationInstance.NotificationName,
                Priority = notificationInstance.Priority,
                Status = Enum.GetName(typeof(NotificationInstanceStatus), notificationInstance.Status)
            };
        }
    }
}
