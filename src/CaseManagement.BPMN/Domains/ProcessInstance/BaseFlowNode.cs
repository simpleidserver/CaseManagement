using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace CaseManagement.BPMN.Domains
{
    public abstract class BaseFlowNode : BaseFlowElement, ICloneable
    {
        public abstract FlowNodeTypes FlowNode { get; }

        protected void FeedFlowNode(BaseFlowNode node)
        {
            FeedFlowElt(node);
        }

        public static BaseFlowNode Deserialize(string json)
        {
            var jObj = JsonConvert.DeserializeObject<JObject>(json);
            var flowNode = (FlowNodeTypes)int.Parse(jObj["FlowNode"].ToString());
            return Deserialize(json, flowNode);
        }

        public static BaseFlowNode Deserialize(string json, FlowNodeTypes flowNode)
        {
            switch (flowNode)
            {
                case FlowNodeTypes.EMPTYTASK:
                    return Deserialize<EmptyTask>(json);
                case FlowNodeTypes.STARTEVENT:
                    return StartEvent.Deserialize(json);
                case FlowNodeTypes.SERVICETASK:
                    return Deserialize<ServiceTask>(json);
                case FlowNodeTypes.EXCLUSIVEGATEWAY:
                    return Deserialize<ExclusiveGateway>(json);
                case FlowNodeTypes.PARALLELGATEWAY:
                    return Deserialize<ParallelGateway>(json);
                case FlowNodeTypes.INCLUSIVEGATEWAY:
                    return Deserialize<InclusiveGateway>(json);
                case FlowNodeTypes.USERTASK:
                    return Deserialize<UserTask>(json);
            }

            return null;
        }

        public static TElt Deserialize<TElt>(string json) where TElt : BaseFlowElement
        {
            return JsonConvert.DeserializeObject<TElt>(json);
        }


        public abstract object Clone();
    }
}
