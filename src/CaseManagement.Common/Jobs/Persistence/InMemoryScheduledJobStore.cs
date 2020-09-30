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

        public InMemoryScheduledJobStore(ConcurrentBag<ScheduleJob> scheduleJobs)
        {
            _scheduleJobs = scheduleJobs;
        }

        public Task<SchedulingResult> TryGetNextScheduling(string jobName, CancellationToken token)
        {
            var scheduleJob = _scheduleJobs.FirstOrDefault(_ => _.JobName == jobName);
            if (scheduleJob == null)
            {
                return Task.FromResult(SchedulingResult.Ignore());
            }

            DateTime? nextUtc = null;
            if (!string.IsNullOrWhiteSpace(scheduleJob.CronExpression))
            {

                var expression = CronExpression.Parse(scheduleJob.CronExpression);
                nextUtc = expression.GetNextOccurrence(DateTime.UtcNow);
            }
            else if (scheduleJob.WaitIntervalMS != null)
            {
                nextUtc = DateTime.UtcNow.AddMilliseconds(scheduleJob.WaitIntervalMS.Value);
            }

            if (nextUtc == null)
            {
                return Task.FromResult(SchedulingResult.Ignore());
            }

            return Task.FromResult(SchedulingResult.Schedule(nextUtc.Value));
        }
    }
}
