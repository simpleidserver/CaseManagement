using System;

namespace CaseManagement.Common.Bus
{
    public class ScheduledMessage
    {
        public string QueueName { get; set; }
        public string SerializedContent { get; set; }
        public DateTime ElapsedTime { get; set; }
    }
}
