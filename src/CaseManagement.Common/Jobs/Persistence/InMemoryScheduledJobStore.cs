using Cronos;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.Common.Jobs.Persistence
{
    public class InMemoryScheduledJobStore : IScheduledJobStore
    {
        private readonly ConcurrentBag<ScheduleJob> _scheduleJobs;

        public InMemoryScheduledJobStore()
        {
            _scheduleJobs = new ConcurrentBag<ScheduleJob>();
        }

        public Task<SchedulingResult> TryGetNextScheduling(string jobName, CancellationToken token)
        {
            var scheduleJob = _scheduleJobs.FirstOrDefault(_ => _.JobName == jobName);
            if (scheduleJob == null)
            {
                return Task.FromResult(SchedulingResult.Ignore());
            }

            var expression = CronExpression.Parse(scheduleJob.CronExpression);
            var nextUtc = expression.GetNextOccurrence(DateTime.UtcNow);
            if (nextUtc == null)
            {
                return Task.FromResult(SchedulingResult.Ignore());
            }

            return Task.FromResult(SchedulingResult.Schedule(nextUtc.Value));
        }
    }
}
