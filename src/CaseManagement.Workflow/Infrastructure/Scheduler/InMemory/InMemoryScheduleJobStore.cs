using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CaseManagement.Workflow.Infrastructure.Scheduler.InMemory
{
    public class InMemoryScheduleJobStore : IScheduleJobStore
    {
        private List<ScheduleJob> _scheduleJobs;

        public InMemoryScheduleJobStore()
        {
            _scheduleJobs = new List<ScheduleJob>();
        }

        public Task ScheduleJob(JobMessage message, DateTime dateTime)
        {
            lock(_scheduleJobs)
            {
                _scheduleJobs.Add(new ScheduleJob
                {
                    Id = Guid.NewGuid().ToString(),
                    Message = JsonConvert.SerializeObject(message),
                    ScheduledAt = dateTime,
                    AssemblyQualifiedName = message.GetType().AssemblyQualifiedName
                });
                return Task.CompletedTask;
            }
        }

        public Task<ScheduleJob> TakeNextJob()
        {
            var currentDateTime = DateTime.UtcNow;
            var filtered = _scheduleJobs.Where(s => s.ScheduledAt <= currentDateTime).OrderBy(s => s.ScheduledAt);
            if (!filtered.Any())
            {
                return Task.FromResult<ScheduleJob>(null);
            }

            var first = filtered.First();
            var result = (ScheduleJob)first.Clone();
            lock(_scheduleJobs)
            {
                _scheduleJobs.Remove(first);
            }

            return Task.FromResult(result);
        }
    }
}
