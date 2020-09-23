using CaseManagement.Common.Jobs;
using System;

namespace CaseManagement.BPMN.Infrastructure.Jobs.Notifications
{
    [Serializable]
    public class ProcessInstanceNotification : BaseNotification
    {
        public ProcessInstanceNotification(string id) : base(id) 
        {
            IsNewInstance = true;
        }

        public string ProcessInstanceId { get; set; }
        public bool IsNewInstance { get; set; }
    }
}
