namespace CaseManagement.Common.Jobs.Persistence
{
    public class ScheduleJob
    {
        public string JobName { get; set; }
        public string CronExpression { get; set; }
        public double? WaitIntervalMS { get; set; }

        public static ScheduleJob New<T>(string cronExpression)
        {
            return new ScheduleJob
            {
                JobName = typeof(T).FullName,
                CronExpression  = cronExpression
            };
        }

        public static ScheduleJob New<T>(double waitIntervalMS)
        {
            return new ScheduleJob
            {
                JobName = typeof(T).FullName,
                WaitIntervalMS = waitIntervalMS
            };
        }
    }
}
