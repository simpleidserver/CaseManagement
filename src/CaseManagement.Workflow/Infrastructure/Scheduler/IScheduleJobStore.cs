using System;
using System.Threading.Tasks;

namespace CaseManagement.Workflow.Infrastructure.Scheduler
{
    public interface IScheduleJobStore
    {
        Task<ScheduleJob> TakeNextJob();
        Task ScheduleJob(JobMessage message, DateTime dateTime);
    }
}
