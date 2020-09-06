using System;

namespace CaseManagement.CMMN.Infrastructures.ExternalEvts
{
    public class Subscription
    {
        public string EventName { get; set; }
        public string CasePlanInstanceId { get; set; }
        public string CasePlanElementInstanceId { get; set; }
        public bool IsCaptured { get; set; }
        public DateTime CaptureDateTime { get; set; }
        public DateTime CreationDateTime { get; set; }
    }
}
