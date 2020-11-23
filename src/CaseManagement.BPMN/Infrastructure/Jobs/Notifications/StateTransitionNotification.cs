using CaseManagement.Common.Jobs;
using Newtonsoft.Json.Linq;
using System;

namespace CaseManagement.BPMN.Infrastructure.Jobs.Notifications
{
    [Serializable]
    public class StateTransitionNotification : BaseNotification, ICloneable
    {
        public StateTransitionNotification(string id) : base(id) { }

        public string ProcessInstanceId { get; set; }
        public string FlowNodeInstanceId { get; set; }
        public string State { get; set; }
        public JObject Content { get; set; }

        public object Clone()
        {
            return new StateTransitionNotification(Id)
            {
                ProcessInstanceId = ProcessInstanceId,
                FlowNodeInstanceId = FlowNodeInstanceId,
                State = State,
                Content = Content,
                NbRetry = NbRetry
            };
        }
    }
}
