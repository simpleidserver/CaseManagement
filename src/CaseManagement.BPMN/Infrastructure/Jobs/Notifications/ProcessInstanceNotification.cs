using CaseManagement.Common.Jobs;
using System;

namespace CaseManagement.BPMN.Infrastructure.Jobs.Notifications
{
    [Serializable]
    public class ProcessInstanceNotification : BaseNotification
    {
        public ProcessInstanceNotification(string id) : base(id) { }

        public string ProcessInstanceId { get; set; }
    }
}
