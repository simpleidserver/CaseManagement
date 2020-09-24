using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CaseManagement.BPMN.Domains
{
    public abstract class BaseFlowNode : BaseFlowElement, ICloneable
    {
        public BaseFlowNode()
        {
            Incoming = new List<string>();
            Outgoing = new List<string>();
        }

        public ICollection<string> Incoming { get; set; }
        public ICollection<string> Outgoing { get; set; }
        public abstract FlowNodeTypes FlowNode { get; }

        protected void FeedFlowNode(BaseFlowNode node)
        {
            FeedFlowElt(node);
            node.Incoming = Incoming.ToList();
            node.Outgoing = Outgoing.ToList();
        }

        public static BaseFlowNode Deserialize(string json)
        {
            var jObj = JsonConvert.DeserializeObject<JObject>(json);
            var flowNode = (FlowNodeTypes)int.Parse(jObj["FlowNode"].ToString());
            switch (flowNode)
            {
                case FlowNodeTypes.EMPTYTASK:
                    return Deserialize<EmptyTask>(json);
                case FlowNodeTypes.STARTEVENT:
                    return StartEvent.Deserialize(json);
                case FlowNodeTypes.SERVICETASK:
                    return Deserialize<ServiceTask>(json);
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
