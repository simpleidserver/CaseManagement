using System;

namespace CaseManagement.CMMN.Infrastructure.Bus
{
    public class DeadLetterMessage
    {
        public string QueueName { get; set; }
        public object Content { get; set; }
        public DateTime ElapsedTime { get; set; }
    }
}
