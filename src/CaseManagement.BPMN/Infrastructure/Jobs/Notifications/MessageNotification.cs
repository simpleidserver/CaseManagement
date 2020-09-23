using CaseManagement.Common.Jobs;
using System;

namespace CaseManagement.BPMN.Infrastructure.Jobs.Notifications
{
    [Serializable]
    public class MessageNotification : BaseNotification
    {
        public MessageNotification(string id) : base(id)
        {
        }

        public string MessageName { get; set; }
        public string ProcessInstanceId { get; set; }
    }
}
