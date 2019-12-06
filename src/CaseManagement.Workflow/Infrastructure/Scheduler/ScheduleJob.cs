using System;

namespace CaseManagement.Workflow.Infrastructure.Scheduler
{
    public class ScheduleJob : ICloneable
    {
        public string Id { get; set; }
        public string Message { get; set; }
        public string AssemblyQualifiedName { get; set; }
        public DateTime ScheduledAt { get; set; }

        public object Clone()
        {
            return new ScheduleJob
            {
                Id = Id,
                Message = Message,
                AssemblyQualifiedName = AssemblyQualifiedName,
                ScheduledAt = ScheduledAt
            };
        }
    }
}
