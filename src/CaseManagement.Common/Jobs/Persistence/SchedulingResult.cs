using System;

namespace CaseManagement.Common.Jobs.Persistence
{
    public class SchedulingResult
    {
        private SchedulingResult(DateTime? nextDateTime = null)
        {
            NextDateTime = nextDateTime;
        }

        public DateTime? NextDateTime { get; set; }

        public static SchedulingResult Ignore()
        {
            return new SchedulingResult();
        }

        public static SchedulingResult Schedule(DateTime nextDateTime)
        {
            return new SchedulingResult(nextDateTime: nextDateTime);
        }
    }
}
