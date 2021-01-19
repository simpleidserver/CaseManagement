using CaseManagement.Common.Jobs;
using System.Collections.Generic;

namespace CaseManagement.CMMN.Infrastructure.Jobs.Notifications
{
    public class ExternalEventNotification : BaseNotification
    {
        public ExternalEventNotification(string id) : base(id) { }

        public string EvtName { get; set; }
        public string CasePlanInstanceId { get; set; }
        public string CasePlanElementInstanceId { get; set; }
        public Dictionary<string, string> Parameters { get; set; }
    }
}
