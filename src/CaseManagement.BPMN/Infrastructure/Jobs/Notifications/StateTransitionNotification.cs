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
        public string Content { get; set; }

        [field: NonSerialized]
        public JObject JObjContent
        {
            get
            {
                if (Content == null)
                {
                    return null;
                }

                return JObject.Parse(Content);
            }
        }

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
