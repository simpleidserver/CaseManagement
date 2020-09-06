using System;

namespace CaseManagement.CMMN.Infrastructures.Jobs.Notifications
{
    [Serializable]
    public class CasePlanInstanceNotification : BaseNotification
    {
        public CasePlanInstanceNotification(string id) : base(id) { }

        public string CasePlanInstanceId { get; set; }
    }
}
