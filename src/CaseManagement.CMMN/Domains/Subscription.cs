using System;
using System.Collections.Generic;

namespace CaseManagement.CMMN.Domains
{
    public class Subscription
    {
        public string EventName { get; set; }
        public string CasePlanInstanceId { get; set; }
        public string CasePlanElementInstanceId { get; set; }
        public bool IsCaptured { get; set; }
        public DateTime? CaptureDateTime { get; set; }
        public DateTime CreationDateTime { get; set; }
        public Dictionary<string, string> Parameters { get; set; }
    }
}
