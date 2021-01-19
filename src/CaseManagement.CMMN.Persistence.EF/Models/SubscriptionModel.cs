using System;

namespace CaseManagement.CMMN.Persistence.EF.Models
{
    public class SubscriptionModel
    {
        public long Id { get; set; }
        public string EventName { get; set; }
        public string CasePlanInstanceId { get; set; }
        public string CasePlanElementInstanceId { get; set; }
        public bool IsCaptured { get; set; }
        public string Parameters { get; set; }
        public DateTime? CaptureDateTime { get; set; }
        public DateTime CreationDateTime { get; set; }
    }
}
