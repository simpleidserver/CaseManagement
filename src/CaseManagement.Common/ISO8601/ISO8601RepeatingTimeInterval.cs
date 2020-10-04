
namespace CaseManagement.Common.ISO8601
{
    public class ISO8601RepeatingTimeInterval
    {
        public ISO8601RepeatingTimeInterval(int recurringTimeInterval, ISO8601TimeInterval interval)
        {
            RecurringTimeInterval = recurringTimeInterval;
            Interval = interval;
        }

        public int RecurringTimeInterval { get; set; }
        public ISO8601TimeInterval Interval { get; set; }
    }
}
