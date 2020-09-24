using CaseManagement.BPMN.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace CaseManagement.BPMN.Domains
{
    public abstract class BaseEventDefinition : BaseElement, ICloneable
    {
        public abstract object Clone();
        public abstract EvtDefTypes Type { get; }

        public virtual bool IsSatisfied(ProcessInstanceAggregate processInstance, BaseToken token)
        {
            return false;
        }

        public static BaseEventDefinition Deserialize(JObject jObj)
        {
            var type = (EvtDefTypes)int.Parse(jObj["Type"].ToString());
            switch(type)
            {
                case EvtDefTypes.MESSAGE:
                    return JsonConvert.DeserializeObject<MessageEventDefinition>(jObj.ToString());
                case EvtDefTypes.SIGNAL:
                    return JsonConvert.DeserializeObject<SignalEventDefinition>(jObj.ToString());
                case EvtDefTypes.TIMER:
                    return JsonConvert.DeserializeObject<TimerEventDefinition>(jObj.ToString());
            }

            return null;
        }
    }
}
