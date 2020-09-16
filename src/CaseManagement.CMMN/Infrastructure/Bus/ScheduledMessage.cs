using System;

namespace CaseManagement.CMMN.Infrastructure.Bus
{
    public class ScheduledMessage
    {
        public string QueueName { get; set; }
        public string SerializedContent { get; set; }
        public DateTime ElapsedTime { get; set; }
    }
}
