using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace CaseManagement.BPMN.Domains
{
    public class SerializedFlowNodeDefinition: ICloneable
    {
        public string SerializedContent { get; set; }
        public FlowNodeTypes Type { get; set; }

        #region Operations

        public BaseFlowNode Deserialize()
        {
            return Deserialize(SerializedContent, Type);
        }

        #endregion

        public static SerializedFlowNodeDefinition Create(string serializedContent)
        {
            return new SerializedFlowNodeDefinition
            {
                SerializedContent = serializedContent,
                Type = Parse(serializedContent)
            };
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

        public static FlowNodeTypes Parse(string json)
        {
            var jObj = JsonConvert.DeserializeObject<JObject>(json);
            return (FlowNodeTypes)int.Parse(jObj["FlowNode"].ToString());
        }

        public static TElt Deserialize<TElt>(string json) where TElt : BaseFlowElement
        {
            return JsonConvert.DeserializeObject<TElt>(json);
        }

        public object Clone()
        {
            return new SerializedFlowNodeDefinition
            {
                Type = Type,
                SerializedContent = SerializedContent
            };
        }
    }
}
