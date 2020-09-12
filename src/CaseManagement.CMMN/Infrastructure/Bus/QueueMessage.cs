using System;

namespace CaseManagement.CMMN.Infrastructure.Bus
{
    public class QueueMessage
    {
        public long Id { get; set; }
        public string QueueName { get; set; }
        public string SerializedContent { get; set; }
        public DateTime CreationDateTime { get; set; }
    }
}
