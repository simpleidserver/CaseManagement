namespace CaseManagement.Common.Jobs.Persistence
{
    public class ScheduleJob
    {
        public string JobName { get; set; }
        public string CronExpression { get; set; }
    }
}
