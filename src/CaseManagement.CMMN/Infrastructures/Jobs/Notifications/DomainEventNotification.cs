using System;
using System.Collections.Generic;

namespace CaseManagement.CMMN.Infrastructures.Jobs.Notifications
{
    [Serializable]
    public class DomainEventNotification : BaseNotification
    {
        public DomainEventNotification(string id) : base(id) 
        {
            Evts = new List<DomainEventNotificationRecord>();
        }

        public ICollection<DomainEventNotificationRecord> Evts { get; set; }
    }

    [Serializable]
    public class DomainEventNotificationRecord
    {
        public string Type { get; set; }
        public string Content { get; set; }
    }
}
