using CaseManagement.HumanTask.Domains;
using System;

namespace CaseManagement.HumanTask.HumanTaskInstance.Queries.Results
{
    public class TaskInstanceDetailsResult
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public int Priority { get; set; }

        public static TaskInstanceDetailsResult ToDto(HumanTaskInstanceAggregate humanTaskInstance)
        {
            return new TaskInstanceDetailsResult
            {
                Id = humanTaskInstance.AggregateId,
                Name = humanTaskInstance.HumanTaskDefName,
                Priority = humanTaskInstance.Priority,
                Status = Enum.GetName(typeof(HumanTaskInstanceStatus), humanTaskInstance.Status)
            };
        }
    }
}
