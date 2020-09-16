using System;

namespace CaseManagement.CMMN.Persistence.EF.Models
{
    public class ScheduledMessageModel
    {
        public long Id { get; set; }
        public string QueueName { get; set; }
        public string SerializedContent { get; set; }
        public DateTime ElapsedTime { get; set; }
    }
}
