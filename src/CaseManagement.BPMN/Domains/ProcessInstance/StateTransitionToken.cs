using Newtonsoft.Json.Linq;
using System;

namespace CaseManagement.BPMN.Domains
{
    public class StateTransitionToken : ICloneable
    {
        public string FlowNodeInstanceId { get; set; }
        public string StateTransition { get; set; }
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

        public static StateTransitionToken Create(string flowNodeInstanceId, string stateTransition, string content)
        {
            return new StateTransitionToken
            {
                FlowNodeInstanceId = flowNodeInstanceId,
                StateTransition = stateTransition,
                Content = content
            };
        }

        public object Clone()
        {
            return new StateTransitionToken
            {
                FlowNodeInstanceId = FlowNodeInstanceId,
                StateTransition = StateTransition,
                Content = Content
            };
        }
    }
}
